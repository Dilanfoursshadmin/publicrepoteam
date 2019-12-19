using Newtonsoft.Json;
using RocketSystem.Controllers;
using RocketSystem.DbLink;
using RocketSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RocketSystem.Classes
{
    public class BonusCalculation
    {
        public static int bonus = 0;
        public static long count, min, max, defferencelevel, temposrynumbersecondpower, temporylevel, numberofintroducer;
        public static int positioncount = 0;
        public static int maxlevel, allpostitioncount, countofbcnumber, introudcebonus;
        public static int allbonus = 0;
        public static string introducertrue = "no";
        public static double memberBonus;
        public static int treelevelss, rotatecount, highsppedpositioncount, temporyallbonus, functionrunorno;
        public static int introduceoneprice, intoducetwoprice, intoducethreeprice, maxlevelhighspeed, temporythirdlevelbonus;
        public static int introducers, introudcemans, temporyzerolevelbonus, temporyfirstlevelbonus, temporysecondlevelbonus;
        public static int tenthousentposition = 0;
        public static int hundredthousentposition = 0;
        public static int thurtyfivethousentposition = 0;
        public static double allPositionsbonus = 0.0;

        protected static int thismonth = DateTime.Now.AddMonths(1).Month;
        //protected static int thismonth = DateTime.Now.Month + 1;//remove + 1
        protected static int lastmonth = DateTime.Now.Month;//add -1
        protected static int year = DateTime.Now.AddYears(1).Year;
        protected static int lastyear = DateTime.Now.Year;

         
        private static DataAccessLayer db = new DataAccessLayer();


        public dynamic CalallbonusAdmin()
        {


            List<PaidBonus> addlistpaidbonus = new List<PaidBonus>();
            var allmeberlistall = db.StageOnes.GroupBy(d => d.membershipNo).ToList();
            int x = 0;
            foreach (var members1 in allmeberlistall)
            {
                string membernumber = members1.Key;

                var allmemberlistfirst = db.StageOnes.Where(d => d.membershipNo == membernumber).ToList();
                foreach (var members in allmemberlistfirst)
                {
                    StageOne bonusfirst = new StageOne();
                    StageFive bonusfirstfive = new StageFive();
                    StageThree bonusfirstthird = new StageThree();
                    bonusfirst.membershipNo = members.membershipNo;
                    bonusfirst.bcNo = members.bcNo;
                    bonusfirst.jumpHistory = members.jumpHistory;

                    var claculateintroducebonus = MyMethod(bonusfirst);
                    PaidBonus bonusdetails = new PaidBonus();
                    bonusdetails.memberId = members.membershipNo;
                    bonusdetails.bcNumber = members.bcNo;

                    bonusdetails.positionHistory = Convert.ToInt32(members.jumpHistory);
                    int jumpingcolumn = Convert.ToInt32(members.jumpHistory);
                    var count1 = db.PaidBonuss.Where(d => d.memberId == members.membershipNo && d.bcNumber == members.bcNo && d.positionHistory == jumpingcolumn).Count();
                    double introducebonuspaid = 0.0;
                    if (count1 != 0)
                    {
                        introducebonuspaid = db.PaidBonuss.Where(d => d.memberId == members.membershipNo && d.bcNumber == members.bcNo && d.positionHistory == jumpingcolumn).Sum(d => d.paidIntoduceBonus);
                    }
                    //bonusdetails.paidIntoduceBonus = claculateintroducebonus.bonus - introducebonuspaid;
                    bonusdetails.paidIntoduceBonus = claculateintroducebonus.bonus;

                    bonusfirstfive.membershipNo = members.membershipNo;
                    bonusfirstfive.bcNo = members.bcNo;
                    bonusfirstfive.jumpHistory = members.jumpHistory;
                    var fivestagebonus = CalculateBonusStageFive(bonusfirstfive);
                    int jumpingcolumn2 = Convert.ToInt32(members.jumpHistory);
                    int count3 = db.PaidBonuss.Where(d => d.memberId == members.membershipNo && d.bcNumber == members.bcNo && d.positionHistory == jumpingcolumn2).Count();
                    double paidstagefive = 0.0;
                    if (count3 != 0)
                    {
                        paidstagefive = db.PaidBonuss.Where(d => d.memberId == members.membershipNo && d.bcNumber == members.bcNo && d.positionHistory == jumpingcolumn2).Sum(d => d.paidFifthStageBonus);
                    }
                    //bonusdetails.paidFifthStageBonus = fivestagebonus.bonus5 - paidstagefive;
                    bonusdetails.paidFifthStageBonus = fivestagebonus.bonus5; //not get privious bonus
                    var levelcolumn = db.StageFives.Where(d => d.membershipNo == bonusdetails.memberId && d.bcNo == bonusdetails.bcNumber && d.jumpHistory == bonusdetails.positionHistory.ToString()).FirstOrDefault();
                    bonusdetails.complteorno = 0;
                    if (levelcolumn != null)
                    {
                        max = (long)Math.Pow(2, 3) * levelcolumn.treeColumn;
                        min = (long)Math.Pow(2, 3) * (levelcolumn.treeColumn - 1) + 1;

                        int countcolumn = db.StageFives.Where(d => d.treeLevel == levelcolumn.treeLevel + 3 && d.treeColumn >= min && d.treeColumn <= max).Count();
                        if (countcolumn == 8)
                        {
                            bonusdetails.complteorno = 1;
                        }
                        else
                        {
                            bonusdetails.complteorno = 0;
                        }
                    }
                    bonusfirstthird.membershipNo = members.membershipNo;
                    bonusfirstthird.bcNo = members.bcNo;
                    bonusfirstthird.jumpHistory = members.jumpHistory;

                    var thirdstagebonus = CalculateBonusStageThree(bonusfirstthird);
                    int jumpingcolumn4 = Convert.ToInt32(members.jumpHistory);
                    int count5 = db.PaidBonuss.Where(d => d.memberId == members.membershipNo && d.bcNumber == members.bcNo && d.positionHistory == jumpingcolumn4).Count();
                    double paidstagethird = 0.0;
                    if (count5 != 0)
                    {
                        paidstagethird = db.PaidBonuss.Where(d => d.memberId == members.membershipNo && d.bcNumber == members.bcNo && d.positionHistory == jumpingcolumn4).Sum(d => d.paidThirdStageBonus);
                    }
                    //bonusdetails.paidThirdStageBonus = thirdstagebonus.bonus3 - paidstagethird;
                    bonusdetails.paidThirdStageBonus = thirdstagebonus.bonus3;

                    if (members.bcNo == 1)
                    {
                        double prsentagebonus = PercentageBonusForEachMember(members.membershipNo, members.bcNo);
                        bonusdetails.paidPresentageBonus = prsentagebonus;

                        double positionbon = positionBonus(members.membershipNo, 3);
                        bonusdetails.paidpositionBonus = positionbon;
                    }
                    else
                    {
                        bonusdetails.paidPresentageBonus = 0.0;
                        bonusdetails.paidpositionBonus = 0.0;
                    }
                    int months = DateTime.Now.Month;
                    int years = DateTime.Now.Year;

                    bonusdetails.paidBonusDateTime = new DateTime(years, months, 30);

                    PaidBonus bonusdetails1 = new PaidBonus();
                    bonusdetails1 = bonusdetails;
                    addlistpaidbonus.Add(bonusdetails1);
                    x = x + 1;
                }

                var allmeberlist1 = db.StageThrees.Where(d => d.membershipNo == membernumber && d.jumpHistory == "3" || d.membershipNo == membernumber && d.jumpHistory == "2").ToList();
                foreach (var allmemberlistone in allmeberlist1)
                {
                    PaidBonus bonusdetails = new PaidBonus();
                    var stagetrees = new StageThree();
                    stagetrees.membershipNo = allmemberlistone.membershipNo;
                    stagetrees.bcNo = allmemberlistone.bcNo;
                    stagetrees.jumpHistory = allmemberlistone.jumpHistory;

                    var stagebonus = CalculateBonusStageThree(stagetrees);
                    int jumpingcolumn5 = Convert.ToInt32(allmemberlistone.jumpHistory);
                    int count6 = db.PaidBonuss.Where(d => d.memberId == allmemberlistone.membershipNo && d.bcNumber == allmemberlistone.bcNo && d.positionHistory == jumpingcolumn5).Count();
                    double thidingdbonus = 0.0;
                    if (count6 != 0)
                    {
                        thidingdbonus = db.PaidBonuss.Where(d => d.memberId == allmemberlistone.membershipNo && d.bcNumber == allmemberlistone.bcNo && d.positionHistory == jumpingcolumn5).Sum(d => d.paidThirdStageBonus);
                    }
                    //bonusdetails.paidThirdStageBonus = stagebonus.bonus3 - thidingdbonus;
                    bonusdetails.paidThirdStageBonus = stagebonus.bonus3;
                    var stagetrees1 = new StageFive();
                    stagetrees1.membershipNo = allmemberlistone.membershipNo;
                    stagetrees1.bcNo = allmemberlistone.bcNo;
                    stagetrees1.jumpHistory = allmemberlistone.jumpHistory;
                    var stagebonus1 = CalculateBonusStageFive(stagetrees1);

                    int jumpingcolumn6 = Convert.ToInt32(allmemberlistone.jumpHistory);
                    int count7 = db.PaidBonuss.Where(d => d.memberId == allmemberlistone.membershipNo && d.bcNumber == allmemberlistone.bcNo && d.positionHistory == jumpingcolumn6).Count();
                    double fivesdbonus = 0.0;
                    if (count7 != 0)
                    {
                        fivesdbonus = db.PaidBonuss.Where(d => d.memberId == allmemberlistone.membershipNo && d.bcNumber == allmemberlistone.bcNo && d.positionHistory == jumpingcolumn6).Sum(d => d.paidFifthStageBonus);
                    }
                    bonusdetails.paidFifthStageBonus = stagebonus1.bonus5 - fivesdbonus;
                    bonusdetails.memberId = allmemberlistone.membershipNo;
                    bonusdetails.bcNumber = allmemberlistone.bcNo;
                    bonusdetails.positionHistory = Convert.ToInt32(allmemberlistone.jumpHistory);
                    bonusdetails.paidIntoduceBonus = 0.0;
                    bonusdetails.paidPresentageBonus = 0.0;
                    bonusdetails.paidpositionBonus = 0.0;

                    int months = DateTime.Now.Month;
                    int years = DateTime.Now.Year;

                    bonusdetails.paidBonusDateTime = new DateTime(years, months, 30);

                    PaidBonus bonusdetails2 = new PaidBonus();
                    bonusdetails2 = bonusdetails;
                    addlistpaidbonus.Add(bonusdetails2);
                }
            }
            return addlistpaidbonus;
        }
        public dynamic calculatepositionbonusall(int cart)
        {
            var memberlist = db.StageOnes.GroupBy(d => d.membershipNo).ToList();
            List<PaidBonus> bonuspay = new List<PaidBonus>();

            foreach (var a in memberlist)
            {
                double poositionbonuscart = positionBonus(a.Key, cart);

                var paidbonusdetail = new PaidBonus();
                paidbonusdetail.memberId = a.Key;
                paidbonusdetail.bcNumber = 1;
                paidbonusdetail.positionHistory = 1;
                int count = db.PaidBonuss.Where(d => d.memberId == a.Key).Count();
                double posbonus = 0;
                if (count != 0)
                {
                    posbonus = db.PaidBonuss.Where(d => d.memberId == a.Key).Sum(d => d.paidpositionBonus);
                }

                //paidbonusdetail.paidpositionBonus = poositionbonuscart - posbonus;
                paidbonusdetail.paidpositionBonus = poositionbonuscart;

                int year = DateTime.Now.Year;
                int thatmonth = DateTime.Now.Month;
                if (cart == 1)
                {
                    paidbonusdetail.paidBonusDateTime = new DateTime(year, thatmonth, 10);
                }
                else if (cart == 2)
                {
                    paidbonusdetail.paidBonusDateTime = new DateTime(year, thatmonth, 20);
                }
                var paidbonusdetail1 = new PaidBonus();

                paidbonusdetail1 = paidbonusdetail;
                if (poositionbonuscart != 0)
                {
                    bonuspay.Add(paidbonusdetail1);
                }
            }
            return bonuspay;
        }
        public static double positionBonus(string membershipnumber, int cart)
        {

            double value = db.BonusDetails.Where(d => d.bonusId == 4).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            double presentage = value / 100;
            int yerthis = DateTime.Now.Year;
            if (lastmonth == 12)
            {
                yerthis = yerthis + 1;
            }

            DateTime firstDay = new DateTime(lastyear, lastmonth, 1);
            DateTime lastDay = new DateTime(yerthis, lastmonth, 1);

            if (cart == 1)
            {
                firstDay = new DateTime(lastyear, lastmonth, 1);
                lastDay = new DateTime(lastyear, lastmonth, 11);
            }
            else if (cart == 2)
            {
                firstDay = new DateTime(lastyear, lastmonth, 11);
                lastDay = new DateTime(lastyear, lastmonth, 21);
            }
            else if (cart == 3)
            {
                firstDay = new DateTime(lastyear, lastmonth, 21);
                lastDay = new DateTime(yerthis, thismonth, 1);

            }

            var introducepostion = db.StageOnes.Where(d => d.membershipNo == membershipnumber && d.bcNo == 1).FirstOrDefault();
            var countdetails = db.StageOnes.Where(p => (p.package == 1 && p.introducePromoCode == introducepostion.positionCode && p.entryDate >= firstDay && p.entryDate < lastDay)).Join(db.Packages, p => p.package, o => o.packageId, (p, o) => new { p.membershipNo, p.bcNo, o.packagePrize }).ToList();
            var countdetails1 = db.StageOnes.Where(p => (p.package == 2 && p.introducePromoCode == introducepostion.positionCode && p.entryDate >= firstDay && p.entryDate < lastDay)).Join(db.Packages, p => p.package, o => o.packageId, (p, o) => new { p.membershipNo, p.bcNo, o.packagePrize }).ToList();
            var countdetails2 = db.StageOnes.Where(p => (p.package == 3 && p.introducePromoCode == introducepostion.positionCode && p.entryDate >= firstDay && p.entryDate < lastDay)).Join(db.Packages, p => p.package, o => o.packageId, (p, o) => new { p.membershipNo, p.bcNo, o.packagePrize }).ToList();



            if (countdetails.Count() != 0)
            {
                hundredthousentposition = Convert.ToInt32(countdetails[0].packagePrize);
            }
            if (countdetails1.Count() != 0)
            {
                thurtyfivethousentposition = Convert.ToInt32(countdetails1[0].packagePrize);
            }
            if (countdetails2.Count() != 0)
            {
                tenthousentposition = Convert.ToInt32(countdetails2[0].packagePrize);
            }

            allPositionsbonus = (countdetails.Count() * hundredthousentposition * presentage) + (countdetails1.Count() * thurtyfivethousentposition * presentage) + (countdetails2.Count() * tenthousentposition * presentage);
            return allPositionsbonus;
        }
        public dynamic Presentagebonusadmin()
        {
            List<PaidBonus> newlist = new List<PaidBonus>();
            var peopeles = db.StageOnes.GroupBy(d => d.membershipNo).ToList();
            foreach (var a in peopeles)
            {
                double presentagebonus = PercentageBonusForEachMember(a.Key, 0);
                double sharebonus = shareBonus(a.Key);

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                DateTime first = new DateTime(year, month, 25);
                DateTime second = new DateTime(year + 1, 1, 1);
                if (month == 12)
                {
                    second = new DateTime(year + 1, 1, 1);
                }
                else
                {
                    second = new DateTime(year, month + 1, 1);
                }

                var listing = new PaidBonus();
                listing.memberId = a.Key;
                listing.bcNumber = 1;
                listing.positionHistory = 1;
                var x = db.PaidBonuss.Where(d => d.memberId == a.Key && d.bcNumber == 1 && d.positionHistory == 1 && d.paidBonusDateTime >= first && d.paidBonusDateTime < second).OrderByDescending(d => d.paidBonusId).FirstOrDefault();
                listing.paidBonusId = x.paidBonusId;
                listing.paidPresentageBonus = presentagebonus;
                listing.paidBonusDateTime = x.paidBonusDateTime;
                listing.paidpositionBonus = x.paidpositionBonus;
                listing.paidIntoduceBonus = x.paidIntoduceBonus;
                listing.paidThirdStageBonus = x.paidThirdStageBonus;
                listing.paidFifthStageBonus = x.paidFifthStageBonus;
                listing.paidshareBonus = sharebonus;

                PaidBonus secondpaid = new PaidBonus();
                secondpaid = listing;
                newlist.Add(secondpaid);
            }

            return newlist;
        }
        public StageOne MyMethod(StageOne bonusfirst)
        {
            allbonus = 0;
            if (bonusfirst.jumpHistory == "1" || bonusfirst.bcNo == -1)
            {
                int bonus = bonusstor(bonusfirst.membershipNo, bonusfirst.bcNo);
            }
            bonus = allbonus;
            bonusfirst.bonus = bonus;
            return bonusfirst;
        }
        public static int bonusstor(string memeberid, int bcnumber)
        {


            DateTime lastmonthfirstday = new DateTime(lastyear, lastmonth, 1);
            DateTime thismonthfirstday = new DateTime(year, thismonth, 1);
            if (lastmonth != 12)
            {
                thismonthfirstday = new DateTime(lastyear, thismonth, 1);
            }

            introduceoneprice = db.BonusDetails.Where(d => d.bonusId == 1).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            intoducetwoprice = db.BonusDetails.Where(d => d.bonusId == 2).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            intoducethreeprice = db.BonusDetails.Where(d => d.bonusId == 3).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();

            functionrunorno = 0;
            treelevelss = -1;
            introudcebonus = 0;

            List<StageOne> memberlist = new List<StageOne>();
            List<StageOne> allmembertree = new List<StageOne>();
            allmembertree = db.StageOnes.ToList();// max level in all tree
            var promocodezero = db.StageOnes.Where(d => d.membershipNo == memeberid && d.bcNo == 1).FirstOrDefault();//get zero bc number promo code
            var positionnewlist = db.StageOnes.Where(d => d.introducePromoCode == promocodezero.positionCode).ToList();//number of position introducing that member;
            maxlevel = allmembertree.Max(d => d.treeLevel);
            if (bcnumber == -1)//passing combo box all value i am put number -1
            {
                memberlist = db.StageOnes.Where(d => d.membershipNo == memeberid).ToList();
            }
            else
            {
                memberlist = db.StageOnes.Where(d => d.membershipNo == memeberid && d.bcNo == bcnumber).ToList();
            }


            foreach (var member in memberlist)
            {

                temporyallbonus = 0;
                allpostitioncount = 0;
                rotatecount = 0;
                introudcebonus = 0;
                treelevelss = -1;
                functionrunorno = 0;
                allmembertree.Clear();



                for (int i = 1; i <= 15; i++)
                {

                    temposrynumbersecondpower = (int)Math.Pow(2, i);
                    min = temposrynumbersecondpower * (member.treeColumn - 1) + 1;//add position min column
                    max = member.treeColumn * ((int)Math.Pow(2, i));
                    var allmember1 = db.StageOnes.Where(d => d.treeLevel == (member.treeLevel + i) && d.treeColumn >= min && d.treeColumn <= max && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).ToList();
                    allmembertree.AddRange(allmember1);

                }

                var newList = allmembertree.OrderBy(nx => nx.treeId).ToList();
                foreach (var allmember in newList)
                {
                    DateTime positiondate = DateTime.Now;
                    if (allmember.freeStatus != 3 && allmember.freeStatus != 5)
                    {
                        var csvdatae = db.TemporaryPositions.Where(d => d.positionCode == allmember.positionCode).Join(db.CsvPositionDetails, k => k.positionId, r => r.positionId, (k, r) => new { r.csvDataId }).Join(db.CsvDatas, o => o.csvDataId, p => p.csvDataId, (o, p) => new { p.csvDate, p.csvDataId }).OrderByDescending(d => d.csvDataId).FirstOrDefault();
                        positiondate = csvdatae.csvDate;
                    }
                    else
                    {
                        if (allmember.freeStatus == 3)
                        {

                            int stagethreememberid = allmember.freeLinks;
                            var memberthird = db.StageThrees.Where(d => d.treeId == stagethreememberid).FirstOrDefault();
                            positiondate = getTime(3, memberthird);
                        }
                        else
                        {
                            int stagethrememberid = allmember.freeLinks;
                            var memberthird = db.StageFives.Where(d => d.treeId == stagethrememberid).FirstOrDefault();

                            StageThree threestageposition = new StageThree();

                            threestageposition.treeId = memberthird.treeId;
                            threestageposition.membershipNo = memberthird.membershipNo;
                            threestageposition.bcNo = memberthird.bcNo;
                            threestageposition.treeLevel = memberthird.treeLevel;
                            threestageposition.treeColumn = memberthird.treeColumn;
                            threestageposition.jump = memberthird.jump;
                            threestageposition.entryDate = memberthird.entryDate;
                            threestageposition.previousPosition = memberthird.previousPosition;
                            threestageposition.package = memberthird.package;
                            threestageposition.jumpHistory = memberthird.jumpHistory;

                            positiondate = getTime(5, threestageposition);

                        }

                        //freeposition code heare
                    }

                    int intocount = 6;
                    int freepositioncount = 0;
                    if (promocodezero.membershipNo != "000001")
                    {
                        var count = db.PositionDetails.Where(d => d.introducePromoCode == promocodezero.positionCode).Join(db.CsvPositionDetails, s => s.positionId, v => v.positionId, (k, v) => new { v.csvDataId, k.systemUpdate }).Join(db.CsvDatas, a => a.csvDataId, b => b.csvDataId, (a, b) => new { a.systemUpdate, b.csvDate }).Where(d => d.csvDate <= positiondate).FirstOrDefault();
                        int intocount1 = 0;
                        if (count != null)
                        {
                            intocount1 = db.PositionDetails.Where(d => d.introducePromoCode == promocodezero.positionCode).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, a => a.csvDataId, b => b.csvDataId, (a, b) => new { a.systemUpdate, b.csvDate }).Where(d => d.csvDate <= positiondate).Sum(d => d.systemUpdate);
                        }
                        int normalcount = 1;
                        int countings = db.PositionDetails.Where(d => d.membershipNo == promocodezero.membershipNo).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, a => a.csvDataId, b => b.csvDataId, (a, b) => new { a.systemUpdate, b.csvDate }).Where(d => d.csvDate <= positiondate).Count();
                        if (countings > 0)
                        {
                            normalcount = db.PositionDetails.Where(d => d.membershipNo == promocodezero.membershipNo).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, a => a.csvDataId, b => b.csvDataId, (a, b) => new { a.systemUpdate, b.csvDate }).Where(d => d.csvDate <= positiondate).Sum(d => d.systemUpdate);



                        }
                        freepositioncount = freepositioncount1(promocodezero.membershipNo, positiondate);
                        intocount = intocount1 + normalcount - 1 + freepositioncount;
                    }



                    if (intocount >= 2 && intocount < 4)
                    {
                        introudcebonus = introudcebonus + introduceoneprice;
                    }
                    if (intocount >= 4 && intocount < 6)
                    {
                        introudcebonus = introudcebonus + intoducetwoprice;
                    }
                    if (intocount >= 6)
                    {
                        introudcebonus = introudcebonus + intoducethreeprice;
                        int counting = allmembertree.Where(d => d.treeId > allmember.treeId).Count();
                        int highspeed = counting * intoducethreeprice;
                        introudcebonus = introudcebonus + highspeed;
                        break;
                    }
                }

                if (functionrunorno == 0)
                {
                    temporyallbonus = introudcebonus;
                    allbonus = temporyallbonus + allbonus;
                }
                else if (functionrunorno == 1)
                {
                    allbonus = temporyallbonus + allbonus;
                }

            }
            return allbonus;
        }
        public static int highspeeedbonuscalculation(int currentlevel, long firstcolumn, int firstlevel, int bonusprice, int maxlevels)
        {
            maxlevelhighspeed = maxlevels - firstlevel;
            if (maxlevelhighspeed >= 15)
            {
                maxlevels = firstlevel + 15;
            }
            positioncount = 0;
            for (int i = currentlevel; i <= maxlevels; i++)
            {
                temposrynumbersecondpower = (int)Math.Pow(2, i - firstlevel);
                min = temposrynumbersecondpower * (firstcolumn - 1) + 1;
                max = firstcolumn * ((int)Math.Pow(2, i - firstlevel));

                int counts = db.StageOnes.Where(d => d.treeLevel == i && d.treeColumn >= min && d.treeColumn <= max).Count();
                positioncount = positioncount + counts;
            }

            bonus = positioncount * bonusprice;
            return bonus;
        }
        public StageFive CalculateBonusStageFive(StageFive tree)
        {


            DateTime lastmonthfirstday = new DateTime(lastyear, lastmonth, 1);
            DateTime thismonthfirstday = new DateTime(year, thismonth, 1);
            if (lastmonth != 12)
            {
                thismonthfirstday = new DateTime(lastyear, thismonth, 1);
            }
            List<StageFive> threestagebc = new List<StageFive>();

            int levelZeroBonus = 0;
            int levelOneBonus = 0;
            int levelTwoBonus = 0;
            int levelThreeBonus = 0;

            int finalBonus = 0;
            long max = 0;
            long min = 0;
            long maxone = 0;
            long minone = 0;
            long maxtwo = 0;
            long mintwo = 0;
            //int levelCount = 0;
            int nextLevel = tree.treeLevel; // get the level in which to get the count
            if (tree.bcNo == -1)
            {

                threestagebc = db.StageFives.Where(d => d.membershipNo == tree.membershipNo).ToList();
            }
            else
            {
                threestagebc = db.StageFives.Where(d => d.membershipNo == tree.membershipNo && d.bcNo == tree.bcNo && d.jumpHistory == tree.jumpHistory).ToList();
            }


            var introducinpromo = db.StageOnes.Where(d => d.membershipNo == tree.membershipNo && d.bcNo == 1).FirstOrDefault();//get introduces promocode
            foreach (var b in threestagebc)
            {
                int temporylevelZeroBonus = 0;
                int temporylevelOneBonus = 0;
                int temporylevelTwoBonus = 0;
                int temporylevelThreeBonus = 0;

                var positionvalue = db.StageOnes.Where(p => p.package == b.package).Join(db.Packages, p => p.package, o => o.packageId, (p, o) => new { o.packagePrize }).FirstOrDefault();
                int packagepositionvalue = Convert.ToInt32(positionvalue.packagePrize);//get paackage value

                switch (packagepositionvalue)
                {
                    case 350000:
                        temporyzerolevelbonus = db.BonusDetails.Where(d => d.bonusId == 9).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporyfirstlevelbonus = db.BonusDetails.Where(d => d.bonusId == 10).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporysecondlevelbonus = db.BonusDetails.Where(d => d.bonusId == 11).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporythirdlevelbonus = db.BonusDetails.Where(d => d.bonusId == 12).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        break;

                    case 100000:
                        temporyzerolevelbonus = db.BonusDetails.Where(d => d.bonusId == 9).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporyfirstlevelbonus = db.BonusDetails.Where(d => d.bonusId == 10).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporysecondlevelbonus = db.BonusDetails.Where(d => d.bonusId == 11).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporythirdlevelbonus = db.BonusDetails.Where(d => d.bonusId == 12).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        break;

                    case 10000:
                        temporyzerolevelbonus = db.BonusDetails.Where(d => d.bonusId == 17).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporyfirstlevelbonus = db.BonusDetails.Where(d => d.bonusId == 18).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporysecondlevelbonus = db.BonusDetails.Where(d => d.bonusId == 19).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporythirdlevelbonus = db.BonusDetails.Where(d => d.bonusId == 20).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        break;

                }



                //int temporybonusequal = 0;


                nextLevel = b.treeLevel;
                max = (long)Math.Pow(2, 3) * b.treeColumn;
                min = (long)Math.Pow(2, 3) * (b.treeColumn - 1) + 1;
                maxone = (long)Math.Pow(2, 1) * b.treeColumn;
                minone = (long)Math.Pow(2, 1) * (b.treeColumn - 1) + 1;
                maxtwo = (long)Math.Pow(2, 2) * b.treeColumn;
                mintwo = (long)Math.Pow(2, 2) * (b.treeColumn - 1) + 1;
                //var member = db.trees.Where(d => d.treeLevel == b.treeLevel).ToList(); // Check whether the member is in level 0
                var putposition = db.StageFives.Where(d => d.treeLevel == b.treeLevel + 3 && d.treeColumn >= min && d.treeColumn <= max && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).ToList();
                var putpositionlistone = db.StageFives.Where(d => d.treeLevel == b.treeLevel + 1 && d.treeColumn >= minone && d.treeColumn <= maxone && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).ToList();
                var putpositionlisttwo = db.StageFives.Where(d => d.treeLevel == b.treeLevel + 2 && d.treeColumn >= mintwo && d.treeColumn <= maxtwo && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).ToList();
                putposition.AddRange(putpositionlistone);
                putposition.AddRange(putpositionlisttwo);

                StageThree objectthree = new StageThree();
                objectthree.treeId = b.treeId;
                objectthree.membershipNo = b.membershipNo;
                objectthree.bcNo = b.bcNo;
                objectthree.treeLevel = b.treeLevel;
                objectthree.treeColumn = b.treeColumn;
                objectthree.jump = b.jump;
                objectthree.entryDate = b.entryDate;
                objectthree.previousPosition = b.previousPosition;
                objectthree.package = b.package;
                objectthree.jumpHistory = b.jumpHistory;
                introducers = 6;

                if (b.membershipNo != "000001")
                {
                    int numberintroducing = calpostionTime(5, objectthree);
                    //freepositioncount
                    introducers = numberintroducing;
                }
                //DateTime melisecond = b.entryDate.AddDays(4);
                //introducers = db.StageOnes.Where(d => d.introducePromoCode == introducinpromo.positionCode && d.entryDate <= melisecond ).Count();//introudcing count 1st stage that time
                var introducerhascomethismonth = db.StageFives.Where(d => d.membershipNo == b.membershipNo && d.bcNo == b.bcNo && d.jumpHistory == b.jumpHistory && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).FirstOrDefault();

                if (introducers >= 6 && introducerhascomethismonth != null)
                {
                    if (packagepositionvalue == 350000 || packagepositionvalue == 100000)
                    {
                        temporylevelZeroBonus = temporyzerolevelbonus; //Stage 0 Bonus
                    }
                    else if (packagepositionvalue == 10000)
                    {
                        temporyallbonus = temporyzerolevelbonus;//stage 0 bonus
                    }

                }

                foreach (var c in putposition)
                {
                    max = (long)Math.Pow(2, c.treeLevel - b.treeLevel) * b.treeColumn;
                    min = (long)Math.Pow(2, c.treeLevel - b.treeLevel) * (b.treeColumn - 1) + 1;
                    //introudcemans = db.StageOnes.Where(d => d.introducePromoCode == introducinpromo.positionCode && d.entryDate <= c.entryDate.AddDays(4)).Count();

                    StageThree objectfour = new StageThree();
                    objectfour.treeId = c.treeId;
                    objectfour.membershipNo = c.membershipNo;
                    objectfour.bcNo = c.bcNo;
                    objectfour.treeLevel = c.treeLevel;
                    objectfour.treeColumn = c.treeColumn;
                    objectfour.jump = c.jump;
                    objectfour.entryDate = c.entryDate;
                    objectfour.previousPosition = c.previousPosition;
                    objectfour.package = c.package;
                    objectfour.jumpHistory = c.jumpHistory;
                    introudcemans = 6;
                    if (b.membershipNo != "000001")
                    {
                        DateTime timing = getTime(5, objectfour);
                        var countings = db.StageOnes.Where(d => d.membershipNo == b.membershipNo && d.bcNo == b.bcNo && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, k => k.introducePromoCode, (a, k) => new { k.positionId, k.systemUpdate }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate, k.systemUpdate }).Where(d => d.csvDate <= timing).FirstOrDefault();
                        int numberofintroducecount = 0;
                        if (countings != null)
                        {
                            numberofintroducecount = db.StageOnes.Where(d => d.membershipNo == b.membershipNo && d.bcNo == b.bcNo && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, k => k.introducePromoCode, (a, k) => new { k.positionId, k.systemUpdate }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate, k.systemUpdate }).Where(d => d.csvDate <= timing).Sum(d => d.systemUpdate);

                        }
                        int numberofintroudcecount1 = db.StageOnes.Where(d => d.membershipNo == b.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, r => b.membershipNo, (a, r) => new { r.systemUpdate, r.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= timing).Sum(d => d.systemUpdate);
                        int freecount = freepositioncount1(b.membershipNo, timing);
                        introudcemans = numberofintroducecount + numberofintroudcecount1 - 1 + freecount;
                    }
                    if (c.treeLevel - b.treeLevel == 3 && introudcemans >= 6 && c.treeColumn >= min && c.treeColumn <= max)
                    {
                        //temporybonusequal = 100;


                        temporylevelThreeBonus = temporylevelThreeBonus + temporythirdlevelbonus;

                    }
                    else if (c.treeLevel - b.treeLevel == 2 && introudcemans >= 6 && c.treeColumn >= min && c.treeColumn <= max)
                    {

                        //temporybonusequal = 100;
                        temporylevelTwoBonus = temporylevelTwoBonus + temporysecondlevelbonus;

                    }
                    else if (c.treeLevel - b.treeLevel == 1 && introudcemans >= 6 && c.treeColumn >= min && c.treeColumn <= max)
                    {

                        //temporybonusequal = 100;
                        temporylevelOneBonus = temporylevelOneBonus + temporyfirstlevelbonus;



                    }
                }
                levelZeroBonus = levelZeroBonus + temporylevelZeroBonus;
                levelOneBonus = levelOneBonus + temporylevelOneBonus;
                levelThreeBonus = levelThreeBonus + temporylevelThreeBonus;
                levelTwoBonus = levelTwoBonus + temporylevelTwoBonus;
            }

            finalBonus = levelZeroBonus + levelOneBonus + levelTwoBonus + levelThreeBonus;
            tree.bonus5 = finalBonus;
            return tree;

        }
        public StageThree CalculateBonusStageThree(StageThree tree)
        {


            DateTime lastmonthfirstday = new DateTime(lastyear, lastmonth, 1);
            DateTime thismonthfirstday = new DateTime(year, thismonth, 1);
            if (lastmonth != 12)
            {
                thismonthfirstday = new DateTime(lastyear, thismonth, 1);
            }
            List<StageThree> threestagebc = new List<StageThree>();

            int levelZeroBonus = 0;
            int levelOneBonus = 0;
            int levelTwoBonus = 0;
            int levelThreeBonus = 0;

            int finalBonus = 0;
            long max = 0;
            long min = 0;
            long maxone = 0;
            long minone = 0;
            long maxtwo = 0;
            long mintwo = 0;
            long levelCount = 0;
            int nextLevel = tree.treeLevel; // get the level in which to get the count
            if (tree.bcNo == -1)
            {

                threestagebc = db.StageThrees.Where(d => d.membershipNo == tree.membershipNo).ToList();
            }
            else
            {
                threestagebc = db.StageThrees.Where(d => d.membershipNo == tree.membershipNo && d.bcNo == tree.bcNo && d.jumpHistory == tree.jumpHistory).ToList();
            }


            var introducinpromo = db.StageOnes.Where(d => d.membershipNo == tree.membershipNo && d.bcNo == 1).FirstOrDefault();//get introduces promocode
            foreach (var b in threestagebc)
            {

                int temporylevelZeroBonus = 0;
                int temporylevelOneBonus = 0;
                int temporylevelTwoBonus = 0;
                int temporylevelThreeBonus = 0;
                //int temporybonusequal = 0;
                var positionvalue = db.StageOnes.Where(p => p.package == b.package).Join(db.Packages, p => p.package, o => o.packageId, (p, o) => new { o.packagePrize }).FirstOrDefault();
                int packagepositionvalue = Convert.ToInt32(positionvalue.packagePrize);//get paackage value

                switch (packagepositionvalue)
                {
                    case 350000:
                        temporyzerolevelbonus = db.BonusDetails.Where(d => d.bonusId == 5).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporyfirstlevelbonus = db.BonusDetails.Where(d => d.bonusId == 6).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporysecondlevelbonus = db.BonusDetails.Where(d => d.bonusId == 7).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporythirdlevelbonus = db.BonusDetails.Where(d => d.bonusId == 8).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        break;

                    case 100000:
                        temporyzerolevelbonus = db.BonusDetails.Where(d => d.bonusId == 5).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporyfirstlevelbonus = db.BonusDetails.Where(d => d.bonusId == 6).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporysecondlevelbonus = db.BonusDetails.Where(d => d.bonusId == 7).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporythirdlevelbonus = db.BonusDetails.Where(d => d.bonusId == 8).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        break;

                    case 10000:
                        temporyzerolevelbonus = db.BonusDetails.Where(d => d.bonusId == 13).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporyfirstlevelbonus = db.BonusDetails.Where(d => d.bonusId == 14).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporysecondlevelbonus = db.BonusDetails.Where(d => d.bonusId == 15).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        temporythirdlevelbonus = db.BonusDetails.Where(d => d.bonusId == 16).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
                        break;

                }

                nextLevel = b.treeLevel;
                max = (long)Math.Pow(2, 3) * b.treeColumn;
                min = (long)Math.Pow(2, 3) * (b.treeColumn - 1) + 1;
                maxone = (long)Math.Pow(2, 1) * b.treeColumn;
                minone = (long)Math.Pow(2, 1) * (b.treeColumn - 1) + 1;
                maxtwo = (long)Math.Pow(2, 2) * b.treeColumn;
                mintwo = (long)Math.Pow(2, 2) * (b.treeColumn - 1) + 1;
                //var member = db.trees.Where(d => d.treeLevel == b.treeLevel).ToList(); // Check whether the member is in level 0
                var putposition = db.StageThrees.Where(d => d.treeLevel == b.treeLevel + 3 && d.treeColumn >= min && d.treeColumn <= max && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).ToList();
                var putpositionlistone = db.StageThrees.Where(d => d.treeLevel == b.treeLevel + 1 && d.treeColumn >= minone && d.treeColumn <= maxone && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).ToList();
                var putpositionlisttwo = db.StageThrees.Where(d => d.treeLevel == b.treeLevel + 2 && d.treeColumn >= mintwo && d.treeColumn <= maxtwo && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).ToList();
                putposition.AddRange(putpositionlistone);
                putposition.AddRange(putpositionlisttwo);
                introducers = 6;
                if (b.membershipNo != "000001")
                {
                    int numberofintroducer = calpostionTime(3, b);
                    introducers = numberofintroducer;
                }

                //DateTime melisecond = b.entryDate.AddDays(4);
                //introducers = db.StageOnes.Where(d => d.introducePromoCode == introducinpromo.positionCode && d.entryDate <= melisecond).Count();//introudcing count 1st stage that time

                var introducerhascomethismonth = db.StageThrees.Where(d => d.membershipNo == b.membershipNo && d.bcNo == b.bcNo && d.jumpHistory == b.jumpHistory && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).FirstOrDefault();
                if (introducers >= 2 && introducerhascomethismonth != null)
                {
                    temporylevelZeroBonus = temporyzerolevelbonus; //Stage 0 Bonus
                }

                foreach (var c in putposition)
                {
                    max = (int)Math.Pow(2, c.treeLevel - b.treeLevel) * b.treeColumn;
                    min = (int)Math.Pow(2, c.treeLevel - b.treeLevel) * (b.treeColumn - 1) + 1;
                    introudcemans = 6;
                    if (b.membershipNo != "000001")
                    {
                        DateTime fourdaysafter = getTime(3, c);
                        //introudcemans = db.StageOnes.Where(d => d.introducePromoCode == introducinpromo.positionCode && d.entryDate <= fourdaysafter).Count();
                        var countings = db.StageOnes.Where(d => d.membershipNo == b.membershipNo && d.bcNo == b.bcNo && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, k => k.introducePromoCode, (a, k) => new { k.positionId, k.systemUpdate }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate, k.systemUpdate }).Where(d => d.csvDate <= fourdaysafter).FirstOrDefault();
                        int numberofintroducecount = 0;
                        if (countings != null)
                        {
                            numberofintroducecount = db.StageOnes.Where(d => d.membershipNo == b.membershipNo && d.bcNo == b.bcNo && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, k => k.introducePromoCode, (a, k) => new { k.positionId, k.systemUpdate }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate, k.systemUpdate }).Where(d => d.csvDate <= fourdaysafter).Sum(d => d.systemUpdate);
                        }
                        int numberofintroudcecount1 = 1;
                        int coun7 = db.StageOnes.Where(d => d.membershipNo == b.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, r => b.membershipNo, (a, r) => new { r.systemUpdate, r.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= fourdaysafter).Count();
                        if (coun7 != 0)
                        {
                            numberofintroudcecount1 = db.StageOnes.Where(d => d.membershipNo == b.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, r => b.membershipNo, (a, r) => new { r.systemUpdate, r.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= fourdaysafter).Sum(d => d.systemUpdate);
                        }
                        int freecount = freepositioncount1(b.membershipNo, fourdaysafter);
                        introudcemans = numberofintroducecount + numberofintroudcecount1 - 1 + freecount;

                    }

                    if (c.treeLevel - b.treeLevel == 3 && introudcemans >= 6 && c.treeColumn >= min && c.treeColumn <= max)
                    {
                        //temporybonusequal = 100;
                        temporylevelThreeBonus = temporylevelThreeBonus + temporythirdlevelbonus;
                    }
                    else if (c.treeLevel - b.treeLevel == 2 && introudcemans >= 4 && c.treeColumn >= min && c.treeColumn <= max)
                    {
                        //temporybonusequal = 100;
                        temporylevelTwoBonus = temporylevelTwoBonus + temporysecondlevelbonus;
                    }
                    else if (c.treeLevel - b.treeLevel == 1 && introudcemans >= 2 && c.treeColumn >= min && c.treeColumn <= max)
                    {
                        //temporybonusequal = 100;
                        temporylevelOneBonus = temporylevelOneBonus + temporyfirstlevelbonus;
                    }
                }


                levelZeroBonus = levelZeroBonus + temporylevelZeroBonus;
                levelOneBonus = levelOneBonus + temporylevelOneBonus;
                levelThreeBonus = levelThreeBonus + temporylevelThreeBonus;
                levelTwoBonus = levelTwoBonus + temporylevelTwoBonus;
            }
            finalBonus = levelZeroBonus + levelOneBonus + levelTwoBonus + levelThreeBonus;
            tree.bonus3 = finalBonus;
            return tree;

        }
        public static int calpostionTime(int stage, StageThree objecter)
        {
            int freepositioncount = 0;
            DateTime addtimeposition = new DateTime(2019, 08, 01);
            int numberofintroducecount = 0;
            int numberofintroudcecount1 = 0;
            int finalintroducecount = 0;
            string whatsreturn = "";
            switch (stage)
            {
                case 3:
                    switch (objecter.jumpHistory)
                    {
                        case "3":
                            whatsreturn = "third";
                            var whoistimeintroduce = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == objecter.bcNo && d.jumpHistory == "1").Join(db.TemporaryPositions, a => a.positionCode, b => b.positionCode, (a, b) => new { b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { l.csvDataId, n.positionId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.positionId, m.csvDate }).FirstOrDefault();
                            var countings = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce.csvDate).FirstOrDefault();
                            numberofintroducecount = 0;
                            if (countings != null)
                            {
                                numberofintroducecount = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce.csvDate).Sum(d => d.systemUpdate);
                            }
                            int coun123 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce.csvDate).Count();
                            numberofintroudcecount1 = 1;
                            if (coun123 != 0)
                            {
                                numberofintroudcecount1 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce.csvDate).Sum(d => d.systemUpdate);
                            }
                            freepositioncount = freepositioncount1(objecter.membershipNo, whoistimeintroduce.csvDate);
                            finalintroducecount = numberofintroducecount + numberofintroudcecount1 - 1 + freepositioncount;
                            return finalintroducecount;
                        default:
                            var tempory = db.StageTwoes.Where(d => d.treeId == objecter.previousPosition).FirstOrDefault();
                            int levelis = tempory.treeLevel + 2;
                            long maxis = (long)Math.Pow(2, 2) * tempory.treeColumn;
                            long minis = (long)Math.Pow(2, 2) * (tempory.treeColumn - 1) + 1;

                            var temporyone = db.StageTwoes.Where(d => d.treeColumn >= minis && d.treeColumn <= maxis && d.treeLevel == levelis).OrderByDescending(d => d.treeId).FirstOrDefault();

                            switch (temporyone.jumpHistory)
                            {
                                case "2":
                                    whatsreturn = "second";
                                    var whoistimeintroduce1 = db.StageOnes.Where(d => d.membershipNo == temporyone.membershipNo && d.bcNo == temporyone.bcNo && d.jumpHistory == "1").Join(db.TemporaryPositions, a => a.positionCode, b => b.positionCode, (a, b) => new { b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate }).FirstOrDefault();

                                    var countings1 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.positionId, b.systemUpdate }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate, k.systemUpdate }).Where(d => d.csvDate <= whoistimeintroduce1.csvDate).FirstOrDefault();
                                    numberofintroducecount = 0;
                                    if (countings1 != null)
                                    {
                                        numberofintroducecount = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.positionId, b.systemUpdate }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate, k.systemUpdate }).Where(d => d.csvDate <= whoistimeintroduce1.csvDate).Sum(d => d.systemUpdate);
                                    }
                                    numberofintroudcecount1 = 1;
                                    int coun1 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce1.csvDate).Count();
                                    if (coun1 != 0)
                                    {
                                        numberofintroudcecount1 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce1.csvDate).Sum(d => d.systemUpdate);
                                    }
                                    freepositioncount = freepositioncount1(objecter.membershipNo, whoistimeintroduce1.csvDate);
                                    finalintroducecount = numberofintroducecount + numberofintroudcecount1 - 1 + freepositioncount;
                                    return finalintroducecount;
                                    break;

                                case "1":
                                    whatsreturn = "first";
                                    var stageoneposition = db.StageOnes.Where(d => d.treeId == temporyone.previousPosition).FirstOrDefault();
                                    int levelis1 = stageoneposition.treeLevel + 2;
                                    long maxis1 = (long)Math.Pow(2, 2) * stageoneposition.treeColumn;
                                    long minis1 = (long)Math.Pow(2, 2) * (stageoneposition.treeColumn - 1) + 1;

                                    var temporyone2 = db.StageOnes.Where(d => d.treeColumn >= minis1 && d.treeColumn <= maxis1 && d.treeLevel == levelis1).OrderByDescending(d => d.treeId).FirstOrDefault();
                                    if (temporyone2.freeStatus != 3 && temporyone2.freeStatus != 5)
                                    {
                                        var whoistimeintroduce2 = db.StageOnes.Where(d => d.treeId == temporyone2.treeId).Join(db.TemporaryPositions, k => k.positionCode, m => m.positionCode, (k, m) => new { m.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, l => l.csvDataId, r => r.csvDataId, (l, r) => new { r.csvDate }).FirstOrDefault();
                                        var countings2 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce2.csvDate).FirstOrDefault();
                                        numberofintroducecount = 0;
                                        if (countings2 != null)
                                        {
                                            numberofintroducecount = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce2.csvDate).Sum(d => d.systemUpdate);
                                        }
                                        numberofintroudcecount1 = 1;
                                        int coun2 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce2.csvDate).Count();
                                        if (coun2 != 0)
                                        {
                                            numberofintroudcecount1 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce2.csvDate).Sum(d => d.systemUpdate);
                                        }
                                        freepositioncount = freepositioncount1(objecter.membershipNo, whoistimeintroduce2.csvDate);
                                        finalintroducecount = numberofintroducecount + numberofintroudcecount1 - 1 + freepositioncount;
                                        return finalintroducecount;
                                    }
                                    else
                                    {
                                        finalintroducecount = firststageIntroducecountThisisFreePosition1("caption", temporyone2);
                                        return finalintroducecount;
                                    }
                                    break;
                            }
                            break;
                    }





                    break;

                case 5:
                    var tempory5 = db.StageFours.Where(d => d.treeId == objecter.previousPosition).FirstOrDefault();
                    int levelis5 = tempory5.treeLevel + 2;
                    long maxis5 = (long)Math.Pow(2, 2) * tempory5.treeColumn;
                    long minis5 = (long)Math.Pow(2, 2) * (tempory5.treeColumn - 1) + 1;

                    var temporyone6 = db.StageFours.Where(d => d.treeColumn >= minis5 && d.treeColumn <= maxis5 && d.treeLevel == levelis5).OrderByDescending(d => d.treeId).FirstOrDefault();

                    var tempory6 = db.StageThrees.Where(d => d.treeId == temporyone6.previousPosition).FirstOrDefault();
                    int levelis6 = tempory6.treeLevel + 2;
                    long maxis6 = (long)Math.Pow(2, 2) * tempory6.treeColumn;
                    long minis6 = (long)Math.Pow(2, 2) * (tempory6.treeColumn - 1) + 1;

                    var temporyone7 = db.StageThrees.Where(d => d.treeColumn >= minis6 && d.treeColumn <= minis6 && d.treeLevel == levelis6).OrderByDescending(d => d.treeId).FirstOrDefault();

                    switch (temporyone7.jumpHistory)
                    {
                        case "3":
                            var whoistimeintroduce = db.StageOnes.Where(d => d.membershipNo == temporyone7.membershipNo && d.bcNo == temporyone7.bcNo && d.jumpHistory == "1").Join(db.TemporaryPositions, a => a.positionCode, b => b.positionCode, (a, b) => new { b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.positionId, m.csvDate }).FirstOrDefault();
                            var countings = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce.csvDate).FirstOrDefault();
                            numberofintroducecount = 0;
                            if (countings != null)
                            {
                                numberofintroducecount = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce.csvDate).Sum(d => d.systemUpdate);
                            }
                            numberofintroudcecount1 = 1;
                            int coun3 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce.csvDate).Count();
                            if (coun3 != 0)
                            {
                                numberofintroudcecount1 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce.csvDate).Sum(d => d.systemUpdate);
                            }
                            freepositioncount = freepositioncount1(objecter.membershipNo, whoistimeintroduce.csvDate);
                            finalintroducecount = numberofintroducecount + numberofintroudcecount1 - 1 + freepositioncount;
                            return finalintroducecount;

                        default:
                            var tempory = db.StageTwoes.Where(d => d.treeId == temporyone7.previousPosition).FirstOrDefault();
                            int levelis = tempory.treeLevel + 2;
                            long maxis = (long)Math.Pow(2, 2) * tempory.treeColumn;
                            long minis = (long)Math.Pow(2, 2) * (tempory.treeColumn - 1) + 1;

                            var temporyone = db.StageTwoes.Where(d => d.treeColumn >= minis && d.treeColumn <= maxis && d.treeLevel == levelis).OrderByDescending(d => d.treeId).FirstOrDefault();

                            switch (temporyone.jumpHistory)
                            {
                                case "2":
                                    var whoistimeintroduce1 = db.StageOnes.Where(d => d.membershipNo == temporyone.membershipNo && d.bcNo == temporyone.bcNo && d.jumpHistory == "1").Join(db.TemporaryPositions, a => a.positionCode, b => b.positionCode, (a, b) => new { b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate }).FirstOrDefault();
                                    var countings1 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.positionId, b.systemUpdate }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate, k.systemUpdate }).Where(d => d.csvDate <= whoistimeintroduce1.csvDate).FirstOrDefault();
                                    numberofintroducecount = 0;
                                    if (countings1 != null)
                                    {
                                        numberofintroducecount = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.positionId, b.systemUpdate }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate, k.systemUpdate }).Where(d => d.csvDate <= whoistimeintroduce1.csvDate).Sum(d => d.systemUpdate);
                                    }
                                    numberofintroudcecount1 = 1;
                                    int coun4 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce1.csvDate).Count();
                                    if (coun4 != 0)
                                    {
                                        numberofintroudcecount1 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce1.csvDate).Sum(d => d.systemUpdate);
                                    }
                                    freepositioncount = freepositioncount1(objecter.membershipNo, whoistimeintroduce1.csvDate);
                                    finalintroducecount = numberofintroducecount + numberofintroudcecount1 - 1 + freepositioncount;
                                    return finalintroducecount;

                                    break;

                                case "1":
                                    var stageoneposition = db.StageOnes.Where(d => d.treeId == temporyone.previousPosition).FirstOrDefault();
                                    int levelis1 = stageoneposition.treeLevel + 2;
                                    long maxis1 = (long)Math.Pow(2, 2) * stageoneposition.treeColumn;
                                    long minis1 = (long)Math.Pow(2, 2) * (stageoneposition.treeColumn - 1) + 1;

                                    var temporyone2 = db.StageOnes.Where(d => d.treeColumn >= minis1 && d.treeColumn <= maxis1 && d.treeLevel == levelis1).OrderByDescending(d => d.treeId).FirstOrDefault();
                                    if (temporyone2.freeStatus != 3 && temporyone2.freeStatus != 5)
                                    {
                                        var whoistimeintroduce2 = db.StageOnes.Where(d => d.treeId == temporyone2.treeId).Join(db.TemporaryPositions, k => k.positionCode, m => m.positionCode, (k, m) => new { m.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, l => l.csvDataId, r => r.csvDataId, (l, r) => new { r.csvDate }).FirstOrDefault();
                                        var countings2 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce2.csvDate).FirstOrDefault();
                                        numberofintroducecount = 0;
                                        if (countings2 != null)
                                        {
                                            numberofintroducecount = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.positionCode, b => b.introducePromoCode, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce2.csvDate).Sum(d => d.systemUpdate);
                                        }
                                        numberofintroudcecount1 = 1;
                                        int coun5 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce2.csvDate).Count();
                                        if (coun5 != 0)
                                        {
                                            numberofintroudcecount1 = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == 1 && d.jumpHistory == "1").Join(db.PositionDetails, a => a.membershipNo, b => b.membershipNo, (a, b) => new { b.systemUpdate, b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.systemUpdate, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.systemUpdate, m.csvDate }).Where(d => d.csvDate <= whoistimeintroduce2.csvDate).Sum(d => d.systemUpdate);
                                        }
                                        freepositioncount = freepositioncount1(objecter.membershipNo, whoistimeintroduce2.csvDate);
                                        finalintroducecount = numberofintroducecount + numberofintroudcecount1 - 1 + freepositioncount;
                                        return finalintroducecount;
                                    }
                                    else
                                    {

                                        finalintroducecount = firststageIntroducecountThisisFreePosition1("caption", temporyone2);
                                        return finalintroducecount;
                                    }

                                    break;
                            }
                            break;
                    }

                    break;



            }

            //end time
            return 0;
        }
        public static DateTime getTime(int stage, StageThree objecter)
        {
            string whatsreturn = "";
            DateTime addtimeposition = new DateTime(2019, 08, 01);
            DateTime rerundate = new DateTime(2019, 08, 01);
            int numberofintroducecount = 0;
            switch (stage)
            {
                case 3:
                    switch (objecter.jumpHistory)
                    {
                        case "3":
                            whatsreturn = "third";
                            var whoistimeintroduce = db.StageOnes.Where(d => d.membershipNo == objecter.membershipNo && d.bcNo == objecter.bcNo && d.jumpHistory == "1").Join(db.TemporaryPositions, a => a.positionCode, b => b.positionCode, (a, b) => new { b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.positionId, m.csvDate }).FirstOrDefault();
                            rerundate = whoistimeintroduce.csvDate;
                            return whoistimeintroduce.csvDate;
                        default:
                            var tempory = db.StageTwoes.Where(d => d.treeId == objecter.previousPosition).FirstOrDefault();
                            int levelis = tempory.treeLevel + 2;
                            long maxis = (long)Math.Pow(2, 2) * tempory.treeColumn;
                            long minis = (long)Math.Pow(2, 2) * (tempory.treeColumn - 1) + 1;

                            var temporyone = db.StageTwoes.Where(d => d.treeColumn >= minis && d.treeColumn <= maxis && d.treeLevel == levelis).OrderByDescending(d => d.treeId).FirstOrDefault();

                            switch (temporyone.jumpHistory)
                            {
                                case "2":
                                    whatsreturn = "second";
                                    var whoistimeintroduce1 = db.StageOnes.Where(d => d.membershipNo == temporyone.membershipNo && d.bcNo == temporyone.bcNo && d.jumpHistory == "1").Join(db.TemporaryPositions, a => a.positionCode, b => b.positionCode, (a, b) => new { b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate }).FirstOrDefault();

                                    rerundate = whoistimeintroduce1.csvDate;
                                    return whoistimeintroduce1.csvDate;

                                    break;

                                case "1":
                                    whatsreturn = "first";
                                    var stageoneposition = db.StageOnes.Where(d => d.treeId == temporyone.previousPosition).FirstOrDefault();
                                    int levelis1 = stageoneposition.treeLevel + 2;
                                    long maxis1 = (long)Math.Pow(2, 2) * stageoneposition.treeColumn;
                                    long minis1 = (long)Math.Pow(2, 2) * (stageoneposition.treeColumn - 1) + 1;

                                    var temporyone2 = db.StageOnes.Where(d => d.treeColumn >= minis1 && d.treeColumn <= maxis1 && d.treeLevel == levelis1).OrderByDescending(d => d.treeId).FirstOrDefault();
                                    if (temporyone2.freeStatus != 3 && temporyone2.freeStatus != 5)
                                    {
                                        var whoistimeintroduce2 = db.StageOnes.Where(d => d.treeId == temporyone2.treeId).Join(db.TemporaryPositions, k => k.positionCode, m => m.positionCode, (k, m) => new { m.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, l => l.csvDataId, r => r.csvDataId, (l, r) => new { r.csvDate }).FirstOrDefault();

                                        rerundate = whoistimeintroduce2.csvDate;
                                        return whoistimeintroduce2.csvDate;
                                    }
                                    else
                                    {
                                        DateTime rerundate1 = firststageIntroducecountThisisFreePosition("gettime", temporyone2);

                                        return rerundate1;


                                        //your code here
                                    }
                                    break;
                            }
                            break;
                    }





                    break;

                case 5:
                    var tempory10 = db.StageFours.Where(d => d.treeId == objecter.previousPosition).FirstOrDefault();
                    int levelis10 = tempory10.treeLevel + 2;
                    long maxis10 = (long)Math.Pow(2, 2) * tempory10.treeColumn;
                    long minis10 = (long)Math.Pow(2, 2) * (tempory10.treeColumn - 1) + 1;

                    var temporyone11 = db.StageFours.Where(d => d.treeColumn >= minis10 && d.treeColumn <= maxis10 && d.treeLevel == levelis10).OrderByDescending(d => d.treeId).FirstOrDefault();

                    var tempory11 = db.StageThrees.Where(d => d.treeId == temporyone11.previousPosition).FirstOrDefault();
                    int levelis11 = tempory11.treeLevel + 2;
                    long maxis11 = (long)Math.Pow(2, 2) * tempory11.treeColumn;
                    long minis11 = (long)Math.Pow(2, 2) * (tempory11.treeColumn - 1) + 1;

                    var temporyone12 = db.StageThrees.Where(d => d.treeColumn >= minis11 && d.treeColumn <= maxis11 && d.treeLevel == levelis11).OrderByDescending(d => d.treeId).FirstOrDefault();

                    switch (temporyone12.jumpHistory)
                    {
                        case "3":
                            var whoistimeintroduce = db.StageOnes.Where(d => d.membershipNo == temporyone12.membershipNo && d.bcNo == temporyone12.bcNo && d.jumpHistory == "1").Join(db.TemporaryPositions, a => a.positionCode, b => b.positionCode, (a, b) => new { b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { k.positionId, m.csvDate }).FirstOrDefault();

                            rerundate = whoistimeintroduce.csvDate;
                            return whoistimeintroduce.csvDate;
                        default:
                            var tempory = db.StageTwoes.Where(d => d.treeId == temporyone12.previousPosition).FirstOrDefault();
                            int levelis = tempory.treeLevel + 2;
                            long maxis = (long)Math.Pow(2, 2) * tempory.treeColumn;
                            long minis = (long)Math.Pow(2, 2) * (tempory.treeColumn - 1) + 1;

                            var temporyone = db.StageTwoes.Where(d => d.treeColumn >= minis && d.treeColumn <= maxis && d.treeLevel == levelis).OrderByDescending(d => d.treeId).FirstOrDefault();

                            switch (temporyone.jumpHistory)
                            {
                                case "2":
                                    var whoistimeintroduce1 = db.StageOnes.Where(d => d.membershipNo == temporyone.membershipNo && d.bcNo == temporyone.bcNo && d.jumpHistory == "1").Join(db.TemporaryPositions, a => a.positionCode, b => b.positionCode, (a, b) => new { b.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, k => k.csvDataId, m => m.csvDataId, (k, m) => new { m.csvDate }).FirstOrDefault();

                                    rerundate = whoistimeintroduce1.csvDate;
                                    return whoistimeintroduce1.csvDate;
                                    break;

                                case "1":
                                    var stageoneposition = db.StageOnes.Where(d => d.treeId == temporyone.previousPosition).FirstOrDefault();
                                    int levelis1 = stageoneposition.treeLevel + 2;
                                    long maxis1 = (long)Math.Pow(2, 2) * stageoneposition.treeColumn;
                                    long minis1 = (long)Math.Pow(2, 2) * (stageoneposition.treeColumn - 1) + 1;

                                    var temporyone2 = db.StageOnes.Where(d => d.treeColumn >= minis1 && d.treeColumn <= maxis1 && d.treeLevel == levelis1).OrderByDescending(d => d.treeId).FirstOrDefault();
                                    if (temporyone2.freeStatus != 3 && temporyone2.freeStatus != 5)
                                    {
                                        var whoistimeintroduce2 = db.StageOnes.Where(d => d.treeId == temporyone2.treeId).Join(db.TemporaryPositions, k => k.positionCode, m => m.positionCode, (k, m) => new { m.positionId }).Join(db.CsvPositionDetails, n => n.positionId, l => l.positionId, (n, l) => new { n.positionId, l.csvDataId }).Join(db.CsvDatas, l => l.csvDataId, r => r.csvDataId, (l, r) => new { r.csvDate }).FirstOrDefault();

                                        rerundate = whoistimeintroduce2.csvDate;
                                        return whoistimeintroduce2.csvDate;
                                    }
                                    else
                                    {
                                        DateTime rerundate1 = firststageIntroducecountThisisFreePosition("gettime", temporyone2);
                                        return rerundate1;
                                        //code heare
                                    }
                                    break;
                            }
                            break;
                    }
                    break;



            }

            //end time
            return addtimeposition;
        }
        private static double fivePeopleBonusAmount { get; set; }
        private static double tenPeopleBonusAmount { get; set; }
        public static void CalculateBonusPercentageBonus(string membershno)
        {
            double presentages1 = db.BonusDetails.Where(d => d.bonusId == 21).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            double threepersonintroduce = presentages1 / 100;
            double presentages2 = db.BonusDetails.Where(d => d.bonusId == 22).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            double fourpersonintroduce = presentages2 / 100;

            // int yerthis = DateTime.Now.Year;

            DateTime lastmonthfirstday = new DateTime(lastyear, lastmonth, 1);
            DateTime thismonthfirstday = new DateTime(year, thismonth, 1);
            if (lastmonth != 12)
            {
                thismonthfirstday = new DateTime(lastyear, thismonth, 1);
            }
            List<StageOne> newlist = new List<StageOne>();

            var memeberlist1 = db.StageOnes.GroupBy(d => d.membershipNo).ToList();

            int profit = 0;

            int checkhavesendbonus = db.PaidBonuss.Where(d => d.paidBonusDateTime >= lastmonthfirstday && d.paidBonusDateTime < thismonthfirstday).Count();
            if (checkhavesendbonus != 0)
            {
                double sendbonus = db.PaidBonuss.Where(d => d.paidBonusDateTime >= lastmonthfirstday && d.paidBonusDateTime < thismonthfirstday).Sum(d => d.paidFifthStageBonus + d.paidIntoduceBonus + d.paidpositionBonus + d.paidThirdStageBonus);

                int staga1thousendcount = db.StageOnes.Where(d => d.package == 1 && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).Count();
                int staga1thurtyfivethousendcount = db.StageOnes.Where(d => d.package == 2 && d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday).Count();
                int totalincome = staga1thousendcount * 100000 + staga1thurtyfivethousendcount * 350000;
                profit = totalincome - Convert.ToInt32(sendbonus);
            }

            int tenPeopleIntroduction = 0;
            int fivePeopleIntroduction = 0;
            int count1 = memeberlist1.Count();

            foreach (var b in memeberlist1)
            {
                string x = b.ElementAt(0).membershipNo.ToString();
                var promos = db.StageOnes.Where(d => d.membershipNo == x && d.bcNo == 1).FirstOrDefault();
                int introduceposition = db.StageOnes.Where(d => d.introducePromoCode == promos.positionCode && d.entryDate < thismonthfirstday).Count();

                if (introduceposition >= 4)
                {
                    fivePeopleIntroduction = fivePeopleIntroduction + 1;
                    if (x == membershno)
                    {
                        introducertrue = "yes1";
                    }
                }
                if (introduceposition >= 6)
                {
                    tenPeopleIntroduction = tenPeopleIntroduction + 1;
                    if (x == membershno)
                    {
                        introducertrue = "yes2";
                    }
                }
            }
            //var member = db.StageOnes.GroupBy(d => d.introducePromoCode).OrderByDescending(d=>d.Count()).ToList();

            fivePeopleBonusAmount = (threepersonintroduce * profit) / fivePeopleIntroduction; //bonus amount for people who have introduced more than five people
            tenPeopleBonusAmount = (fourpersonintroduce * profit) / tenPeopleIntroduction; //bonus amount for people who have introduced more than ten people

        }
        public static double PercentageBonusForEachMember(string membershipno, int bcno)
        {
            //int yerthis = DateTime.Now.Year;

            DateTime lastmonthfirstday = new DateTime(lastyear, lastmonth, 1);
            DateTime thismonthfirstday = new DateTime(year, thismonth, 1);
            if (lastmonth != 12)
            {
                thismonthfirstday = new DateTime(lastyear, thismonth, 1);
            }

            CalculateBonusPercentageBonus(membershipno);
            var introducer = db.StageOnes.Where(d => d.membershipNo == membershipno && d.bcNo == 1).FirstOrDefault();

            int inrodocuercount = db.StageOnes.Where(d => d.introducePromoCode == introducer.positionCode && d.entryDate < thismonthfirstday).Count();


            if (inrodocuercount >= 6)
            {
                memberBonus = tenPeopleBonusAmount + fivePeopleBonusAmount;
            }
            else
            {
                memberBonus = 0;
            }
            if (inrodocuercount >= 4 && inrodocuercount < 6)
            {
                memberBonus = fivePeopleBonusAmount;
            }


            return memberBonus;

        }
        public static double positionBonus(string membershipnumber)
        {
            double presentages1 = db.BonusDetails.Where(d => d.bonusId == 4).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            double positionbonusvalue = presentages1 / 100;

            var introducepostion = db.StageOnes.Where(d => d.membershipNo == membershipnumber && d.bcNo == 1).FirstOrDefault();
            var countdetails = db.StageOnes.Where(p => (p.package == 1 && p.introducePromoCode == introducepostion.positionCode)).Join(db.Packages, p => p.package, o => o.packageId, (p, o) => new { p.membershipNo, p.bcNo, o.packagePrize }).ToList();
            var countdetails1 = db.StageOnes.Where(p => (p.package == 2 && p.introducePromoCode == introducepostion.positionCode)).Join(db.Packages, p => p.package, o => o.packageId, (p, o) => new { p.membershipNo, p.bcNo, o.packagePrize }).ToList();
            var countdetails2 = db.StageOnes.Where(p => (p.package == 3 && p.introducePromoCode == introducepostion.positionCode)).Join(db.Packages, p => p.package, o => o.packageId, (p, o) => new { p.membershipNo, p.bcNo, o.packagePrize }).ToList();



            if (countdetails.Count() != 0)
            {
                hundredthousentposition = Convert.ToInt32(countdetails[0].packagePrize);
            }
            if (countdetails1.Count() != 0)
            {
                thurtyfivethousentposition = Convert.ToInt32(countdetails1[0].packagePrize);
            }
            if (countdetails2.Count() != 0)
            {
                tenthousentposition = Convert.ToInt32(countdetails2[0].packagePrize);
            }

            allPositionsbonus = (countdetails.Count() * hundredthousentposition * positionbonusvalue) + (countdetails1.Count() * thurtyfivethousentposition * positionbonusvalue) + (countdetails2.Count() * tenthousentposition * positionbonusvalue);
            return allPositionsbonus;
        }
        public static double shareBonus(string membernumber)
        {
            int coinone = db.BonusDetails.Where(d => d.bonusId == 23).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            int cointwo = db.BonusDetails.Where(d => d.bonusId == 24).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();

            double profiting = db.BonusDetails.Where(d => d.bonusId == 25).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            double profitingcoinpresentage = profiting / 100;

            //int yerthis = DateTime.Now.Year;

            DateTime lastmonthfirstday = new DateTime(lastyear, lastmonth, 1);
            DateTime thismonthfirstday = new DateTime(year, thismonth, 1);
            if (lastmonth != 12)
            {
                thismonthfirstday = new DateTime(lastyear, thismonth, 1);
            }
            int count = db.PaidBonuss.Where(d => d.paidBonusDateTime >= lastmonthfirstday && d.paidBonusDateTime < thismonthfirstday).Count();
            double sendbonus = 0.0;
            if (count != 0)
            {
                sendbonus = db.PaidBonuss.Where(d => d.paidBonusDateTime >= lastmonthfirstday && d.paidBonusDateTime < thismonthfirstday).Sum(d => d.paidFifthStageBonus + d.paidIntoduceBonus + d.paidpositionBonus + d.paidThirdStageBonus);
            }
            double income = db.StageOnes.Where(d => d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday && d.package == 1).Count() * 100000 + db.StageOnes.Where(d => d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday && d.package == 2).Count() * 350000;
            double profit = income - sendbonus;
            double profit5present = profit * profitingcoinpresentage;

            long apicoinmember = Task.Run(() => BonusCalculation.numberofcoin(membernumber)).Result;
            long apitotalcoin = Task.Run(() => BonusCalculation.getAllCoin()).Result;


            long coin = db.StageOnes.Where(d => d.package == 1).Count() * coinone + db.StageOnes.Where(d => d.package == 2).Count() * cointwo + apitotalcoin;

            double coinvalue = profit5present / coin;
            
            //double test1 = (db.StageOnes.Where(d => d.package == 1 && d.membershipNo == membernumber).Count() * coinone + db.StageOnes.Where(d => d.package == 2 && d.membershipNo == membernumber).Count() * cointwo ) * coinvalue;
            //double test2 = apicoinmember * coinvalue;

            double membercoin = db.StageOnes.Where(d => d.package == 1 && d.membershipNo == membernumber).Count() * coinone + db.StageOnes.Where(d => d.package == 2 && d.membershipNo == membernumber).Count() * cointwo + apicoinmember;
            double sharebonus = membercoin * coinvalue;
            return sharebonus;
        }
        public static DateTime firststageIntroducecountThisisFreePosition(string situation, StageOne allmember)
        {
            DateTime newdate = new DateTime(2019, 08, 05);
            if (allmember.freeStatus == 3)
            {

                int treeidfirst = allmember.freeLinks;
                var threestageposition = db.StageThrees.Where(d => d.treeId == treeidfirst).FirstOrDefault();

                temposrynumbersecondpower = (int)Math.Pow(2, 3);
                min = temposrynumbersecondpower * (threestageposition.treeColumn - 1) + 1;//add position min column
                max = threestageposition.treeColumn * ((int)Math.Pow(2, 3));
                var Numberoflast = db.StageThrees.Where(d => d.treeLevel == (threestageposition.treeLevel + 3) && d.treeColumn >= min && d.treeColumn <= max).OrderByDescending(d => d.treeId).FirstOrDefault();


                if (situation == "gettime")
                {
                    newdate = getTime(3, Numberoflast);
                    return newdate;
                }


            }
            else if (allmember.freeStatus == 5)
            {
                int treeidfirst = allmember.freeLinks;
                var threestageposition5 = db.StageFives.Where(d => d.treeId == treeidfirst).FirstOrDefault();

                temposrynumbersecondpower = (int)Math.Pow(2, 3);
                min = temposrynumbersecondpower * (threestageposition5.treeColumn - 1) + 1;//add position min column
                max = threestageposition5.treeColumn * ((int)Math.Pow(2, 3));
                var Numberoflast = db.StageFives.Where(d => d.treeLevel == (threestageposition5.treeLevel + 3) && d.treeColumn >= min && d.treeColumn <= max).OrderByDescending(d => d.treeId).FirstOrDefault();

                StageThree threestageposition = new StageThree();

                threestageposition.treeId = Numberoflast.treeId;
                threestageposition.membershipNo = Numberoflast.membershipNo;
                threestageposition.bcNo = Numberoflast.bcNo;
                threestageposition.treeLevel = Numberoflast.treeLevel;
                threestageposition.treeColumn = Numberoflast.treeColumn;
                threestageposition.jump = Numberoflast.jump;
                threestageposition.entryDate = Numberoflast.entryDate;
                threestageposition.previousPosition = Numberoflast.previousPosition;
                threestageposition.package = Numberoflast.package;
                threestageposition.jumpHistory = Numberoflast.jumpHistory;


                if (situation == "gettime")
                {
                    newdate = getTime(5, threestageposition);
                    return newdate;
                }

            }
            return newdate;

        }
        public static int firststageIntroducecountThisisFreePosition1(string situation, StageOne allmember)
        {
            int countintroducers = 0;
            if (allmember.freeStatus == 3)
            {

                int treeidfirst = allmember.freeLinks;
                var threestageposition = db.StageThrees.Where(d => d.treeId == treeidfirst).FirstOrDefault();


                temposrynumbersecondpower = (int)Math.Pow(2, 3);
                min = temposrynumbersecondpower * (threestageposition.treeColumn - 1) + 1;//add position min column
                max = threestageposition.treeColumn * ((int)Math.Pow(2, 3));
                var Numberoflast = db.StageThrees.Where(d => d.treeLevel == (threestageposition.treeLevel + 3) && d.treeColumn >= min && d.treeColumn <= max).OrderByDescending(d => d.treeId).FirstOrDefault();


                if (situation == "caption")
                {
                    countintroducers = calpostionTime(3, threestageposition);
                    return countintroducers;
                }


            }
            else if (allmember.freeStatus == 5)
            {
                int treeidfirst = allmember.freeLinks;
                var threestageposition5 = db.StageFives.Where(d => d.treeId == treeidfirst).FirstOrDefault();


                temposrynumbersecondpower = (int)Math.Pow(2, 3);
                min = temposrynumbersecondpower * (threestageposition5.treeColumn - 1) + 1;//add position min column
                max = threestageposition5.treeColumn * ((int)Math.Pow(2, 3));
                var Numberoflast = db.StageFives.Where(d => d.treeLevel == (threestageposition5.treeLevel + 3) && d.treeColumn >= min && d.treeColumn <= max).OrderByDescending(d => d.treeId).FirstOrDefault();


                StageThree threestageposition = new StageThree();

                threestageposition.treeId = Numberoflast.treeId;
                threestageposition.membershipNo = Numberoflast.membershipNo;
                threestageposition.bcNo = Numberoflast.bcNo;
                threestageposition.treeLevel = Numberoflast.treeLevel;
                threestageposition.treeColumn = Numberoflast.treeColumn;
                threestageposition.jump = Numberoflast.jump;
                threestageposition.entryDate = Numberoflast.entryDate;
                threestageposition.previousPosition = Numberoflast.previousPosition;
                threestageposition.package = Numberoflast.package;
                threestageposition.jumpHistory = Numberoflast.jumpHistory;


                if (situation == "caption")
                {
                    countintroducers = calpostionTime(5, threestageposition);
                    return countintroducers;
                }

            }
            return countintroducers;

        }
        public static int freepositioncount1(string membershipno, DateTime positiondate)
        {
            int freepositionmemberwisecount = 0;
            var freeposition = db.StageOnes.Where(d => d.membershipNo == membershipno && d.freeStatus == 3 || d.membershipNo == membershipno && d.freeStatus == 5).ToList();
            foreach (var pos in freeposition)
            {

                switch (pos.freeStatus)
                {
                    case 3:
                        var next = db.StageThrees.Where(d => d.treeId == pos.freeLinks).FirstOrDefault();
                        DateTime freedate = getTime(3, next);
                        if (freedate <= positiondate)
                        {
                            freepositionmemberwisecount = freepositionmemberwisecount + 1;
                        }
                        break;

                    case 5:
                        var next1 = db.StageFives.Where(d => d.treeId == pos.freeLinks).FirstOrDefault();
                        StageThree nextone = new StageThree();

                        nextone.treeId = next1.treeId;
                        nextone.membershipNo = next1.membershipNo;
                        nextone.bcNo = next1.bcNo;
                        nextone.treeLevel = next1.treeLevel;
                        nextone.treeColumn = next1.treeColumn;
                        nextone.jump = next1.jump;
                        nextone.entryDate = next1.entryDate;
                        nextone.previousPosition = next1.previousPosition;
                        nextone.package = next1.package;
                        nextone.jumpHistory = next1.jumpHistory;

                        DateTime freedate1 = getTime(5, nextone);
                        if (freedate1 <= positiondate)
                        {
                            freepositionmemberwisecount = freepositionmemberwisecount + 1;
                        }
                        break;
                }
            }
            return freepositionmemberwisecount;
        }
        public static async Task<long> numberofcoin(string membershipno)
        {
            var data = db.Users.First();

            string mydate = DateTime.Now.ToString("yyyy-MM-dd");
            string final = SHA.GenerateSHA256String((membershipno + mydate));
            string path = "https://fourssh.org/users/sendCointCount?membershipno=" + membershipno + "&key=" + final;

            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(path);
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            object routes_list = json_serializer.DeserializeObject(json);
            var model = JsonConvert.DeserializeObject<ApiMyCoin>(json);
            ApiMyCoin newd = new ApiMyCoin();
            newd.coin = model.coin;
            long r = (long)Convert.ToDouble(newd.coin);
            return r;

        }
        public static async Task<long> getAllCoin()
        {
            string path = "https://fourssh.org/users/sendCointTotalCount";

            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(path);
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            object routes_list = json_serializer.DeserializeObject(json);
            var model = JsonConvert.DeserializeObject<ApiMyCoin>(json);

            ApiMyCoin totalcoin = new ApiMyCoin();
            totalcoin.coin = model.coin;
            long r = (long)Convert.ToDouble(totalcoin.coin);
            return r;

        }
        public static double getcoinvalue()
        {
            int coinone = db.BonusDetails.Where(d => d.bonusId == 23).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            int cointwo = db.BonusDetails.Where(d => d.bonusId == 24).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();

            double profiting = db.BonusDetails.Where(d => d.bonusId == 25).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            double profitingcoinpresentage = profiting / 100;

           
            DateTime lastmonthfirstday = new DateTime(lastyear, lastmonth, 1);
            DateTime thismonthfirstday = new DateTime(year, thismonth, 1);
            if (lastmonth != 12)
            {
                thismonthfirstday = new DateTime(lastyear, thismonth, 1);
            }
            int count = db.PaidBonuss.Where(d => d.paidBonusDateTime >= lastmonthfirstday && d.paidBonusDateTime < thismonthfirstday).Count();
            double sendbonus = 0.0;
            if (count != 0)
            {
                sendbonus = db.PaidBonuss.Where(d => d.paidBonusDateTime >= lastmonthfirstday && d.paidBonusDateTime < thismonthfirstday).Sum(d => d.paidFifthStageBonus + d.paidIntoduceBonus + d.paidpositionBonus + d.paidThirdStageBonus);
            }
            double income = db.StageOnes.Where(d => d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday && d.package == 1).Count() * 100000 + db.StageOnes.Where(d => d.entryDate >= lastmonthfirstday && d.entryDate < thismonthfirstday && d.package == 2).Count() * 350000;
            double profit = income - sendbonus;
            double profit5present = profit * profitingcoinpresentage;

            
            long apitotalcoin = Task.Run(() => BonusCalculation.getAllCoin()).Result;


            long coin = db.StageOnes.Where(d => d.package == 1).Count() * coinone + db.StageOnes.Where(d => d.package == 2).Count() * cointwo + apitotalcoin;

            double coinvalue = profit5present / coin;
            return coinvalue;
        }
        public dynamic sharebonusandprofitpresentagenew()
        {
            int onepackagecoin = db.BonusDetails.Where(d => d.bonusId == 23).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            int twopackagecoin = db.BonusDetails.Where(d => d.bonusId == 24).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();

            List<PaidBonus> newobj = new List<PaidBonus>();
            double coinvalues = getcoinvalue();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            DateTime first = new DateTime(year, month, 25);
            DateTime second = new DateTime(year + 1, 1, 1);
            if (month == 12)
            {
                second = new DateTime(year + 1, 1, 1);
            }
            else
            {
                second = new DateTime(year, month + 1, 1);
            }
            CalculateBonusPercentageBonus("000001");
            
            var memberlist = db.PaidBonuss.Where(d => d.positionHistory == 1 && d.paidBonusDateTime >= first && d.paidBonusDateTime < second).GroupBy(d => d.memberId).ToList();
            foreach(var groupmember in memberlist)
            {
                foreach(var memberbcs in groupmember)
                {
                    if(memberbcs.bcNumber == 1)
                    {
                        int counts = db.StageOnes.Where(d => d.membershipNo == memberbcs.memberId && d.bcNo == memberbcs.bcNumber).Join(db.StageOnes, k => k.positionCode, l => l.introducePromoCode, (k, l) => new { k.membershipNo }).Count();

                        memberbcs.paidPresentageBonusThreeperson = 0;
                        memberbcs.paidPresentageBonus =0;

                        if (counts >= 6)
                        {
                            memberbcs.paidPresentageBonusThreeperson = fivePeopleBonusAmount;
                            memberbcs.paidPresentageBonus = tenPeopleBonusAmount;

                        }
                        else if(count >= 3)
                        {
                            memberbcs.paidPresentageBonusThreeperson = fivePeopleBonusAmount;
                        }
                        long apicoinmember = Task.Run(() => BonusCalculation.numberofcoin(memberbcs.memberId)).Result;
                        memberbcs.paidshareBonus = coinvalues * apicoinmember;
                        
                    }
                    
                    var package = db.StageOnes.Where(d => d.membershipNo == memberbcs.memberId && d.bcNo == memberbcs.bcNumber).FirstOrDefault();
                    if(package.package == 2)
                    {
                        memberbcs.paidshareBonusRocket = coinvalues * twopackagecoin;
                    }
                    else
                    {
                        memberbcs.paidshareBonusRocket = coinvalues * onepackagecoin;
                    }
                    PaidBonus temporyobject = new PaidBonus();
                    temporyobject = memberbcs;

                    newobj.Add(temporyobject);


                }
            }
            return newobj;

        }
    }
}