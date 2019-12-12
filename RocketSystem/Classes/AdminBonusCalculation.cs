using RocketSystem.DbLink;
using RocketSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Classes
{
    public class AdminBonusCalculation
    {
        private static DataAccessLayer db = new DataAccessLayer();

        public dynamic introducepositionlist()
        {
            List<StageOne> introducer = new List<StageOne>();

            var introducers = db.StageOnes.GroupBy(d => d.introducePromoCode).OrderByDescending(a => a.Count()).ToList();
            int k = 0;
            foreach (var a in introducers)
            {

                var membership = db.StageOnes.Where(d => d.positionCode == a.Key).FirstOrDefault();
                var username = db.Users.Where(d => d.membershipNo == membership.membershipNo).FirstOrDefault();

                if (username != null)
                {
                    var introducer2 = new StageOne();
                    introducer2.membershipNo = username.name;
                    introducer2.bcNo = a.Count();
                    var introducer4 = new StageOne();
                    introducer4 = introducer2;
                    introducer.Add(introducer4);
                }
                if (k >= 5)
                {
                    break;
                }
                k++;
            }
            return introducer;
        }
        public dynamic toppackageintroducepositionlist()
        {
            List<StageOne> introducer = new List<StageOne>();

            var introducers = db.StageOnes.Where(d => d.package == 2).GroupBy(d => d.introducePromoCode).OrderByDescending(a => a.Count()).ToList();
            int k = 0;
            foreach (var a in introducers)
            {

                var membership = db.StageOnes.Where(d => d.positionCode == a.Key).FirstOrDefault();
                var username = db.Users.Where(d => d.membershipNo == membership.membershipNo).FirstOrDefault();

                if (username != null)
                {
                    var introducer2 = new StageOne();
                    introducer2.membershipNo = username.name;
                    introducer2.bcNo = a.Count();
                    var introducer4 = new StageOne();
                    introducer4 = introducer2;
                    introducer.Add(introducer4);
                }
                if (k >= 5)
                {
                    break;
                }
                k++;
            }
            return introducer;
        }

        public dynamic middlepackageintroducepositionlist()
        {
            List<StageOne> introducer = new List<StageOne>();

            var introducers = db.StageOnes.Where(d => d.package == 1).GroupBy(d => d.introducePromoCode).OrderByDescending(a => a.Count()).ToList();
            int k = 0;
            foreach (var a in introducers)
            {

                var membership = db.StageOnes.Where(d => d.positionCode == a.Key).FirstOrDefault();
                var username = db.Users.Where(d => d.membershipNo == membership.membershipNo).FirstOrDefault();

                if (username != null)
                {
                    var introducer2 = new StageOne();
                    introducer2.membershipNo = username.name;
                    introducer2.bcNo = a.Count();
                    var introducer4 = new StageOne();
                    introducer4 = introducer2;
                    introducer.Add(introducer4);
                }
                if (k >= 5)
                {
                    break;
                }
                k++;
            }
            return introducer;
        }

        public dynamic monthmemberwisebonus()
        {
            var firstdate = db.PaidBonuss.FirstOrDefault();
            var lastdate = db.PaidBonuss.OrderByDescending(d => d.paidBonusId).FirstOrDefault();

            int yearstart = firstdate.paidBonusDateTime.Year;
            int yearend = lastdate.paidBonusDateTime.Year;

            var listmem = db.PaidBonuss.GroupBy(d => d.memberId).ToList();

            foreach (var listmembe in listmem)
            {
                for (int i = yearstart; i <= yearend; i++)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        DateTime starrtdatecalculation = new DateTime(i, j, 1);
                        DateTime enddatecalculation = new DateTime(i, 1, 1);
                        if (j == 12)
                        {
                            enddatecalculation = new DateTime(i + 1, 1, 1);
                        }
                        else
                        {
                            enddatecalculation = new DateTime(i, j + 1, 1);
                        }

                        double countbonus = db.PaidBonuss.Where(d => d.memberId == listmembe.Key && d.paidBonusDateTime >= starrtdatecalculation && d.paidBonusDateTime < enddatecalculation).Count();
                        if (countbonus != 0)
                        {
                            double allbonus = db.PaidBonuss.Where(d => d.memberId == listmembe.Key && d.paidBonusDateTime >= starrtdatecalculation && d.paidBonusDateTime < enddatecalculation).Sum(d => d.paidIntoduceBonus + d.paidpositionBonus + d.paidPresentageBonus + d.paidThirdStageBonus + d.paidFifthStageBonus);
                        }
                        int allprice = db.StageOnes.Where(d => d.membershipNo == listmembe.Key && d.package == 1).Count() * 100000 + db.StageOnes.Where(d => d.membershipNo == listmembe.Key && d.package == 2).Count() * 350000;

                    }
                }
            }

            return null;
        }
        public dynamic coustomerwiseprofit()
        {
            List<PaidBonus> companyscustomer = new List<PaidBonus>();
            var mans = db.PaidBonuss.GroupBy(d => d.memberId).ToList();

            foreach (var compnyscus in mans)
            {
                var customer1 = new PaidBonus();
                string mansin = compnyscus.Key;
                double man01 = db.PaidBonuss.Where(d => d.memberId == mansin).Sum(d => d.paidIntoduceBonus + d.paidpositionBonus + d.paidPresentageBonus + d.paidThirdStageBonus + d.paidFifthStageBonus);
                customer1.memberId = mansin;
                customer1.paidIntoduceBonus = man01;
                double man02 = db.StageOnes.Where(d => d.membershipNo == mansin && d.package == 1).Count() * 100000;
                double man03 = db.StageOnes.Where(d => d.membershipNo == mansin && d.package == 2).Count() * 350000;
                double man04 = db.StageOnes.Where(d => d.membershipNo == mansin && d.package == 3).Count() * 10000;
                double manstotl = man02 + man03 + man04;
                customer1.paidpositionBonus = manstotl;
                customer1.paidPresentageBonus = manstotl - man01;
                customer1.paidThirdStageBonus = ((manstotl - man01) / manstotl) * 100;



                var temporyadd = new PaidBonus();
                temporyadd = customer1;

                companyscustomer.Add(temporyadd);

            }
            return companyscustomer;

        }

        public dynamic paidpresentagebonus()
        {
            var paidbonus = db.PaidBonuss.ToList();
            List<int> paidbonusview = new List<int>();

            double presentagebonus = paidbonus.Sum(d => d.paidPresentageBonus);
            if (presentagebonus == 0)
                presentagebonus = 1;
            double introducebonus = paidbonus.Sum(d => d.paidIntoduceBonus);
            double positionbonus = paidbonus.Sum(d => d.paidpositionBonus);
            double paidthirdstagebonus = paidbonus.Sum(d => d.paidThirdStageBonus);
            double paidfifthstagebonus = paidbonus.Sum(d => d.paidFifthStageBonus);
            double paidsharebonus = paidbonus.Sum(d => d.paidshareBonus);

            double all = presentagebonus + introducebonus + positionbonus + paidthirdstagebonus + paidfifthstagebonus + paidsharebonus;
            presentagebonus = (presentagebonus / all) * 100;
            introducebonus = (introducebonus / all) * 100;
            positionbonus = (positionbonus / all) * 100;
            paidthirdstagebonus = (paidthirdstagebonus / all) * 100;
            paidfifthstagebonus = (paidfifthstagebonus / all) * 100;
            paidsharebonus = (paidsharebonus / all) * 100;

            int presentagebonus1 = Convert.ToInt32(presentagebonus);
            int positionbonus1 = Convert.ToInt32(positionbonus);
            int paidfifthstagebonus1 = Convert.ToInt32(paidfifthstagebonus);
            int paidthirdstagebonus1 = Convert.ToInt32(paidthirdstagebonus);
            int introducebonus1 = Convert.ToInt32(introducebonus);
            int paidsharebonus1 = Convert.ToInt32(paidsharebonus);

            paidbonusview.Add(presentagebonus1);
            paidbonusview.Add(positionbonus1);
            paidbonusview.Add(paidfifthstagebonus1);
            paidbonusview.Add(paidthirdstagebonus1);
            paidbonusview.Add(introducebonus1);
            paidbonusview.Add(paidsharebonus1);

            return paidbonusview;

        }
        public dynamic paidBonus()
        {
            List<int> paidbonusview = new List<int>();
            using (DataAccessLayer db = new DataAccessLayer())
            {
                var paidbonus = db.PaidBonuss.ToList();

                double presentagebonus = paidbonus.Sum(d => d.paidPresentageBonus);
                double introducebonus = paidbonus.Sum(d => d.paidIntoduceBonus);
                double positionbonus = paidbonus.Sum(d => d.paidpositionBonus);
                double paidthirdstagebonus = paidbonus.Sum(d => d.paidThirdStageBonus);
                double paidfifthstagebonus = paidbonus.Sum(d => d.paidFifthStageBonus);
                double sharebonus = paidbonus.Sum(d => d.paidshareBonus);

                int presentagebonus1 = Convert.ToInt32(presentagebonus);
                int positionbonus1 = Convert.ToInt32(positionbonus);
                int paidfifthstagebonus1 = Convert.ToInt32(paidfifthstagebonus);
                int paidthirdstagebonus1 = Convert.ToInt32(paidthirdstagebonus);
                int introducebonus1 = Convert.ToInt32(introducebonus);
                int sharebonus1 = Convert.ToInt32(sharebonus);

                paidbonusview.Add(presentagebonus1);
                paidbonusview.Add(positionbonus1);
                paidbonusview.Add(paidfifthstagebonus1);
                paidbonusview.Add(paidthirdstagebonus1);
                paidbonusview.Add(introducebonus1);
                paidbonusview.Add(sharebonus1);
            }

            return paidbonusview;

        }

        public dynamic postionDetails()
        {
            var paidbonus = db.PaidBonuss.ToList();
            List<int> positiondetails = new List<int>();
            int middleuser = db.StageOnes.Where(d => d.jumpHistory == "1" && d.package == 1).Count();
            int topuser = db.StageTwoes.Where(d => d.jumpHistory == "2" && d.package == 2).Count();
            int smalluser = db.StageOnes.Where(d => d.freeStatus == 5 || d.freeStatus == 3).Count();
            int totaluser = middleuser + topuser + smalluser;

            positiondetails.Add(topuser);
            positiondetails.Add(middleuser);
            positiondetails.Add(smalluser);
            positiondetails.Add(totaluser);

            return positiondetails;

        }

        public dynamic profitpastmonth()
        {
            var profits = db.PaidBonuss.ToList();
            List<int> profitdetails = new List<int>();

            double paidintroducebonus10 = 0.0;
            double paidpostition10 = 0.0;
            double paidpresentage10 = 0.0;
            double paidthirdsection10 = 0.0;
            double paidfifthsection10 = 0.0;
            double paidsharebonus = 0.0;
            double totalmonthallbonus;

            int years = DateTime.Now.Year;
            DateTime dt = new DateTime(years, 01, 1);
            for (int i = 1; i <= 12; i++)
            {
                DateTime firstmonthdate = dt.AddMonths(i - 1);
                DateTime firstOfNextMonth = dt.AddMonths(i);
                var check = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Count();
                if (check == 0)
                {
                    paidintroducebonus10 = 0.0;
                }
                else
                {
                    paidintroducebonus10 = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Sum(d => d.paidIntoduceBonus);
                }
                var check1 = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Count();
                if (check1 == 0)
                {
                    paidpostition10 = 0.0;
                }
                else
                {
                    paidpostition10 = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Sum(d => d.paidpositionBonus);
                }
                var check2 = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Count();
                if (check2 == 0)
                {
                    paidpresentage10 = 0.0;
                }
                else
                {
                    paidpresentage10 = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Sum(d => d.paidPresentageBonus);
                }
                var check3 = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Count();
                if (check3 == 0)
                {
                    paidthirdsection10 = 0.0;
                }
                else
                {
                    paidthirdsection10 = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Sum(d => d.paidThirdStageBonus);
                }
                var check4 = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Count();
                if (check4 == 0)
                {
                    paidfifthsection10 = 0.0;
                }
                else
                {
                    paidfifthsection10 = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Sum(d => d.paidFifthStageBonus);
                }
                var check5 = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Count();
                if (check5 == 0)
                {
                    paidsharebonus = 0.0;
                }
                else
                {
                    paidsharebonus = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstmonthdate && d.paidBonusDateTime < firstOfNextMonth).Sum(d => d.paidshareBonus);
                }
                totalmonthallbonus = paidintroducebonus10 + paidpostition10 + paidpresentage10 + paidthirdsection10 + paidfifthsection10 + paidsharebonus;
                profitdetails.Add(Convert.ToInt32(totalmonthallbonus));

                int counting01 = db.StageOnes.Where(d => d.package == 1 && d.entryDate >= firstmonthdate && d.entryDate < firstOfNextMonth).Count();
                int counting02 = db.StageOnes.Where(d => d.package == 2 && d.entryDate >= firstmonthdate && d.entryDate < firstOfNextMonth).Count();
                int counting03 = db.StageOnes.Where(d => d.package == 3 && d.entryDate >= firstmonthdate && d.entryDate < firstOfNextMonth).Count();
                int firstprof = counting01 * 100000 + counting02 * 350000 + 10000 * counting03;
                profitdetails.Add(firstprof);
                int profit = firstprof - Convert.ToInt32(totalmonthallbonus);
                profitdetails.Add(profit);
            }
            return profitdetails;

        }
        public dynamic datewiseposition()
        {
            int middleuser = 0;
            int topuser = 0;
            int smalluser = 0;
            List<int> datewiseposition = new List<int>();
            DateTime nowdate = DateTime.Now.Date;
            string day = DateTime.Now.DayOfWeek.ToString();
            int y = 0;
            if (day == "Monday")
            {
                y = -1;
            }
            if (day == "Tuesday")
            {
                y = -2;
            }
            if (day == "Wednesday")
            {
                y = -3;
            }
            if (day == "Thursday")
            {
                y = -4;
            }
            if (day == "Friday")
            {
                y = -5;
            }
            if (day == "Saturday")
            {
                y = -6;
            }
            if (day == "Sunday")
            {
                y = 0;
            }

            for (int i = y; i <= y + 7; i++)
            {
                DateTime expdate = DateTime.Now.Date.AddDays(i);
                DateTime expdate1 = DateTime.Now.Date.AddDays(i + 1);

                middleuser = db.StageOnes.Where(d => d.jumpHistory == "1" && d.package == 1 && d.entryDate >= expdate && d.entryDate < expdate1).Count();

                topuser = db.StageOnes.Where(d => d.jumpHistory == "1" && d.package == 2 && d.entryDate >= expdate && d.entryDate < expdate1).Count();

                smalluser = db.StageOnes.Where(d => d.jumpHistory == "1" && d.package == 3 && d.entryDate >= expdate && d.entryDate < expdate1).Count();

                datewiseposition.Add(middleuser);
                datewiseposition.Add(topuser);
                datewiseposition.Add(smalluser);
                middleuser = 0;
                topuser = 0;
                smalluser = 0;
            }
            datewiseposition.Add(y);
            return datewiseposition;
        }

        public dynamic topmemberbonus()
        {
            List<PaidBonus> topmembers = new List<PaidBonus>();
            int x = 0;

            var memberbonus = db.PaidBonuss.GroupBy(d => d.memberId).ToList();

            foreach (var membersbonustop in memberbonus)
            {

                string key = membersbonustop.Key;
                PaidBonus introducersbon = new PaidBonus();
                introducersbon.paidIntoduceBonus = db.PaidBonuss.Where(d => d.memberId == key).Sum(d => d.paidIntoduceBonus) +
                                                    db.PaidBonuss.Where(d => d.memberId == key).Sum(d => d.paidPresentageBonus) +
                                                    db.PaidBonuss.Where(d => d.memberId == key).Sum(d => d.paidpositionBonus) +
                                                    db.PaidBonuss.Where(d => d.memberId == key).Sum(d => d.paidThirdStageBonus) +
                                                    db.PaidBonuss.Where(d => d.memberId == key).Sum(d => d.paidFifthStageBonus);

                introducersbon.paidPresentageBonus = Convert.ToInt32(key);
                var userdetails = db.Users.Where(d => d.membershipNo == key).FirstOrDefault();
                if (userdetails == null)
                {
                    introducersbon.memberId = "unknown";
                }
                else
                {
                    introducersbon.memberId = userdetails.name;
                }
                if (x == 0)
                {
                    PaidBonus introducebon1 = new PaidBonus();
                    introducebon1 = introducersbon;
                    topmembers.Add(introducebon1);
                }
                if (x >= 1)
                {
                    PaidBonus introducebon2 = new PaidBonus();
                    introducebon2 = introducersbon;
                    topmembers.Add(introducebon2);
                }

                x = x + 1;
            }

            var newList = topmembers.OrderByDescending(nx => nx.paidIntoduceBonus).ToList();
            return newList;
        }
        public dynamic onlytopmemberbonus()
        {
            List<PaidBonus> topmembers = new List<PaidBonus>();

            int x = 0;
            var toppositon1 = db.StageThrees.Where(d => d.package == 2 && d.jumpHistory == "3").GroupBy(d => d.membershipNo).OrderByDescending(g => g.Count()).ToList();
            int counting = toppositon1.Count();
            if (counting > 6)
            {
                counting = 6;
            }
            for (int i = 0; i < counting; i++)
            {
                PaidBonus introducerbon = new PaidBonus();
                string names = toppositon1[i].Key;
                if (db.Users.Where(d => d.membershipNo == names).Count() != 0)
                {
                    var information = db.Users.Where(d => d.membershipNo == names).FirstOrDefault();
                    introducerbon.memberId = information.name;
                    introducerbon.paidIntoduceBonus = toppositon1[i].Count();

                    PaidBonus introducebon1 = new PaidBonus();
                    introducebon1 = introducerbon;
                    topmembers.Add(introducebon1);

                }
            }

            return topmembers;
        }

        public dynamic onlymiddlememberbonus()
        {
            List<PaidBonus> topmembers = new List<PaidBonus>();

            int x = 0;
            var toppositon1 = db.StageOnes.Where(d => d.package == 1 && d.jumpHistory == "1" && d.freeStatus == 0).GroupBy(d => d.membershipNo).OrderByDescending(g => g.Count()).ToList();
            int counting = toppositon1.Count();
            if (counting > 6)
            {
                counting = 6;
            }
            for (int i = 0; i < counting; i++)
            {
                PaidBonus introducerbon = new PaidBonus();
                string names = toppositon1[i].Key;
                if (db.Users.Where(d => d.membershipNo == names).Count() != 0)
                {
                    var information = db.Users.Where(d => d.membershipNo == names).FirstOrDefault();
                    introducerbon.memberId = information.name;
                    introducerbon.paidIntoduceBonus = toppositon1[i].Count();

                    PaidBonus introducebon1 = new PaidBonus();
                    introducebon1 = introducerbon;
                    topmembers.Add(introducebon1);
                }
            }

            return topmembers;
        }

        public dynamic onlysmallmemberbonus()
        {
            List<PaidBonus> topmembers = new List<PaidBonus>();

            int x = 0;
            var toppositon1 = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 5).GroupBy(d => d.membershipNo).OrderByDescending(g => g.Count()).ToList();
            int counting = toppositon1.Count();
            if (counting > 6)
            {
                counting = 6;
            }
            for (int i = 0; i < counting; i++)
            {
                PaidBonus introducerbon = new PaidBonus();
                string names = toppositon1[i].Key;
                if (db.Users.Where(d => d.membershipNo == names).Count() != 0)
                {
                    var information = db.Users.Where(d => d.membershipNo == names).FirstOrDefault();
                    introducerbon.memberId = information.name;
                    introducerbon.paidIntoduceBonus = toppositon1[i].Count();

                    PaidBonus introducebon1 = new PaidBonus();
                    introducebon1 = introducerbon;
                    topmembers.Add(introducebon1);
                }
            }

            return topmembers;
        }

        public dynamic rejectedPosition()
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                List<StageOne> listofreject = new List<StageOne>();
                int i = 0;
                int creditmoney;
                int debitmoney;
                var rejectlist = db.RejectedPositions.Where(d => d.status == "pending").Join(db.PositionDetails, p => p.positionId, o => o.positionId, (p, o) => new { membernumber = o.membershipNo, o.positionCount, o.depositDate, o.introducePromoCode, o.positionId, o.package, p.rejectedPositionId }).Join(db.StageOnes, q => q.introducePromoCode, n => n.positionCode, (q, n) => new { q.rejectedPositionId, q.package, introducemembersnumber = n.membershipNo, q.positionCount, q.depositDate, q.positionId, q.membernumber }).Join(db.Users, y => y.membernumber, x => x.membershipNo, (x, y) => new { x.package, x.rejectedPositionId, x.introducemembersnumber, x.membernumber, x.positionCount, x.positionId, x.depositDate, y.name, y.accountName }).ToList();
                var rejectedlistnee = rejectlist.Select(d => new StageOne { membershipNo = d.membernumber, introducePromoCode = d.introducemembersnumber, entryDate = d.depositDate, treeLevel = d.positionCount, jumpHistory = d.name, jump = d.rejectedPositionId, bcNo = d.positionId }).GroupBy(d => d.jump).ToList();
                foreach (var a in rejectedlistnee)
                {

                    foreach (var b in a)
                    {
                        creditmoney = 0;
                        debitmoney = 0;
                        listofreject.Add(b);
                        int creditmoneycount = db.MemberBalanceTransactions.Where(d => d.memberId == b.membershipNo && d.creditOrDebit == "credit").Count();
                        int debitmoneycount = db.MemberBalanceTransactions.Where(d => d.memberId == b.membershipNo && d.creditOrDebit == "debit").Count();
                        if (creditmoneycount != 0)
                        {
                            creditmoney = db.MemberBalanceTransactions.Where(d => d.memberId == b.membershipNo && d.creditOrDebit == "credit").Sum(d => d.balanceAmount);
                        }
                        if (debitmoneycount != 0)
                        {
                            debitmoney = db.MemberBalanceTransactions.Where(d => d.memberId == b.membershipNo && d.creditOrDebit == "debit").Sum(d => d.balanceAmount);
                        }
                        listofreject[i].treeColumn = creditmoney - debitmoney;
                        break;
                    }
                    i++;
                }

                return listofreject;
            }
            return null;
        }

        public dynamic yearlyBonus()
        {
            List<int> paidbonusyearly = new List<int>();
            for (int i = 0; i >= -3; i--)
            {
                int year = DateTime.Now.Year;
                var year1 = DateTime.Now.AddYears(i).Year;
                DateTime firstDay = new DateTime(year1, 1, 1);
                DateTime lastDay = new DateTime(year1, 12, 31);

                int condtion = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstDay && d.paidBonusDateTime <= lastDay).Count();
                int income = db.StageOnes.Where(d => d.package == 2 && d.entryDate >= firstDay && d.entryDate <= lastDay).Count() * 350000 + db.StageOnes.Where(d => d.package == 1 && d.entryDate >= firstDay && d.entryDate <= lastDay).Count() * 100000 + db.StageOnes.Where(d => d.package == 3 && d.entryDate >= firstDay && d.entryDate <= lastDay).Count() * 10000;
                if (condtion != 0)
                {
                    double paidyearlybonus = db.PaidBonuss.Where(d => d.paidBonusDateTime >= firstDay && d.paidBonusDateTime <= lastDay).Sum(d => d.paidIntoduceBonus + d.paidFifthStageBonus + d.paidpositionBonus + d.paidThirdStageBonus + d.paidPresentageBonus + d.paidshareBonus);
                    paidbonusyearly.Add(Convert.ToInt32(paidyearlybonus));
                }
                else
                {
                    paidbonusyearly.Add(0);
                }
                paidbonusyearly.Add(income);
            }

            return paidbonusyearly;
        }

        public dynamic expecterIncome()
        {
            List<int> expectedincomes = new List<int>();
            int year = DateTime.Now.Year;
            int expecteincomevalue1;
            int expecteincomevalue2;
            int expecteincomevalue3;
            var month = new PaidBonus();
            for (int i = 1; i <= 12; i++)
            {

                int allexpected = 0;
                expecteincomevalue1 = 0;
                expecteincomevalue2 = 0;
                expecteincomevalue3 = 0;
                int k = i + 1;
                if (k == 13)
                {
                    k = 1;
                }
                DateTime firstDay = new DateTime(year, i, 1);
                DateTime lastDay = new DateTime(year, k, 1);
                int expecteincome1 = db.PositionDetails.Where(d => d.package == "2" && d.registerDate >= firstDay && d.registerDate < lastDay).Count();
                if (expecteincome1 != 0)
                {
                    expecteincomevalue1 = db.PositionDetails.Where(d => d.package == "2" && d.registerDate >= firstDay && d.registerDate < lastDay).Sum(d => d.positionCount) * 350000;
                }
                int expecteincome2 = db.PositionDetails.Where(d => d.package == "1" && d.registerDate >= firstDay && d.registerDate < lastDay).Count();
                if (expecteincome2 != 0)
                {
                    expecteincomevalue2 = db.PositionDetails.Where(d => d.package == "1" && d.registerDate >= firstDay && d.registerDate < lastDay).Sum(d => d.positionCount) * 100000;
                }
                int expecteincome3 = db.PositionDetails.Where(d => d.package == "3" && d.registerDate >= firstDay && d.registerDate < lastDay).Count();
                if (expecteincome3 != 0)
                {
                    expecteincomevalue3 = db.PositionDetails.Where(d => d.package == "3" && d.registerDate >= firstDay && d.registerDate < lastDay).Sum(d => d.positionCount) * 10000;
                }
                allexpected = expecteincomevalue1 + expecteincomevalue2 + expecteincomevalue3;
                expectedincomes.Add(allexpected);
            }

            for (int i = 1; i <= 12; i++)
            {
                int allexpected = 0;
                expecteincomevalue1 = 0;
                expecteincomevalue2 = 0;
                expecteincomevalue3 = 0;
                int k = i + 1;
                if (k == 13)
                {
                    k = 1;
                }
                DateTime firstDay = new DateTime(year, i, 1);
                DateTime lastDay = new DateTime(year, k, 1);
                int expecteincome1 = db.StageOnes.Where(d => d.package == 2 && d.entryDate >= firstDay && d.entryDate < lastDay).Count();
                if (expecteincome1 != 0)
                {
                    expecteincomevalue1 = db.StageOnes.Where(d => d.package == 2 && d.entryDate >= firstDay && d.entryDate < lastDay).Count() * 350000;
                }
                int expecteincome2 = db.StageOnes.Where(d => d.package == 1 && d.entryDate >= firstDay && d.entryDate < lastDay).Count();
                if (expecteincome2 != 0)
                {
                    expecteincomevalue2 = db.StageOnes.Where(d => d.package == 1 && d.entryDate >= firstDay && d.entryDate < lastDay).Count() * 100000;
                }
                int expecteincome3 = db.StageOnes.Where(d => d.package == 3 && d.entryDate >= firstDay && d.entryDate < lastDay).Count();
                if (expecteincome3 != 0)
                {
                    expecteincomevalue3 = db.StageOnes.Where(d => d.package == 3 && d.entryDate >= firstDay && d.entryDate < lastDay).Count() * 10000;
                }
                allexpected = expecteincomevalue1 + expecteincomevalue2 + expecteincomevalue3;
                expectedincomes.Add(allexpected);
            }
            return expectedincomes;
        }
        public dynamic freepositionode()
        {
            List<int> freeposition = new List<int>();
            int year = DateTime.Now.Year;
            for (int i = 1; i <= 12; i++)
            {
                DateTime datefirst = new DateTime(year, i, 1);
                DateTime lastdate = new DateTime(year, 1, 1);
                if (i == 12)
                {
                    lastdate = new DateTime(year + 1, 1, 1);
                }
                else
                {
                    lastdate = new DateTime(year, i + 1, 1);
                }
                int freepos = db.StageOnes.Where(d => d.freeStatus == 3 && d.entryDate >= datefirst && d.entryDate < lastdate || d.freeStatus == 5 && d.entryDate >= datefirst && d.entryDate < lastdate).Count();
                int moneypos = db.StageOnes.Where(d => d.freeStatus == 0 && d.entryDate >= datefirst && d.entryDate < lastdate).Count();

                freeposition.Add(freepos);
                freeposition.Add(moneypos);
            }
            return freeposition;
        }
        public dynamic freepositionbonus()
        {
            List<int> freepositionbonus = new List<int>();
            int havecolumn = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 5).Join(db.PaidBonuss.Where(d => d.positionHistory == 1), o => new { member = o.membershipNo, bcnum = o.bcNo }, m => new { member = m.memberId, bcnum = m.bcNumber }, (o, m) => new { o.treeId, o.membershipNo, o.bcNo, m.memberId, m.paidBonusDateTime, m.paidBonusId, m.paidFifthStageBonus, m.paidThirdStageBonus, m.paidpositionBonus, m.paidPresentageBonus, m.paidshareBonus, m.paidIntoduceBonus, m.positionHistory }).ToList().Count();
            if (havecolumn != 0)
            {
                var next = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 5).Join(db.PaidBonuss.Where(d => d.positionHistory == 1), o => new { member = o.membershipNo, bcnum = o.bcNo }, m => new { member = m.memberId, bcnum = m.bcNumber }, (o, m) => new { o.treeId, o.membershipNo, o.bcNo, m.memberId, m.paidBonusDateTime, m.paidBonusId, m.paidFifthStageBonus, m.paidThirdStageBonus, m.paidpositionBonus, m.paidPresentageBonus, m.paidshareBonus, m.paidIntoduceBonus, m.positionHistory }).ToList();

                double total = next.Sum(d => d.paidIntoduceBonus + d.paidpositionBonus + d.paidPresentageBonus + d.paidshareBonus + d.paidThirdStageBonus + d.paidFifthStageBonus);
                double bon = next.Sum(d => d.paidpositionBonus);
                if (bon == 0)
                {
                    freepositionbonus.Add(0);
                }
                else
                {
                    freepositionbonus.Add(Convert.ToInt32((next.Sum(d => d.paidpositionBonus) / total) * 100));
                }
                bon = next.Sum(d => d.paidIntoduceBonus);
                if (bon == 0)
                {
                    freepositionbonus.Add(0);
                }
                else
                {
                    freepositionbonus.Add(Convert.ToInt32((next.Sum(d => d.paidIntoduceBonus) / total) * 100));
                }
                bon = next.Sum(d => d.paidshareBonus);
                if (bon == 0)
                {
                    freepositionbonus.Add(0);
                }
                else
                {
                    freepositionbonus.Add(Convert.ToInt32((next.Sum(d => d.paidshareBonus) / total) * 100));
                }
                bon = next.Sum(d => d.paidThirdStageBonus);
                if (bon == 0)
                {
                    freepositionbonus.Add(0);
                }
                else
                {
                    freepositionbonus.Add(Convert.ToInt32((next.Sum(d => d.paidThirdStageBonus) / total) * 100));
                }
                bon = next.Sum(d => d.paidFifthStageBonus);
                if (bon == 0)
                {
                    freepositionbonus.Add(0);
                }
                else
                {
                    freepositionbonus.Add(Convert.ToInt32((next.Sum(d => d.paidFifthStageBonus) / total) * 100));
                }
                bon = next.Sum(d => d.paidPresentageBonus);
                if (bon == 0)
                {
                    freepositionbonus.Add(0);
                }
                else
                {
                    freepositionbonus.Add(Convert.ToInt32((next.Sum(d => d.paidPresentageBonus) / total) * 100));
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    freepositionbonus.Add(0);
                }
            }

            return freepositionbonus;
        }
        public dynamic membersprestage()
        {
            int i = 0;
            int x = 0;
            List<int> memberpresent = new List<int>();
            int count6personcheck = db.StageOnes.GroupBy(d => d.introducePromoCode).Count();
            if (count6personcheck != 0)
            {
                var count6person = db.StageOnes.GroupBy(d => d.introducePromoCode).ToList();
                foreach (var a in count6person)
                {
                    if (a.Count() >= 6)
                    {
                        i++;
                    }
                    else if (a.Count() >= 4)
                    {
                        x++;
                    }
                }
            }
            memberpresent.Add(i);
            memberpresent.Add(x);

            return memberpresent;
        }

        public dynamic memberamountofpresentagebonus()
        {
            string membership = "";
            int i = 0;
            int x = 0;
            List<int> memberpresent = new List<int>();
            int count6personcheck = db.StageOnes.GroupBy(d => d.introducePromoCode).Count();
            if (count6personcheck != 0)
            {
                var count6person = db.StageOnes.GroupBy(d => d.introducePromoCode).ToList();
                foreach (var a in count6person)
                {
                    if (a.Count() >= 6)
                    {
                        double temporybonusvalue = 0;
                        membership = a.ElementAt(0).membershipNo;
                        int count = db.PaidBonuss.Where(d => d.memberId == membership).Count();
                        if (count != 0)
                        {
                            temporybonusvalue = db.PaidBonuss.Where(d => d.memberId == membership).Sum(d => d.paidPresentageBonus);
                        }

                        i = i + Convert.ToInt32(temporybonusvalue);

                    }
                    else if (a.Count() >= 4)
                    {
                        membership = a.ElementAt(0).membershipNo;
                        double temporybonusvalue1 = 0;
                        int count1 = db.PaidBonuss.Where(d => d.memberId == membership).Count();
                        if (count1 != 0)
                        {
                            temporybonusvalue1 = db.PaidBonuss.Where(d => d.memberId == membership).Sum(d => d.paidPresentageBonus);
                        }
                        x = x + Convert.ToInt32(temporybonusvalue1);
                    }
                }
            }


            memberpresent.Add(i);
            memberpresent.Add(x);

            return memberpresent;
        }
        public dynamic freepositontotalbonus()
        {
            List<double> freetotal = new List<double>();
            double introducebonus = 0.0;
            double positionbonus = 0.0;
            double profitpresentagebonus = 0.0;
            double sharebonus = 0.0;
            double thirdstagebonus = 0.0;
            double fifthstagebonus = 0.0;

            double count = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 5).Join(db.PaidBonuss.Where(d => d.positionHistory == 1), o => new { member = o.membershipNo, bcnum = o.bcNo }, m => new { member = m.memberId, bcnum = m.bcNumber }, (o, m) => new { o.treeId, o.membershipNo, o.bcNo, m.memberId, m.paidBonusDateTime, m.paidBonusId, m.paidFifthStageBonus, m.paidThirdStageBonus, m.paidpositionBonus, m.paidPresentageBonus, m.paidshareBonus, m.paidIntoduceBonus, m.positionHistory }).Count();
            if (count != 0)
            {
                //int introduce = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 2).Join(db.PaidBonuss, o => new { member = o.membershipNo, bcnum = o.bcNo, jump = o.jumpHistory }, m => new { member = m.memberId, bcnum = m.bcNumber, jump = m.positionHistory }, (o, m) => new { m.paidIntoduceBonus });
                introducebonus = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 5).Join(db.PaidBonuss.Where(d => d.positionHistory == 1), o => new { member = o.membershipNo, bcnum = o.bcNo }, m => new { member = m.memberId, bcnum = m.bcNumber }, (o, m) => new { o.treeId, o.membershipNo, o.bcNo, m.memberId, m.paidBonusDateTime, m.paidBonusId, m.paidFifthStageBonus, m.paidThirdStageBonus, m.paidpositionBonus, m.paidPresentageBonus, m.paidshareBonus, m.paidIntoduceBonus, m.positionHistory }).Sum(d => d.paidIntoduceBonus);
                positionbonus = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 5).Join(db.PaidBonuss.Where(d => d.positionHistory == 1), o => new { member = o.membershipNo, bcnum = o.bcNo }, m => new { member = m.memberId, bcnum = m.bcNumber }, (o, m) => new { o.treeId, o.membershipNo, o.bcNo, m.memberId, m.paidBonusDateTime, m.paidBonusId, m.paidFifthStageBonus, m.paidThirdStageBonus, m.paidpositionBonus, m.paidPresentageBonus, m.paidshareBonus, m.paidIntoduceBonus, m.positionHistory }).Sum(d => d.paidpositionBonus);
                profitpresentagebonus = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 5).Join(db.PaidBonuss.Where(d => d.positionHistory == 1), o => new { member = o.membershipNo, bcnum = o.bcNo }, m => new { member = m.memberId, bcnum = m.bcNumber }, (o, m) => new { o.treeId, o.membershipNo, o.bcNo, m.memberId, m.paidBonusDateTime, m.paidBonusId, m.paidFifthStageBonus, m.paidThirdStageBonus, m.paidpositionBonus, m.paidPresentageBonus, m.paidshareBonus, m.paidIntoduceBonus, m.positionHistory }).Sum(d => d.paidPresentageBonus);
                sharebonus = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 5).Join(db.PaidBonuss.Where(d => d.positionHistory == 1), o => new { member = o.membershipNo, bcnum = o.bcNo }, m => new { member = m.memberId, bcnum = m.bcNumber }, (o, m) => new { o.treeId, o.membershipNo, o.bcNo, m.memberId, m.paidBonusDateTime, m.paidBonusId, m.paidFifthStageBonus, m.paidThirdStageBonus, m.paidpositionBonus, m.paidPresentageBonus, m.paidshareBonus, m.paidIntoduceBonus, m.positionHistory }).Sum(d => d.paidshareBonus);
                thirdstagebonus = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 5).Join(db.PaidBonuss.Where(d => d.positionHistory == 1), o => new { member = o.membershipNo, bcnum = o.bcNo }, m => new { member = m.memberId, bcnum = m.bcNumber }, (o, m) => new { o.treeId, o.membershipNo, o.bcNo, m.memberId, m.paidBonusDateTime, m.paidBonusId, m.paidFifthStageBonus, m.paidThirdStageBonus, m.paidpositionBonus, m.paidPresentageBonus, m.paidshareBonus, m.paidIntoduceBonus, m.positionHistory }).Sum(d => d.paidThirdStageBonus);
                fifthstagebonus = db.StageOnes.Where(d => d.freeStatus == 3 || d.freeStatus == 5).Join(db.PaidBonuss.Where(d => d.positionHistory == 1), o => new { member = o.membershipNo, bcnum = o.bcNo }, m => new { member = m.memberId, bcnum = m.bcNumber }, (o, m) => new { o.treeId, o.membershipNo, o.bcNo, m.memberId, m.paidBonusDateTime, m.paidBonusId, m.paidFifthStageBonus, m.paidThirdStageBonus, m.paidpositionBonus, m.paidPresentageBonus, m.paidshareBonus, m.paidIntoduceBonus, m.positionHistory }).Sum(d => d.paidFifthStageBonus);
            }
            freetotal.Add(introducebonus);
            freetotal.Add(positionbonus);
            freetotal.Add(profitpresentagebonus);
            freetotal.Add(sharebonus);
            freetotal.Add(thirdstagebonus);
            freetotal.Add(fifthstagebonus);

            return freetotal;
        }
    }
}