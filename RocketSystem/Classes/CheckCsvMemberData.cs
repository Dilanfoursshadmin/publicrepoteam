using RocketSystem.DbLink;
using RocketSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RocketSystem.Classes
{
    public class CheckCsvMemberData
    {
        private static DataAccessLayer db = new DataAccessLayer();

        public static List<int> csvMemberData()
        {
            using(DataAccessLayer db = new DataAccessLayer())
            {
                //var lastCheckPosition = db.LastPositionDetails.Max(x => x.positionId);
                //var csvDataList = db.CsvDatas.Where(x => x.status == "pending").GroupBy(x => x.detailTwo).Select(g => new { Id = g.Key, csvDataId = g.Select(x => x.csvDataId) }).ToList();
                var csvDataList = db.CsvDatas.Where(x => x.status == "pending").GroupBy(x => x.detailTwo).Select(x => x.FirstOrDefault()).OrderBy(x => x.csvDataId).ToList();
                //var memberList = db.PositionDetails.Where(x => x.paymentStatus == 0 && x.systemUpdate == 0).ToList();
                int positionDetails = 0;
                // var csvDataList = db.CsvDatas.Where(x => x.csvDataId <= 1).ToList();
                List<int> list = new List<int>();
                foreach (var value in csvDataList)
                {
                    //if csvData has been duplicated..reject both data
                    string duplicateMember = checkMemberDuplicate(value.detailTwo);
                    if (duplicateMember == "Member_Duplicated")
                    {
                        //Reject user
                        string memberId = getMemberFromUser(value.detailTwo);
                        addCsvPositionDetail(value.csvDataId, 0);
                        int csvAmount = db.CsvDatas.Where(x => x.csvDataId == value.csvDataId).Select(x => x.acceptedamount).FirstOrDefault();
                        insertMemberBalanceTransactionTable(duplicateMember, csvAmount, "credit");
                        int amount = Convert.ToInt32(value.acceptedamount);
                        int memberBalanceTransactionId = db.MemberBalanceTransactions.OrderByDescending(p => p.memberBalanceTransactionId).FirstOrDefault().memberBalanceTransactionId;
                        insertBalanceCsvTransactionTable(memberBalanceTransactionId, value.csvDataId, 0, 0);
                        updateCsvData(value.csvDataId, "Rejected - User_Duplicated"); 
                    }
                    else
                    {
                        //if member has not yet registered.
                        string memberId = getMemberFromUser(value.detailTwo);
                        if (memberId == "Member_Unavailable")//if member not available reject user
                        {
                            //Reject User
                            addCsvPositionDetail(value.csvDataId, 0);
                            insertMemberBalanceTransactionTable("Not Registered", value.acceptedamount, "credit");
                            int memberBalanceTransactionId = db.MemberBalanceTransactions.OrderByDescending(p => p.memberBalanceTransactionId).FirstOrDefault().memberBalanceTransactionId;
                            insertBalanceCsvTransactionTable(memberBalanceTransactionId, value.csvDataId, 0, 0);
                            updateCsvData(value.csvDataId, "Rejected - User_not_Available");
                        }
                        else if (memberId == "Invalid_Data")//if member details are invalid
                        {
                            //Reject User
                            addCsvPositionDetail(value.csvDataId, 0);
                            insertMemberBalanceTransactionTable("Not Registered", value.acceptedamount, "credit");
                            int memberBalanceTransactionId = db.MemberBalanceTransactions.OrderByDescending(p => p.memberBalanceTransactionId).FirstOrDefault().memberBalanceTransactionId;
                            insertBalanceCsvTransactionTable(memberBalanceTransactionId, value.csvDataId, 0, 0);
                            updateCsvData(value.csvDataId, "Rejected - Invalid_Data");
                        }
                        else
                        {
                            List<PositionDetail> positionDetailList2 = new List<PositionDetail>();
                            //If User applies for package One
                            var packageOnePositionList = db.PositionDetails.Where(x => x.membershipNo == memberId && x.depositDate == value.csvDate && x.paymentStatus == 0 && x.systemUpdate == 0 && x.positionStatus == "pending" && x.package == "1" && x.positionId != 0).OrderByDescending(x => x.positionId).ToList();
                            if(packageOnePositionList.Count != 0)
                            {
                                positionDetailList2.AddRange(packageOnePositionList);
                            }
                            //If User applies for package Two
                            var packagTwoPositionList = db.PositionDetails.Where(x => x.membershipNo == memberId && x.depositDate == value.csvDate && x.paymentStatus == 0 && x.systemUpdate == 0 && x.positionStatus == "pending" && x.package == "2" && x.positionId != 0).OrderByDescending(x => x.positionId).ToList();
                            if (packagTwoPositionList.Count != 0)
                            {
                                positionDetailList2.AddRange(packagTwoPositionList);
                            }
                            //if member has been registered but not applied for any positions
                            if (positionDetailList2.Count != 0)
                            {
                                value.acceptedamount = db.CsvDatas.Where(x => x.detailTwo == value.detailTwo && x.csvDate == value.csvDate).Select(x => x.acceptedamount).Sum();
                                List<PositionDetail> positionDetailList = positionDetailList2.OrderBy(x => x.positionPriority).ToList();
                                string positionStatus = compareCsvAmountWithPositionApplied(positionDetailList, value.acceptedamount);
                                if (positionStatus == "Amount_Enough")
                                {
                                    foreach (var positionDetailValue in positionDetailList)
                                    {
                                        addCsvPositionDetail(value.csvDataId, positionDetailValue.positionId);
                                        list.Add(positionDetailValue.positionId);
                                        updatePaymentStatus(positionDetailValue.positionId, positionDetailValue.positionCount);
                                        updatePositionStatus(positionDetailValue.positionId, "Accepted");
                                    }
                                    positionDetails = 1;
                                    var csvAcceptedAmountList = db.CsvDatas.Where(x => x.detailTwo == value.detailTwo && x.csvDate == value.csvDate).ToList();
                                    foreach (var value2 in csvAcceptedAmountList)
                                    {
                                        updateCsvData(value2.csvDataId, "Accepted");
                                    }
                                    
                                }
                                else if (positionStatus == "Amount_More")
                                {
                                    int balance = calculateBalance(value.acceptedamount, positionDetailList);
                                    insertMemberBalanceTransactionTable(memberId, balance, "credit");
                                    int memberBalanceTransactionId = db.MemberBalanceTransactions.Where(x => x.memberId == memberId && x.balanceAmount == balance).OrderByDescending(p => p.memberBalanceTransactionId).FirstOrDefault().memberBalanceTransactionId;
                                    foreach (var positionDetailValue in positionDetailList)
                                    {
                                        addCsvPositionDetail(value.csvDataId, positionDetailValue.positionId);
                                        insertBalanceCsvTransactionTable(memberBalanceTransactionId, value.csvDataId, positionDetailValue.positionId, 0);
                                        list.Add(positionDetailValue.positionId);
                                        updatePaymentStatus(positionDetailValue.positionId, positionDetailValue.positionCount);
                                        updatePositionStatus(positionDetailValue.positionId, "Accepted"); 
                                    }
                                    var csvAcceptedAmountList = db.CsvDatas.Where(x => x.detailTwo == value.detailTwo && x.csvDate == value.csvDate).ToList();
                                    foreach (var value2 in csvAcceptedAmountList)
                                    {
                                        updateCsvData(value2.csvDataId, "Accepted - Balance_Remain");
                                    }
                                    positionDetails = 1;
                                }
                                else if (positionStatus == "Amount_Not_Enough")
                                {
                                    int finalPositionCount = 0;
                                    int depositAmount = value.acceptedamount;
                                    int grandFinalPositionCount = 0;
                                    int listAdded = 0;
                                    foreach (var positionDetailValue in positionDetailList)
                                    {
                                        int positionCount = calculatePositionForAmountPaid(positionDetailValue.positionId, depositAmount);
                                        int package = Convert.ToInt32(positionDetailValue.package);
                                        int pakagePrize = db.Packages.Where(x => x.packageId == package).Select(x => x.packagePrize).SingleOrDefault();
                                        int positionCost = positionCount * pakagePrize;
                                        depositAmount = depositAmount - positionCost;
                                        finalPositionCount = finalPositionCount + positionCount;
                                        grandFinalPositionCount = grandFinalPositionCount + finalPositionCount;
                                        updatePaymentStatus(positionDetailValue.positionId, finalPositionCount);
                                        finalPositionCount = 0;
                                        addCsvPositionDetail(value.csvDataId, positionDetailValue.positionId);
                                        if(positionDetailValue.positionCount == positionCount)
                                        {
                                            updatePositionStatus(positionDetailValue.positionId, "Accepted");
                                        }
                                        else
                                        {
                                            updatePositionStatus(positionDetailValue.positionId, "Accepted - Position_Remain");
                                        }
                                        list.Add(positionDetailValue.positionId);
                                    }
                                    if (depositAmount != 0)
                                    {
                                        insertMemberBalanceTransactionTable(memberId, depositAmount, "credit");
                                        int memberBalanceTransactionId = db.MemberBalanceTransactions.Where(x => x.memberId == memberId && x.balanceAmount == depositAmount).OrderByDescending(p => p.memberBalanceTransactionId).FirstOrDefault().memberBalanceTransactionId;
                                        foreach (var positionDetailValue in positionDetailList)
                                        {
                                            insertBalanceCsvTransactionTable(memberBalanceTransactionId, value.csvDataId, positionDetailValue.positionId, 0);
                                            
                                        }
                                    }
                                    else { }
                                    var csvAcceptedAmountList = db.CsvDatas.Where(x => x.detailTwo == value.detailTwo && x.csvDate == value.csvDate).ToList();
                                    foreach (var value2 in csvAcceptedAmountList)
                                    {
                                        updateCsvData(value2.csvDataId, "Accepted - Position_Remain");
                                    }
                                    
                                    if (finalPositionCount == 0)
                                    {
                                        //Reject user
                                        insertMemberBalanceTransactionTable(memberId, value.acceptedamount, "credit");
                                        int amount = Convert.ToInt32(value.acceptedamount);
                                        int memberBalanceTransactionId = db.MemberBalanceTransactions.Where(x => x.memberId == memberId && x.balanceAmount == amount).OrderByDescending(p => p.memberBalanceTransactionId).FirstOrDefault().memberBalanceTransactionId;
                                        foreach (var positionDetailValue in positionDetailList)
                                        {
                                            addCsvPositionDetail(value.csvDataId, positionDetailValue.positionId);
                                            insertRejectTable(positionDetailValue.positionId);
                                            insertBalanceCsvTransactionTable(memberBalanceTransactionId, value.csvDataId, positionDetailValue.positionId, 0); 
                                        }
                                        foreach (var value2 in csvAcceptedAmountList)
                                        {
                                            updateCsvData(value2.csvDataId, "Rejected - Amount_Not_Enough");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Reject User
                                addCsvPositionDetail(value.csvDataId, 0);
                                insertMemberBalanceTransactionTable("Not Registered", value.acceptedamount, "credit");
                                int memberBalanceTransactionId = db.MemberBalanceTransactions.OrderByDescending(p => p.memberBalanceTransactionId).FirstOrDefault().memberBalanceTransactionId;
                                insertBalanceCsvTransactionTable(memberBalanceTransactionId, value.csvDataId, 0, 0);
                                var csvAcceptedAmountList = db.CsvDatas.Where(x => x.detailTwo == value.detailTwo && x.csvDate == value.csvDate).ToList();
                                foreach (var value2 in csvAcceptedAmountList)
                                {
                                    updateCsvData(value2.csvDataId, "Rejected - No_Positions_Applied");
                                }
                            }
                        }
                    }
                }
                if (positionDetails != 0)
                {
                    var maxPosition = db.PositionDetails.OrderByDescending(u => u.positionId).Select(x => x.positionId).FirstOrDefault();
                    insertLastPostion(maxPosition);
                }
                //check Order
                List<int> finalList = orderList(list);
                return finalList;
            }
        }

        public static List<int> orderList(List<int> positionDetails)
        {
            List<int> finalList = new List<int>();
            foreach(var positionDetailsValue in positionDetails)
            {
                if (!finalList.Contains(positionDetailsValue))
                {
                    var positionId = db.PositionDetails.Where(x => x.positionId == positionDetailsValue).Select(x => x.positionId).FirstOrDefault();
                    string introducePromoCode = db.PositionDetails.Where(x => x.positionId == positionId).Select(x => x.introducePromoCode).FirstOrDefault();// get introduce promo code
                    var stageOneAvailable = db.StageOnes.Where(x => x.introducePromoCode == introducePromoCode).ToList(); // check if introducer available in stageOne
                    var positionDetailAbove = db.TemporaryPositions.Where(x => x.positionCode == introducePromoCode && x.positionId < positionId).ToList();//check if introducer has applied before the member
                    int positionDetailBelow = db.TemporaryPositions.Where(x => x.positionCode == introducePromoCode && x.positionId >= positionId).Select(x => x.positionId).FirstOrDefault();//check if introducer has applied after the member
                    if (stageOneAvailable.Count != 0)
                    {
                        // add to list 
                        finalList.Add(positionId);
                    }
                    else if (positionDetailAbove.Count != 0)
                    {
                        // add to list
                        finalList.Add(positionId);
                    }
                    else if (positionDetailBelow != 0)
                    {
                        finalList.Add(positionDetailBelow); // add to introducer list
                        finalList.Add(positionId);  //then add introducee
                    }
                }
               // int positionDetailsValue = Convert.ToInt32(positionDetails[count]);
            }
            return finalList;
        }

        //check if user has been registered and return the memberId
        public static string getMemberFromUser(string birthdayName)
        {
            birthdayName = birthdayName.Replace(" ", "");
            using (DataAccessLayer db = new DataAccessLayer())
            {
                if (birthdayName.Length < 8 || birthdayName == "NoValue")
                {
                    return "Invalid_Data";
                }
                else
                {
                    string birthday = birthdayName.Substring(birthdayName.Length - 8);
                    string memberName = birthdayName.Substring(0, birthdayName.Length - 8);
                    DateTime userBirthday = DateTime.ParseExact(birthday, "yyyyMMdd", CultureInfo.InvariantCulture);
                    var memberId = db.Users.Where(x => x.dateOfBirth == userBirthday && x.katakanaName == memberName).Select(x => x.membershipNo).FirstOrDefault();
                    if (memberId != null)
                    {
                        return memberId;
                    }
                    else
                    {
                        return "Member_Unavailable";
                    }
                }
            }
        }

        //check if member has been duplicated
        public static string checkMemberDuplicate(string birthdayName)
        {
            birthdayName = birthdayName.Replace(" ", "");
            using (DataAccessLayer db = new DataAccessLayer())
            {
                if (birthdayName.Length < 8 || birthdayName == "NoValue")
                {
                    return "Invalid_Data";
                }
                else
                {
                    string birthday = birthdayName.Substring(birthdayName.Length - 8);
                    string memberName = birthdayName.Substring(0, birthdayName.Length - 8);
                    DateTime userBirthday = DateTime.ParseExact(birthday, "yyyyMMdd", CultureInfo.InvariantCulture);
                    var list = db.Users.Where(x => x.dateOfBirth == userBirthday && x.katakanaName == memberName).ToList();
                    if (list.Count > 1)
                    {
                        return "Member_Duplicated";
                    }
                    else
                    {
                        return "Member_Not_Duplicated";
                    }
                }
            }
        }

        public static int calculatePositionForAmountPaid(int positionId, int csvAmount)//if the amount is not enough for the applied positions..then calculate positions for the amount deposited
        {
            int positionCost = 0;
            int finalPositionCount = 0;
            var positionDetail = db.PositionDetails.Where(x => x.positionId == positionId).ToList();
            int positionCount = Convert.ToInt32(positionDetail[0].positionCount);
            int package = Convert.ToInt32(positionDetail[0].package);
            using (DataAccessLayer db = new DataAccessLayer())
            {
                //int positionCount = 0;
                //for(int count = 0; count < positionDetailsList.Count; count++)
                //{
                    //positionCount = value.positionCount;
                    
                    
                    while (positionCount != 0)
                    {
                        int pakagePrize = db.Packages.Where(x => x.packageId == package).Select(x => x.packagePrize).SingleOrDefault();
                        positionCost = positionCount * pakagePrize;
                        if(positionCost <= csvAmount)
                        {
                            finalPositionCount = finalPositionCount + positionCount;
                            positionCount = 0;
                        }
                        else
                        {
                            positionCount = positionCount - 1;
                        }
                    }
                //}
                return finalPositionCount;
            }
        }

        //Check if CSV amount is enough with the No of position Applied
        public static string compareCsvAmountWithPositionApplied(List<PositionDetail> positionDetailsList, int csvAmount)
        {
            int positionCost = calculateAmount(positionDetailsList);
            if (Convert.ToInt32(csvAmount) == positionCost)//if amount is equal approve
            {
                return "Amount_Enough";
            }
            else if (Convert.ToInt32(csvAmount) > positionCost)
            {
                return "Amount_More";
            }
            else
            {
                return "Amount_Not_Enough";
            }
        }

        public static int calculateBalance(int csvAmount, List<PositionDetail> positionDetailsList)
        {
            int positionCost = calculateAmount(positionDetailsList);
            int balance = Convert.ToInt32(csvAmount) - positionCost;
            return balance;
        }

        public static int calculateBalanceForNoOfPositions(int csvAmount, int positionId, int positionCount)
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                string packageType = db.PositionDetails.Where(x => x.positionId == positionId).Select(x => x.package).SingleOrDefault();
                int package = Convert.ToInt32(packageType);
                int pakagePrize = db.Packages.Where(x => x.packageId == package).Select(x => x.packagePrize).SingleOrDefault();
                int amount = positionCount * pakagePrize;
                int balance = csvAmount - amount;
                return balance;
            } 
        }
        public static void insertMemberBalanceTransactionTable(string memberId, int balanceAmount, string creditOrDebit)// Insert into memberBalance Table
        {
            MemberBalanceTransaction memberBalanceTransaction = new MemberBalanceTransaction();
            memberBalanceTransaction.memberId = memberId;
            memberBalanceTransaction.balanceAmount = Convert.ToInt32(balanceAmount);
            memberBalanceTransaction.transactionDate = DateTime.Now;
            memberBalanceTransaction.creditOrDebit = creditOrDebit;
            db.MemberBalanceTransactions.Add(memberBalanceTransaction);
            db.SaveChanges();
        }

        public static void insertBalanceCsvTransactionTable(int memberBalanceTransactionId, int csvDataId, int positionId, int secondCsvDataId) // Inserting the rejected positions
        {
            BalanceCsvTransaction balanceCsvTransaction = new BalanceCsvTransaction();
            balanceCsvTransaction.memberBalanceTransactionId = memberBalanceTransactionId;
            balanceCsvTransaction.csvDataId = csvDataId;
            balanceCsvTransaction.positionId = positionId;
            balanceCsvTransaction.secondCsvDataId = secondCsvDataId;
            db.BalanceCsvTransactions.Add(balanceCsvTransaction);
            db.SaveChanges();
        }

        public static void updateCsvData(int csvDataId, string status)
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                var csvDataList = db.CsvDatas.Where(d => d.csvDataId == csvDataId).ToList();
                CsvData csvData = new CsvData();
                csvData = db.CsvDatas.Find(csvDataList[0].csvDataId);
                csvData.status = status;
                db.CsvDatas.Attach(csvData);
                db.Entry(csvData).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        
        public static void updatePositionStatus(int position, string status)//UpdatePositionDetails Table Position Status
        {
            var positionDetailsList = db.PositionDetails.Where(d => d.positionId == position).ToList();
            PositionDetail positionDetail = new PositionDetail();
            positionDetail = db.PositionDetails.Find(positionDetailsList[0].positionId);
            positionDetail.positionStatus = status;
            db.PositionDetails.Attach(positionDetail);
            db.Entry(positionDetail).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void insertRejectTable(int positionId) // Inserting the rejected positions
        {
            RejectedPosition rejectedPosition = new RejectedPosition();
            rejectedPosition.positionId = positionId;
            rejectedPosition.rejectedDateTime = DateTime.Now;
            rejectedPosition.adminId = "0";
            rejectedPosition.status = "rejected";
            db.RejectedPositions.Add(rejectedPosition);
            db.SaveChanges();
        }

        public static void updateBalanceTable(string memberId, int balance)// Update memberBalance Table
        {
            using(DataAccessLayer db = new DataAccessLayer())
            {
                var memberList = db.MemberBalanceTransactions.Where(d => d.memberId == memberId).ToList();
                MemberBalanceTransaction memberBalance = new MemberBalanceTransaction();
                memberBalance = db.MemberBalanceTransactions.Find(memberList[0].memberBalanceTransactionId);
                memberBalance.balanceAmount = balance;
                db.MemberBalanceTransactions.Attach(memberBalance);
                db.Entry(memberBalance).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public static int checkIfUserExists(string birthdayName, string depositDate)// Check if user Exists in the Csv table
        {
            using(DataAccessLayer db = new DataAccessLayer())
            {
                int dateCompare = 5;
                birthdayName = birthdayName.Replace("/", "");
                var list = db.CsvDatas.Where(d => d.detailTwo == birthdayName).ToList();// comparing name and birthday with csv birthdayname
                DateTime date = Convert.ToDateTime(depositDate);
                if (list != null)
                {
                    dateCompare = DateTime.Compare(date, list[0].csvDate); // comparing deposit date with csv deposit date
                    if (dateCompare == 0)
                    {
                        return 1;
                    }
                }
                return 0;
            }
        }

        public static int calculateAmount(List<PositionDetail> positionDetails)// calculating the cost for the positions applied
        {
            using(DataAccessLayer db = new DataAccessLayer())
            {
                int amount = 0;
                foreach (var value in positionDetails)
                {
                    int package = Convert.ToInt32(value.package);
                    int pakagePrize = db.Packages.Where(x => x.packageId == package).Select(x => x.packagePrize).SingleOrDefault();
                    amount = value.positionCount * pakagePrize + amount;
                }
                return amount;
            }
        }

        public static void insertLastPostion(int positionId) // Inserting into LastPositionDetail 
        {
            LastPositionDetail lastPositionDetail = new LastPositionDetail();
            lastPositionDetail.positionId = positionId;
            lastPositionDetail.positionDate = DateTime.Now;
            db.LastPositionDetails.Add(lastPositionDetail);
            db.SaveChanges();
        }

        public static void updatePaymentStatus(int position, int positionCount)// Update paymentStatus in positionDetail Table
        {
            var positionDetailsList = db.PositionDetails.Where(d => d.positionId == position).ToList();
            PositionDetail positionDetail = new PositionDetail();
            positionDetail = db.PositionDetails.Find(positionDetailsList[0].positionId);
            positionDetail.paymentStatus = positionCount;
            db.PositionDetails.Attach(positionDetail);
            db.Entry(positionDetail).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void addCsvPositionDetail (int csvDataId, int positionId)
        {
            using(DataAccessLayer db = new DataAccessLayer())
            {
                CsvPositionDetail csvPositionDetail = new CsvPositionDetail();
                csvPositionDetail.csvDataId = csvDataId;
                csvPositionDetail.positionId = positionId;
                db.CsvPositionDetails.Add(csvPositionDetail);
                db.SaveChanges();
            }
        }
    }
}