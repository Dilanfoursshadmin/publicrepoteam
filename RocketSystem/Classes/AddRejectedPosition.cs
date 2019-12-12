using RocketSystem.DbLink;
using RocketSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RocketSystem.Classes
{
    public class AddRejectedPosition
    {
        public static List<int> AddPosition(int positionId)
        {
            List<int> arrayList = new List<int>();
            using (DataAccessLayer db = new DataAccessLayer())
            {
                var rejectedPosition = db.PositionDetails.Where(x => x.positionId == positionId).FirstOrDefault();
                int balance = getBalance(rejectedPosition.membershipNo);
                int positionCost = calculatePositionCost(rejectedPosition);
                int positionCostBalance = balance - positionCost;
                if(positionCostBalance == 0)//success
                {
                    updateRejectedPositionStatus(rejectedPosition.positionId);
                    insertMemberBalanceTransaction(rejectedPosition.membershipNo, balance);
                    updatePositionStatus(rejectedPosition.positionId);
                    insertBalanceCsvTransaction(rejectedPosition.membershipNo, rejectedPosition.positionId);
                    arrayList.Add(rejectedPosition.positionId);
                }
                else if(positionCostBalance > 0) //success balance remains
                {
                    updateRejectedPositionStatus(rejectedPosition.positionId);
                    insertMemberBalanceTransaction(rejectedPosition.membershipNo, balance);
                    updatePositionStatus(rejectedPosition.positionId);
                    insertBalanceCsvTransaction(rejectedPosition.membershipNo, rejectedPosition.positionId);
                    insertMemberBalanceAfterAddingPosition(rejectedPosition.membershipNo, positionCostBalance);
                    insertBalanceCsvAfterAddingPosition(rejectedPosition.membershipNo, rejectedPosition.positionId);
                    arrayList.Add(rejectedPosition.positionId);
                }
                else//reject
                {
                    arrayList = null;
                }
                return arrayList;
            }
        }
        
        public static int getBalance(string memberId)//calculate the balance for the rejected position
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                var totalCreditAmount = db.MemberBalanceTransactions.Where(x => x.memberId == memberId && x.creditOrDebit == "credit").OrderByDescending(x => x.memberBalanceTransactionId).Take(2).GroupBy(x => new { x.memberId, x.creditOrDebit }).Select(x => new { totalCreditAmountId = x.Key, balanceAmount = x.Sum(y => y.balanceAmount), memberBalanceTransactionId =  x.Select(y => y.memberBalanceTransactionId) }).FirstOrDefault();
                var totalDebitAmount = db.MemberBalanceTransactions.Where(x => x.memberId == memberId && x.creditOrDebit == "debit").OrderByDescending(x => x.memberBalanceTransactionId).Take(2).GroupBy(x => new { x.memberId, x.creditOrDebit }).Select(x => new { totalCreditAmountId = x.Key, balanceAmount = x.Sum(y => y.balanceAmount) }).FirstOrDefault(); ;
                int totalCredit = 0;
                int totalDebit = 0;
                if (totalCreditAmount != null)
                {
                    totalCredit = totalCreditAmount.balanceAmount;
                }
                if (totalDebitAmount != null)
                {
                    totalDebit = totalDebitAmount.balanceAmount;
                }
                int balanceAmount = totalCredit - totalDebit;
                return balanceAmount;
            }
        }

        public static int calculatePositionCost(PositionDetail rejectedPosition)//calculating the cost for the positions applied
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                int package = Convert.ToInt32(rejectedPosition.package);
                int pakagePrize = db.Packages.Where(x => x.packageId == package).Select(x => x.packagePrize).SingleOrDefault();
                int amount = rejectedPosition.positionCount * pakagePrize;
                return amount;
            }
        }

        public static void updateRejectedPositionStatus(int position)// Update Rejected Postion Status in the Rejected Table
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                var rejectPositionList = db.RejectedPositions.Where(d => d.positionId == position).ToList();
                RejectedPosition rejectedPosition = new RejectedPosition();
                rejectedPosition = db.RejectedPositions.Find(rejectPositionList[0].rejectedPositionId);
                rejectedPosition.status = "Accepted";
                db.RejectedPositions.Attach(rejectedPosition);
                db.Entry(rejectedPosition).State = EntityState.Modified;
                db.SaveChanges();
            }    
        }

        public static void insertMemberBalanceTransaction(string memberId, int amount)// Insert the transaction cost for the rejected position
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                MemberBalanceTransaction memberBalanceTransaction = new MemberBalanceTransaction();
                memberBalanceTransaction.memberId = memberId;
                memberBalanceTransaction.balanceAmount = Convert.ToInt32(amount);
                memberBalanceTransaction.transactionDate = DateTime.Now;
                memberBalanceTransaction.creditOrDebit = "debit";
                db.MemberBalanceTransactions.Add(memberBalanceTransaction);
                db.SaveChanges();
            }
        }

        public static void insertBalanceCsvTransaction(string memberId, int positionId)// Insert the balancecsv transaction for the rejected position
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                var balanceCsvList = db.MemberBalanceTransactions.Where(x => x.memberId == memberId && x.creditOrDebit == "credit").OrderByDescending(x => x.memberBalanceTransactionId).Take(2).GroupBy(x => new { x.memberId, x.creditOrDebit }).Select(x => new { totalCreditAmountId = x.Key, balanceAmount = x.Sum(y => y.balanceAmount), memberBalanceTransactionId = x.Select(y => y.memberBalanceTransactionId) }).FirstOrDefault();
                var firstMemberBalanceCsvTransaction = balanceCsvList.memberBalanceTransactionId.ElementAt(0);
                var secondMemberBalanceCsvTransaction = balanceCsvList.memberBalanceTransactionId.ElementAt(1);
                var csvDataId = db.BalanceCsvTransactions.Where(x => x.memberBalanceTransactionId == firstMemberBalanceCsvTransaction).Select(x => x.csvDataId).SingleOrDefault();
                var secondCsvDataId = db.BalanceCsvTransactions.Where(x => x.memberBalanceTransactionId == secondMemberBalanceCsvTransaction).Select(x => x.csvDataId).SingleOrDefault();
                int memberBalanceTransactionId = db.MemberBalanceTransactions.OrderByDescending(p => p.memberBalanceTransactionId).FirstOrDefault().memberBalanceTransactionId;
                BalanceCsvTransaction balanceCsvTransaction = new BalanceCsvTransaction();
                balanceCsvTransaction.memberBalanceTransactionId = memberBalanceTransactionId;
                balanceCsvTransaction.csvDataId = csvDataId;
                balanceCsvTransaction.positionId = positionId;
                balanceCsvTransaction.secondCsvDataId = secondCsvDataId;
                db.BalanceCsvTransactions.Add(balanceCsvTransaction);
                db.SaveChanges();
            }
        }

        public static void insertMemberBalanceAfterAddingPosition(string memberId, int amount)// Inserting the balance if remains after adding the rejected position
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                MemberBalanceTransaction memberBalanceTransaction = new MemberBalanceTransaction();
                memberBalanceTransaction.memberId = memberId;
                memberBalanceTransaction.balanceAmount = Convert.ToInt32(amount);
                memberBalanceTransaction.transactionDate = DateTime.Now;
                memberBalanceTransaction.creditOrDebit = "credit";
                db.MemberBalanceTransactions.Add(memberBalanceTransaction);
                db.SaveChanges();
            }
        }

        public static void insertBalanceCsvAfterAddingPosition(string memberId, int positionId) // Inserting  balancecsv transaction if balance remains after adding the rejected position
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                int memberBalanceTransactionId = db.BalanceCsvTransactions.OrderByDescending(p => p.memberBalanceTransactionId).FirstOrDefault().memberBalanceTransactionId;
                var csvDataId = db.BalanceCsvTransactions.Where(x => x.memberBalanceTransactionId == memberBalanceTransactionId).OrderByDescending(x => x.memberBalanceTransactionId).Select(x => x.secondCsvDataId).SingleOrDefault();
                BalanceCsvTransaction balanceCsvTransaction = new BalanceCsvTransaction();
                balanceCsvTransaction.memberBalanceTransactionId = memberBalanceTransactionId;
                balanceCsvTransaction.csvDataId = csvDataId;
                balanceCsvTransaction.positionId = positionId;
                balanceCsvTransaction.secondCsvDataId = 0;
                db.BalanceCsvTransactions.Add(balanceCsvTransaction);
                db.SaveChanges();
            }
        }

        public static void updatePositionStatus(int position)//UpdatePositionDetails Table Position Status
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                var positionDetailsList = db.PositionDetails.Where(d => d.positionId == position).ToList();
                PositionDetail positionDetail = new PositionDetail();
                positionDetail = db.PositionDetails.Find(positionDetailsList[0].positionId);
                positionDetail.positionStatus = "Accepted";
                db.PositionDetails.Attach(positionDetail);
                db.Entry(positionDetail).State = EntityState.Modified;
                db.SaveChanges();
            }  
        }
    }
}