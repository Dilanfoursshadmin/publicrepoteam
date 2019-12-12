using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RocketSystem.Classes;
using RocketSystem.DbLink;
using RocketSystem.Models;

namespace RocketSystem.Controllers
{
    public class TreesController : Controller
    {
        int stattreeid = 0;
        int stattree2id = 0;
        private DataAccessLayer db = new DataAccessLayer();

        public ActionResult checkCsvData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult checkCsvData(CsvData csvData)
        {
            List<int> arrayList = CheckCsvMemberData.csvMemberData();
            //CheckCsvMemberData.csvMemberData();
            createResult(arrayList);
            return View();
        }

        // GET: Trees
        public ActionResult Index()
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Session["IntroducerExists"] = "notcheck";
            Session["posicount"] = rules.sendMemberCountInTree(Session["MembershipNo"].ToString());
            string mem = Session["MembershipNo"].ToString();
            ViewBag.posidetails = db.PositionDetails.Where(d => d.membershipNo == mem && d.positionStatus == "pending").ToList();
            return View(db.StageOnes.ToList());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(PositionDetail positionDetail, CsvData csvData)
        {
            if (rules.sendMemberCountInTree(Session["membershipNo"].ToString()) != 0)
            {
                positionDetail.membershipNo = Session["membershipNo"].ToString();
            }
           
            var list = db.PositionDetails.Where(d => d.membershipNo == positionDetail.membershipNo).FirstOrDefault();
            if(list != null)
            {
                var positionCode = db.TemporaryPositions.Where(x => x.positionId == list.positionId).FirstOrDefault();
                Session["introducePromoCode"] = positionCode.positionCode;
            }
            else
            {
                Session["introducePromoCode"] = "E5Q517";
            }
            return RedirectToAction("insertPositionDetails", "PositionDetails", positionDetail);
        }

        public ActionResult Index2(int? id)
        {
            if (id == 1)
            {
                int bcnumval = Convert.ToInt32(Session["bcnum"]);
                if (bcnumval == 0) { Session["bcnum"] = null; }
                var membershipNo = Session["membershipNo"].ToString();
                int bcnum = Convert.ToInt32(Session["bcnum"]);
                var BcnumberList = db.StageOnes.Where(d => d.membershipNo == membershipNo).ToList();
                if (BcnumberList.Count() == 0) { return null; }

                if (Session["bcnum"] == null)
                {
                    bcnum = BcnumberList[0].treeId;
                    Session["bcnum"] = bcnum;

                }
                ArrayList firstStageList = LoginUserPosition.CalFirstBonus(membershipNo, bcnum, 1);
                var list = firstStageList.Cast<StageOne>().ToList();
                return Json(list.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).Select(d => new { tid = d.treeId, xx = d.treeColumn, yy = d.treeLevel, bn = d.bcNo, ft = d.freeStatus, idp = d.introducePromoCode, jp = d.jump, mn = d.membershipNo, pk = d.package, pos = d.positionCode }).ToList(), JsonRequestBehavior.AllowGet);
            }
            else if (id == 2)
            {
                int bcnumval = Convert.ToInt32(Session["bcnum"]);
                if (bcnumval == 0) { Session["bcnum"] = null; }
                var membershipNo = Session["membershipNo"].ToString();
                int bcnum = Convert.ToInt32(Session["bcnum"]);
                var BcnumberList = db.StageTwoes.Where(d => d.membershipNo == membershipNo).ToList();
                if (BcnumberList.Count() == 0) { return null; }

                if (Session["bcnum"] == null)
                {
                    bcnum = BcnumberList[0].treeId;
                    Session["bcnum"] = bcnum;

                }
                ArrayList firstStageList = LoginUserPosition.CalFirstBonus(membershipNo, bcnum, 2);
                var list = firstStageList.Cast<StageTwo>().ToList();

                return Json(list.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).Select(d => new { tid = d.treeId, xx = d.treeColumn, yy = d.treeLevel, bn = d.bcNo, jp = d.jump, mn = d.membershipNo, pk = d.package }).ToList(), JsonRequestBehavior.AllowGet);
            }
            else if (id == 3)
            {
                int bcnumval = Convert.ToInt32(Session["bcnum"]);

                if (bcnumval == 0) { Session["bcnum"] = null; }
                var membershipNo = Session["membershipNo"].ToString();
                int bcnum = Convert.ToInt32(Session["bcnum"]);
                var BcnumberList = db.StageThrees.Where(d => d.membershipNo == membershipNo).ToList();
                if (BcnumberList.Count() == 0) { return null; }

                if (Session["bcnum"] == null)
                {
                    bcnum = BcnumberList[0].treeId;
                    Session["bcnum"] = bcnum;
                }
                ArrayList firstStageList = LoginUserPosition.CalFirstBonus(membershipNo, bcnum, 3);
                var list = firstStageList.Cast<StageThree>().ToList();

                return Json(list.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).Select(d => new { tid = d.treeId, xx = d.treeColumn, yy = d.treeLevel, bn = d.bcNo, jp = d.jump, mn = d.membershipNo, pk = d.package }).ToList(), JsonRequestBehavior.AllowGet);

            }
            else if (id == 4)
            {
                int bcnumval = Convert.ToInt32(Session["bcnum"]);
                if (bcnumval == 0) { Session["bcnum"] = null; }
                var membershipNo = Session["membershipNo"].ToString();
                int bcnum = Convert.ToInt32(Session["bcnum"]);
                var BcnumberList = db.StageFours.Where(d => d.membershipNo == membershipNo).ToList();
                if (BcnumberList.Count() == 0) { return null; }
                if (Session["bcnum"] == null)
                {
                    bcnum = BcnumberList[0].treeId;
                    Session["bcnum"] = bcnum;
                }
                ArrayList firstStageList = LoginUserPosition.CalFirstBonus(membershipNo, bcnum, 4);
                var list = firstStageList.Cast<StageFour>().ToList();

                return Json(list.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).Select(d => new { tid = d.treeId, xx = d.treeColumn, yy = d.treeLevel, bn = d.bcNo, jp = d.jump, mn = d.membershipNo, pk = d.package }).ToList(), JsonRequestBehavior.AllowGet);

            }
            else if (id == 5)
            {
                int bcnumval = Convert.ToInt32(Session["bcnum"]);

                if (bcnumval == 0) { Session["bcnum"] = null; }
                var membershipNo = Session["membershipNo"].ToString();
                int bcnum = Convert.ToInt32(Session["bcnum"]);
                var BcnumberList = db.StageFives.Where(d => d.membershipNo == membershipNo).ToList();
                if (BcnumberList.Count() == 0) { return null; }
                if (Session["bcnum"] == null)
                {
                    bcnum = BcnumberList[0].treeId;
                    Session["bcnum"] = bcnum;

                }
                ArrayList firstStageList = LoginUserPosition.CalFirstBonus(membershipNo, bcnum, 5);
                var list = firstStageList.Cast<StageFive>().ToList();

                return Json(list.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).Select(d => new { tid = d.treeId, xx = d.treeColumn, yy = d.treeLevel, bn = d.bcNo, jp = d.jump, mn = d.membershipNo, pk = d.package }).ToList(), JsonRequestBehavior.AllowGet);

            }
            return View();
        }

        public ActionResult StageOne(int? bcnumval)
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            string mem = Session["membershipNo"].ToString();
            var getlevel = db.StageOnes.Where(a => a.bcNo == 1 && a.membershipNo == mem).FirstOrDefault();
            ViewBag.getl = getlevel.treeLevel;
            Session["StageSession"] = "true";
            if (bcnumval == 0) { Session["bcnum"] = null; }
            var membershipNo = Session["membershipNo"].ToString();
            ViewBag.BcnumberList = db.StageOnes.Where(d => d.membershipNo == membershipNo).ToList();
            //***************
            int nodata = db.StageOnes.Where(d => d.membershipNo == membershipNo).Count();
            if (nodata == 0)
            {
                ViewBag.nodata = 0;
            }
            //*************
            return View();
        }
        public ActionResult StageTwo(int? bcnumval)
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            string mem = Session["membershipNo"].ToString();
            var getlevel = db.StageTwoes.Where(a => a.bcNo == 1 && a.membershipNo == mem).FirstOrDefault();
            ViewBag.getl = getlevel.treeLevel;
            Session["StageSession"] = "true";
            if (bcnumval == 0) { Session["bcnum"] = null; }
            var membershipNo = Session["membershipNo"].ToString();
            ViewBag.BcnumberList = db.StageTwoes.Where(d => d.membershipNo == membershipNo).ToList();
            //***************
            int nodata = db.StageTwoes.Where(d => d.membershipNo == membershipNo).Count();
            if (nodata == 0)
            {
                ViewBag.nodata = 0;
            }
            //*************
            return View();
        }
        public ActionResult StageThree(int? bcnumval)
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            string mem = Session["membershipNo"].ToString();
            var getlevel = db.StageThrees.Where(a => a.bcNo == 1 && a.membershipNo == mem).FirstOrDefault();
            ViewBag.getl = getlevel.treeLevel;
            Session["StageSession"] = "true";
            if (bcnumval == 0) { Session["bcnum"] = null; }
            var membershipNo = Session["membershipNo"].ToString();
            ViewBag.BcnumberList = db.StageThrees.Where(d => d.membershipNo == membershipNo).ToList();
            //***************
            int nodata = db.StageThrees.Where(d => d.membershipNo == membershipNo).Count();
            if (nodata == 0)
            {
                ViewBag.nodata = 0;
            }
            //*************
            return View();
        }
        public ActionResult StageFour(int? bcnumval)
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            string mem = Session["membershipNo"].ToString();
            var getlevel = db.StageFours.Where(a => a.bcNo == 1 && a.membershipNo == mem).FirstOrDefault();
            ViewBag.getl = getlevel.treeLevel;
            Session["StageSession"] = "true";
            if (bcnumval == 0) { Session["bcnum"] = null; }
            var membershipNo = Session["membershipNo"].ToString();
            ViewBag.BcnumberList = db.StageFours.Where(d => d.membershipNo == membershipNo).ToList();
            //***************
            int nodata = db.StageFours.Where(d => d.membershipNo == membershipNo).Count();
            if (nodata == 0)
            {
                ViewBag.nodata = 0;
            }
            //*************
            return View();
        }
        public ActionResult StageFive(int? bcnumval)
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            string mem = Session["membershipNo"].ToString();
            var getlevel = db.StageFives.Where(a => a.bcNo == 1 && a.membershipNo == mem).FirstOrDefault();
            ViewBag.getl = getlevel.treeLevel;
            Session["StageSession"] = "true";
            if (bcnumval == 0) { Session["bcnum"] = null; }
            var membershipNo = Session["membershipNo"].ToString();
            ViewBag.BcnumberList = db.StageFives.Where(d => d.membershipNo == membershipNo).ToList();
            //***************
            int nodata = db.StageFives.Where(d => d.membershipNo == membershipNo).Count();
            if (nodata == 0)
            {
                ViewBag.nodata = 0;
            }
            //*************
            return View();
        }

        [HttpPost]
        public ActionResult StageOne(string bcnum)
        {
            Session["bcnum"] = bcnum;
            return RedirectToAction("StageOne");
        }

        [HttpPost]
        public ActionResult StageTwo(string bcnum)
        {
            Session["bcnum"] = bcnum;
            return RedirectToAction("StageTwo");
        }

        [HttpPost]
        public ActionResult StageThree(string bcnum)
        {
            Session["bcnum"] = bcnum;
            return RedirectToAction("StageThree");
        }

        [HttpPost]
        public ActionResult StageFour(string bcnum)
        {
            Session["bcnum"] = bcnum;
            return RedirectToAction("StageFour");
        }

        [HttpPost]
        public ActionResult StageFive(string bcnum)
        {
            Session["bcnum"] = bcnum;
            return RedirectToAction("StageFive");
        }

        public ActionResult popupAjax(int treeid, int stage)
        {
            if (stage == 1)
            {
                var popuodetails = db.StageOnes.Where(p => (p.treeId == treeid)).Join(db.Users, o => o.membershipNo, p => p.membershipNo,
                 (o, p) => new { o.membershipNo, o.positionCode, p.name, p.mobileNo, o.entryDate, p.nickName, o.treeLevel, o.treeColumn, o.bcNo}).ToList();
                return Json(popuodetails, JsonRequestBehavior.AllowGet);
            }
            else if (stage == 2)
            {
                var popuodetails = db.StageTwoes.Where(p => (p.treeId == treeid)).Join(db.Users, o => o.membershipNo, p => p.membershipNo,
                 (o, p) => new { o.membershipNo,  p.name, p.mobileNo, o.entryDate, p.nickName, o.treeLevel, o.treeColumn, o.bcNo }).ToList();
                return Json(popuodetails, JsonRequestBehavior.AllowGet);
            }
            else if (stage == 3)
            {
                var popuodetails = db.StageThrees.Where(p => (p.treeId == treeid)).Join(db.Users, o => o.membershipNo, p => p.membershipNo,
                  (o, p) => new { o.membershipNo, p.name, p.mobileNo, o.entryDate, p.nickName, o.treeLevel, o.treeColumn, o.bcNo }).ToList();
                return Json(popuodetails, JsonRequestBehavior.AllowGet);
            }
            else if (stage == 4)
            {
                var popuodetails = db.StageFours.Where(p => (p.treeId == treeid)).Join(db.Users, o => o.membershipNo, p => p.membershipNo,
                  (o, p) => new { o.membershipNo, p.name, p.mobileNo, o.entryDate, p.nickName, o.treeLevel, o.treeColumn, o.bcNo }).ToList();
                return Json(popuodetails, JsonRequestBehavior.AllowGet);
            }
            else if (stage == 5)
            {
                var popuodetails = db.StageFives.Where(p => (p.treeId == treeid)).Join(db.Users, o => o.membershipNo, p => p.membershipNo,
                  (o, p) => new { o.membershipNo, p.name, p.mobileNo, o.entryDate, p.nickName, o.treeLevel, o.treeColumn, o.bcNo }).ToList();
                return Json(popuodetails, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        public ActionResult Bonusview(int bcvalue1, int jumphistory)
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            Session["StageSession"] = "false";
            string membership = Session["membershipNo"].ToString();
            //new query
            var list2 = db.StageOnes.Where(d => d.membershipNo == membership && d.jumpHistory == "1").OrderBy(q => q.bcNo).ToList();
            var list2seconds = db.StageTwoes.Where(d => d.membershipNo == membership && d.jumpHistory == "2").OrderBy(q => q.bcNo).ToList();
            var list3rd = db.StageThrees.Where(d => d.membershipNo == membership && d.jumpHistory == "3").OrderBy(q => q.bcNo).ToList();
            List<StageTwo> list2second = new List<StageTwo>();

            foreach (var a in list2)
            {
                var nexttempory = new StageTwo();
                nexttempory.bcNo = a.bcNo;
                nexttempory.jumpHistory = a.jumpHistory;

                var nexttempory1 = new StageTwo();
                nexttempory1 = nexttempory;
                list2second.Add(nexttempory1);
            }

            foreach (var b in list3rd)
            {
                var nexttempory = new StageTwo();
                nexttempory.bcNo = b.bcNo;
                nexttempory.jumpHistory = b.jumpHistory;

                var nexttempory1 = new StageTwo();
                nexttempory1 = nexttempory;
                list2second.Add(nexttempory1);
            }
            
            list2second.AddRange(list2seconds);
            list2second.OrderBy(b => b.bcNo);

            List<int> selectbc = new List<int>();
            List<string> selectbcnew = new List<string>();

            selectbc.Add(bcvalue1);
            selectbcnew.Add(jumphistory.ToString());

            ViewBag.bcnumberselect = selectbc;
            ViewBag.bcnumberselect12 = selectbcnew;

            ViewBag.list2 = list2second;
            //new query end

            //new User wise Bonus
            ViewBag.month = DateTime.Now.Month;
            DateTime corecttime = DateTime.Now.AddMonths(0); //should be corect -1 month add
            int month = corecttime.Month;
            int year = corecttime.Year;
            int year1 = corecttime.Year;
            int month1 = month + 1;
            if (month == 12)
            {
                year1 = year1 + 1;
                month1 = 1;
            }
            DateTime firstdate = new DateTime(year, month, 1);
            DateTime pastmonthdate = new DateTime(year, month, 1);

            DateTime firstallbonusdate = new DateTime(year, month, 21);//only shoud change 21
            DateTime lastday = new DateTime(year1, month1, 1);
            DateTime firstdateten = new DateTime(year, month, 11);
            DateTime firstdatetwenty = new DateTime(year, month, 21);

            double paidthismonthtopaytotal = 0;
            int positioncount = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            if (positioncount != 0)
            {
                paidthismonthtopaytotal = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidIntoduceBonus + d.paidpositionBonus + d.paidPresentageBonus + d.paidFifthStageBonus + d.paidThirdStageBonus + d.paidshareBonus);
            }
            ViewBag.totoalbonuspastmonth = Convert.ToInt32(paidthismonthtopaytotal);

            //new 10/5 positionbonus
            double paidthismonthtopaytotal1 = 0.0;
            int positioncount1 = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstdate && d.paidBonusDateTime < firstdateten).Count();

            if (positioncount1 != 0)
            {
                paidthismonthtopaytotal1 = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstdate && d.paidBonusDateTime < firstdateten).Sum(d => d.paidpositionBonus);
            }
            ViewBag.totoalbonuspastmonth1 = Convert.ToInt32(paidthismonthtopaytotal1);
            //end

            //new 10/15 positionbonus
            double paidthismonthtopaytotal2 = 0.0;
            int positioncount2 = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstdateten && d.paidBonusDateTime < firstdatetwenty).Count();

            if (positioncount2 != 0)
            {
                paidthismonthtopaytotal2 = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstdateten && d.paidBonusDateTime < firstdatetwenty).Sum(d => d.paidpositionBonus);
            }
            ViewBag.totoalbonuspastmonth2 = Convert.ToInt32(paidthismonthtopaytotal2);
            //end

            //past month Profit Presentage Bonus
            int profitcount = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            double profitpresentage = 0.0;
            if (profitcount != 0)
            {
                profitpresentage = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidPresentageBonus);
            }
            ViewBag.profitpresentage1 = Convert.ToInt32(profitpresentage);
            //end

            //past month Profit position Bonus
            int positioncounting = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= pastmonthdate && d.paidBonusDateTime < lastday).Count();
            double positionbonusall = 0.0;
            if (positioncounting != 0)
            {
                positionbonusall = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= pastmonthdate && d.paidBonusDateTime < lastday).Sum(d => d.paidpositionBonus);
            }
            ViewBag.positionbonusall = Convert.ToInt32(positionbonusall);
            //end

            //past month share Bonus
            int sharpositioncount = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            double sharepositionbonus = 0.0;
            if (sharpositioncount != 0)
            {
                sharepositionbonus = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidshareBonus);
            }
            ViewBag.sharebonus = Convert.ToInt32(sharepositionbonus);
            //end

            //past month introduce Bonus
            int positioncounting1 = db.PaidBonuss.Where(d => d.memberId == membership && d.bcNumber == bcvalue1 && d.positionHistory == jumphistory && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            int positioncounting2 = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            double introducebonus = 0.0;
            if (positioncounting1 != 0 && bcvalue1 != -1)
            {
                introducebonus = db.PaidBonuss.Where(d => d.memberId == membership && d.bcNumber == bcvalue1 && d.positionHistory == jumphistory && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidIntoduceBonus);
            }
            else if (positioncounting2 != 0 && bcvalue1 == -1)
            {
                introducebonus = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidIntoduceBonus);
            }
            ViewBag.introducebonusall = Convert.ToInt32(introducebonus);
            //end

            //past month 5thstage Bonus
            int posistionfifthstage = db.PaidBonuss.Where(d => d.memberId == membership && d.bcNumber == bcvalue1 && d.positionHistory == jumphistory && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            int posistionfifthstage1 = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            double fifthstagebonus = 0.0;
            if (posistionfifthstage != 0 && bcvalue1 != -1)
            {
                fifthstagebonus = db.PaidBonuss.Where(d => d.memberId == membership && d.bcNumber == bcvalue1 && d.positionHistory == jumphistory && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidFifthStageBonus);
            }
            else if (posistionfifthstage1 != 0 && bcvalue1 == -1)
            {
                fifthstagebonus = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidFifthStageBonus);
            }
            ViewBag.fifthstagebonuses = Convert.ToInt32(fifthstagebonus);
            //end

            //past month 3rd stage Bonus
            int thirdstagecount = db.PaidBonuss.Where(d => d.memberId == membership && d.bcNumber == bcvalue1 && d.positionHistory == jumphistory && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            int thirdstagecounting1 = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            double thirdstagebonus = 0.0;
            if (thirdstagecount != 0 && bcvalue1 != -1)
            {
                thirdstagebonus = db.PaidBonuss.Where(d => d.memberId == membership && d.bcNumber == bcvalue1 && d.positionHistory == jumphistory && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidThirdStageBonus);
            }
            else if (thirdstagecounting1 != 0 && bcvalue1 == -1)
            {
                thirdstagebonus = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidThirdStageBonus);
            }
            ViewBag.thirdstagebonus = Convert.ToInt32(thirdstagebonus);
            //end



            //any totalbonus start
            int counttotalbonus = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            int counttotalbonus1 = db.PaidBonuss.Where(d => d.memberId == membership && d.bcNumber == bcvalue1 && d.positionHistory == jumphistory && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Count();
            double anytotalbonus = 0.0;
            if (counttotalbonus != 0 && bcvalue1 == -1)
            {
                anytotalbonus = db.PaidBonuss.Where(d => d.memberId == membership && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidIntoduceBonus + d.paidThirdStageBonus + d.paidFifthStageBonus);
            }
            else if (counttotalbonus1 != 0 && bcvalue1 != -1)
            {
                anytotalbonus = db.PaidBonuss.Where(d => d.memberId == membership && d.bcNumber == bcvalue1 && d.positionHistory == jumphistory && d.paidBonusDateTime >= firstallbonusdate && d.paidBonusDateTime < lastday).Sum(d => d.paidIntoduceBonus + d.paidThirdStageBonus + d.paidFifthStageBonus);
            }
            ViewBag.anytotal = Convert.ToInt32(anytotalbonus);
            //any totalbonus end

            //new User wise Bonus End


            string membersNo = Session["membershipNo"].ToString();
            //var coin = new StageOne();
            var coining = db.StageOnes.Where(d => d.membershipNo == membersNo).ToList();
            ViewBag.coining = coining;

            return View();
        }

        [HttpPost]
        public ActionResult Bonusview(string bcvalue1)
        {
            string phrase = bcvalue1;
            string[] words = phrase.Split(' ');
            string stage = words[0];
            int bcnumber = Convert.ToInt32(words[1]);
            string members = Session["membershipNo"].ToString();

            switch (stage)
            {
                case "one":
                    int stageonescount = db.StageOnes.Where(d => d.membershipNo == members && d.bcNo == bcnumber && d.jumpHistory == "1").Count();
                    if (stageonescount != 0)
                    {
                        var stageones = db.StageOnes.Where(d => d.membershipNo == members && d.bcNo == bcnumber && d.jumpHistory == "1").FirstOrDefault();
                        int bcs = stageones.bcNo;
                        int jumphistry = Convert.ToInt32(stageones.jumpHistory);

                        return RedirectToAction("Bonusview", "Trees", new { bcvalue1 = bcs, jumphistory = jumphistry });
                    }
                    break;
                case "two":
                    int stagetwocount = db.StageTwoes.Where(d => d.membershipNo == members && d.bcNo == bcnumber && d.jumpHistory == "2").Count();
                    if (stagetwocount != 0)
                    {
                        var stagetwo = db.StageTwoes.Where(d => d.membershipNo == members && d.bcNo == bcnumber && d.jumpHistory == "2").FirstOrDefault();
                        int bcs = stagetwo.bcNo;
                        int jumphistry = Convert.ToInt32(stagetwo.jumpHistory);
                        return RedirectToAction("Bonusview", "Trees", new { bcvalue1 = bcs, jumphistory = jumphistry });
                    }
                    break;
                case "three":
                    int stagethreecount = db.StageThrees.Where(d => d.membershipNo == members && d.bcNo == bcnumber && d.jumpHistory == "3").Count();
                    if (stagethreecount != 0)
                    {
                        var stagethree = db.StageThrees.Where(d => d.membershipNo == members && d.bcNo == bcnumber && d.jumpHistory == "3").FirstOrDefault();
                        int bcs = stagethree.bcNo;
                        int jumphistry = Convert.ToInt32(stagethree.jumpHistory);
                        return RedirectToAction("Bonusview", "Trees", new { bcvalue1 = bcs, jumphistory = jumphistry });
                    }
                    break;

                case "all":
                    return RedirectToAction("Bonusview", "Trees", new { bcvalue1 = -1, jumphistory = 0 });

                case "fifth":
                    return RedirectToAction("Bonusview", "Trees", new { bcvalue1 = 0, jumphistory = 5 });
            }
            return View();
        }

        //Cool OFF View
        public ActionResult ViewMyPositions()
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            string memberno = Session["membershipNo"].ToString();
            var getdata = db.PositionDetails.Where(a => a.membershipNo == memberno && a.positionStatus == "pending").OrderBy(m => DbFunctions.TruncateTime(m.registerDate)).ThenBy(c => c.positionPriority).ToList();
            ViewBag.maxbc = (int)rules.sendBcNo(memberno, 1);
            return View(getdata);
        }

        //Cool OFF View
        [HttpPost]
        public ActionResult ViewMyPositions(CoolingOff coolingOff)
        {
            int id, count;
            string[] pakcount = coolingOff.positionVal.Split(',');
            for (int aa = 0; aa < pakcount.Length; aa++)
            {
                string[] allpack = pakcount[aa].Split('-');
                id = Convert.ToInt32(allpack[0]);
                count = Convert.ToInt32(allpack[1]);
                PositionDetail pod = new PositionDetail();
                pod = db.PositionDetails.Find(id);
                pod.positionCount -= count;
                db.PositionDetails.Attach(pod);
                db.Entry(pod).State = EntityState.Modified;
                db.SaveChanges();

                coolingOff.coolDownDate = DateTime.Now;
                coolingOff.coolDownCount = count;
                coolingOff.positionVal = "cooling-off";
                coolingOff.positionDetailId = id;
                db.CoolingOffs.Add(coolingOff);
                db.SaveChanges();
            }
            return RedirectToAction("ViewMyPositions");
        }

        // GET: Trees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StageOne stageOne = db.StageOnes.Find(id);
            if (stageOne == null)
            {
                return HttpNotFound();
            }
            return View(stageOne);
        }

        // GET: Trees/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult createResult(List<int> arrayList)
        {
            for (int arrayListCount = 0; arrayListCount < arrayList.Count; arrayListCount++)
            {
                int conmem = Convert.ToInt32(arrayList[arrayListCount]);
                if (rules.paidvalidPositions(conmem) && rules.checkPositions(conmem))
                {
                    StageOne stageOne = new StageOne();
                    StageTwo stageTwo = new StageTwo();
                    StageThree stageThree = new StageThree();
                    StageFour stageFour = new StageFour();
                    StageFive stageFive = new StageFive();
                    PositionDetail tp = new PositionDetail();
                    tp = rules.getTempData(conmem);
                    string num = rules.getTempDataintu(conmem);
                    string num2 = num;
                    int pakstyle = 1;
                    int packagecount = tp.paymentStatus;
                    Session["packagemember"] = tp.membershipNo;
                    int aa;
                    int package = Convert.ToInt32(tp.package);
                    for ( aa= 0; aa < packagecount; aa++)
                    {

                        Package newpk = new Package();

                        tree1Pro(num, num2, stageOne, stageTwo, stageThree, stageFour, stageFive, package, pakstyle, 0, aa, conmem,0);

                        StageOne tree8 = new StageOne();
                        tree8 = rules.introduceposi(num);

                        //int bcno = rules.sendBcNo(tp.membershipNo);
                        if (package == 2)
                        {
                            var find2 = rules.findTreeId2(tree8.bcNo, tree8.membershipNo, 2);
                            insertTree2(stageTwo, 0, stageThree, stageFour, stageFive, find2, package, aa, pakstyle, "2");
                        }
                        if (package == 2)
                        {
                            var find3 = rules.findTreeId2(tree8.bcNo, tree8.membershipNo, 3);
                            insertTree3(0, stageThree, stageFour, stageFive, find3, package, aa, pakstyle, "3");
                        }
                        if (aa == 0 && pakstyle == 1)
                        {
                            num = rules.sendPromoCode(1, stageOne.membershipNo);
                        }
                       
                    }
                    PositionDetail positionDetail1 = new PositionDetail();
                    int pckid = Convert.ToInt32(arrayList[arrayListCount]);
                    positionDetail1 = db.PositionDetails.Find(pckid);
                    positionDetail1.systemUpdate = aa;
                    db.PositionDetails.Attach(positionDetail1);
                    db.Entry(positionDetail1).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }
            return RedirectToAction("Index");
            //return View();
        }
        // POST: StageOnes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StageOne stageOne, StageTwo stageTwo, StageThree stageThree, StageFour stageFour, StageFive stageFive)
        {
            //CheckCsvMemberData.csvMemberData();
            //if (ModelState.IsValid)
            //{
            //    // ViewBag
            //    string num = "E5Q517";
            //    int pakstyle = 2;
            //    for (int aa = 0; aa < 12; aa++)
            //    {
            //        int package = 0;
            //        Package newpk = new Package();
            //        while (true)
            //        {
            //            try
            //            {
            //                newpk = rules.packdata();
            //                package = newpk.packageId;
            //                break;
            //            }
            //            catch (Exception)
            //            {
            //                continue;
            //            }

            //        }
            //        tree1Pro(num, stageOne, stageTwo, stageThree, stageFour, stageFive, package, pakstyle, 0, aa, conmem);

            //        StageOne tree8 = new StageOne();
            //        tree8 = rules.introduceposi(num);

            //        int bcno = rules.sendBcNo("000002");
            //        if (newpk.stage == 2 || newpk.stage == 3)
            //        {
            //            var find2 = rules.findTreeId2(tree8.bcNo, tree8.membershipNo, 2);
            //            insertTree2(stageTwo, bcno - 1, stageThree, stageFour, stageFive, find2, package, aa, pakstyle);
            //        }
            //        if (newpk.stage == 3)
            //        {
            //            var find3 = rules.findTreeId2(tree8.bcNo, tree8.membershipNo, 3);
            //            insertTree3(bcno - 1, stageThree, stageFour, stageFive, find3, package, aa, pakstyle);
            //        }
            //        if (aa == 0 && pakstyle == 1)
            //        {
            //            num = rules.sendPromoCode(1, stageOne.treeId);
            //        }
            //    }
            //    return RedirectToAction("StageOne");
            //}

            return View(stageOne);
        }

        int packbc = 0;
        public int tree1Pro(string num, string num2, StageOne stageOne, StageTwo stageTwo, StageThree stageThree, StageFour stageFour, StageFive stageFive, int package, int pakstyle, int freestages, int aa, int tempid, int freel)
        {
            StageOne tree4 = new StageOne();
            tree4 = rules.introduceposi(num);
            
           
            if (freestages > 0)
            {
                stageOne.membershipNo = tree4.membershipNo;
            }
            else
            {
                stageOne.membershipNo = Session["packagemember"].ToString();
            }
            stageOne.bcNo = (int)rules.sendBcNo(stageOne.membershipNo, 1) + 1;
            if (freestages == 0)
            {
                rules.subFree(tree4.treeLevel, tree4.treeColumn, 1);
                stageOne.treeLevel = (int)ReturnStage.level;
                stageOne.treeColumn = (long)ReturnStage.column;
                packbc = stageOne.bcNo;
            }
            else
            {
                rules.subFree(tree4.treeLevel, tree4.treeColumn, 1);
                stageOne.treeLevel = (int)ReturnStage.level;
                stageOne.treeColumn = (long)ReturnStage.column;
            }
            
            stageOne.introducePromoCode = num;
            stageOne.entryDate = DateTime.Now;
            stageOne.freeStatus = freestages;
            stageOne.jumpHistory = "1";
            if (freestages != 0)
            {
                stageOne.positionCode = rules.sendCoundResult(stageOne.treeLevel, stageOne.treeColumn, stageOne.membershipNo, stageOne.bcNo, stageOne.entryDate, freestages, 0, 0);
                stageOne.freeLinks = freel;
            }
            else
            {
                stageOne.positionCode = rules.sendCoundResult(stageOne.treeLevel, stageOne.treeColumn, stageOne.membershipNo, stageOne.bcNo, stageOne.entryDate, freestages, aa, tempid);
            }
            stageOne.package = package;

            long jpos = (long)rules.jumpingtree(stageOne.treeLevel, stageOne.treeColumn, 1);
            db.StageOnes.Add(stageOne);
            db.SaveChanges();
            if (jpos != 0)
            {
                package = rules.findPackage(jpos, (stageOne.treeLevel - 2), 1);
                tree2Pro(jpos, stageOne, stageTwo, stageThree, stageFour, stageFive, package, "1");
            }
            return stageOne.treeId;
        }
        public void tree2Pro(long jpos, StageOne stageOne, StageTwo stageTwo, StageThree stageThree, StageFour stageFour, StageFive stageFive, int package, string history)
        {
            var getdatas = rules.findTreeId((stageOne.treeLevel - 2), jpos, 1);
            StageOne tree2 = new StageOne();
            tree2 = db.StageOnes.Find(getdatas.treeId);
            tree2.jump = 2;

            db.StageOnes.Attach(tree2);
            db.Entry(tree2).State = EntityState.Modified;
            db.SaveChanges();
            StageOne tree3 = new StageOne();

            rules.HavenextTable((stageOne.treeLevel - 2), jpos, 1);
            rules.subFree((int)ReturnStage.level, ReturnStage.column, 2);
            stageTwo.membershipNo = rules.sendDataTree(jpos, (stageOne.treeLevel - 2), 1).membershipNo;
            stageTwo.treeLevel = (int)rules.returnlevel();
            stageTwo.treeColumn = (long)rules.returncolumn();
            stageTwo.bcNo = getdatas.bcNo;
            stageTwo.entryDate = DateTime.Now;
            stageTwo.previousPosition = getdatas.treeId;
            stageTwo.package = package;
            stageTwo.jumpHistory = getdatas.jumpHistory;
            long jpos2 = (long)rules.jumpingtree(stageTwo.treeLevel, stageTwo.treeColumn, 2);
            db.StageTwoes.Add(stageTwo);
            db.SaveChanges();

            if (jpos2 != 0)
            {
                package = rules.findPackage(jpos2, (stageTwo.treeLevel - 2), 2);
                var treeposhis = db.StageTwoes.Where(d => d.treeLevel == (stageTwo.treeLevel - 2) && d.treeColumn == jpos2).First();
                tree3Pro(jpos2, stageTwo, stageThree, stageFour, stageFive, package, treeposhis.jumpHistory);
                try
                {
                    var getdatajump = db.StageTwoes.Where(d => d.treeColumn == jpos2 && d.treeLevel == (stageTwo.treeLevel - 2)).First();//find jumping postion stagetwo id
                    int idt = getdatajump.previousPosition;
                    StageOne treeone = new StageOne();
                    treeone = db.StageOnes.Find(idt);
                    treeone.jump = 3;//update tree juping to stageOne

                    db.StageOnes.Attach(treeone);
                    db.Entry(treeone).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }
        public void tree3Pro(long jpos2, StageTwo stageTwo, StageThree stageThree, StageFour stageFour, StageFive stageFive, int package, string history)
        {
            var getdatas2 = rules.findTreeId((stageTwo.treeLevel - 2), jpos2, 2);
            StageTwo tree5 = new StageTwo();
            tree5 = db.StageTwoes.Find(getdatas2.treeId);
            tree5.jump = 3;
            db.StageTwoes.Attach(tree5);
            db.Entry(tree5).State = EntityState.Modified;
            db.SaveChanges();

            //tree3 = rules.underJumping((tree.level - 2), jpos);

            rules.HavenextTable((stageTwo.treeLevel - 2), jpos2, 2);
            rules.subFree((int)ReturnStage.level, ReturnStage.column, 3);
            stageThree.membershipNo = rules.sendDataTree(jpos2, (stageTwo.treeLevel - 2), 2).membershipNo;
            stageThree.treeLevel = (int)rules.returnlevel();
            stageThree.treeColumn = (long)rules.returncolumn();
            stageThree.bcNo = getdatas2.bcNo;
            stageThree.entryDate = DateTime.Now;
            stageThree.previousPosition = getdatas2.treeId;
            stageThree.package = package;
            stageThree.jumpHistory = getdatas2.jumpHistory;
            long jpos3 = (long)rules.jumpingtree(stageThree.treeLevel, stageThree.treeColumn, 3);
            db.StageThrees.Add(stageThree);
            db.SaveChanges();
            if (jpos3 != 0 && (stageThree.treeLevel - 2) != 0)
            {
                package = rules.findPackage(jpos3, (stageThree.treeLevel - 2), 3);
                var treeposhis = db.StageThrees.Where(d => d.treeLevel == (stageThree.treeLevel - 2) && d.treeColumn == jpos3).First();

                tree4Pro(jpos3, stageThree, stageFour, stageFive, package, treeposhis.jumpHistory);
                try
                {
                    var getdatajump = db.StageThrees.Where(d => d.treeColumn == jpos3 && d.treeLevel == (stageThree.treeLevel - 2)).First();//find jumping postion stagetwo id
                    int idt = getdatajump.previousPosition;
                    StageTwo treetwo = new StageTwo();
                    treetwo = db.StageTwoes.Find(idt);
                    treetwo.jump = 4;//update tree juping to stageTwo

                    db.StageTwoes.Attach(treetwo);
                    db.Entry(treetwo).State = EntityState.Modified;
                    db.SaveChanges();

                    var getdatajumpone = db.StageTwoes.Where(d => d.treeId == idt).First();//find jumping postion stagetwo id
                    int idto = getdatajumpone.previousPosition;
                    StageOne treeone = new StageOne();
                    treeone = db.StageOnes.Find(idto);
                    treeone.jump = 4;//update tree juping to stageOne

                    db.StageOnes.Attach(treeone);
                    db.Entry(treeone).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {

                }


            }
            long freeposi = (long)rules.freePosition(stageThree.treeColumn, stageThree.treeLevel, 3);
            if (freeposi != 0)
            {
                var gedfreedata = db.StageThrees.Where(d => d.treeColumn == freeposi && d.treeLevel == (stageThree.treeLevel - 3)).FirstOrDefault();
                if (rules.checkFreePosition(gedfreedata.membershipNo, gedfreedata.bcNo, gedfreedata.package, 3, gedfreedata.jumpHistory))
                {

                    string promo = rules.sendCodeFromPosi(stageThree.treeLevel - 3, freeposi, 3);
                    int trid = rules.sendResonId(stageThree.treeLevel - 3, freeposi, 3);
                    Create2(promo, 3, trid);
                }

            }
        }
        public void tree4Pro(long jpos3, StageThree stageThree, StageFour stageFour, StageFive stageFive, int package, string history)
        {

            var getdatas3 = rules.findTreeId((stageThree.treeLevel - 2), jpos3, 3);
            StageThree tree6 = new StageThree();
            tree6 = db.StageThrees.Find(getdatas3.treeId);
            tree6.jump = 4;
            db.StageThrees.Attach(tree6);
            db.Entry(tree6).State = EntityState.Modified;
            db.SaveChanges();

            rules.HavenextTable((stageThree.treeLevel - 2), jpos3, 3);
            rules.subFree((int)ReturnStage.level, ReturnStage.column, 4);
            stageFour.membershipNo = rules.sendDataTree(jpos3, (stageThree.treeLevel - 2), 3).membershipNo;
            stageFour.treeLevel = (int)rules.returnlevel();
            stageFour.treeColumn = (long)rules.returncolumn();
            stageFour.bcNo = getdatas3.bcNo;
            stageFour.entryDate = DateTime.Now;
            stageFour.previousPosition = getdatas3.treeId;
            stageFour.package = package;
            stageFour.jumpHistory = getdatas3.jumpHistory;
            stageFour.jump = 4;

            long jpos4 = (long)rules.jumpingtree(stageFour.treeLevel, stageFour.treeColumn, 4);
            db.StageFours.Add(stageFour);
            db.SaveChanges();
            if (jpos4 != 0)
            {
                package = rules.findPackage(jpos4, (stageFour.treeLevel - 2), 4);
                var treeposhis = db.StageFours.Where(d => d.treeLevel == (stageFour.treeLevel - 2) && d.treeColumn == jpos4).First();
                tree5Pro(jpos4, stageFour, stageFive, package, treeposhis.jumpHistory);
                try
                {
                    var getdatajumptree = db.StageFours.Where(d => d.treeColumn == jpos4 && d.treeLevel == (stageFour.treeLevel - 2)).First();//find jumping postion stagetwo id
                    int idt3 = getdatajumptree.previousPosition;
                    StageThree treetree = new StageThree();
                    treetree = db.StageThrees.Find(idt3);
                    treetree.jump = 5;//update tree juping to stageTwo

                    db.StageThrees.Attach(treetree);
                    db.Entry(treetree).State = EntityState.Modified;
                    db.SaveChanges();

                    var getdatajump = db.StageThrees.Where(d => d.treeId == idt3).First();//find jumping postion stagetwo id
                    int idt = getdatajump.previousPosition;
                    StageTwo treetwo = new StageTwo();
                    treetwo = db.StageTwoes.Find(idt);
                    treetwo.jump = 5;//update tree juping to stageTwo

                    db.StageTwoes.Attach(treetwo);
                    db.Entry(treetwo).State = EntityState.Modified;
                    db.SaveChanges();

                    var getdatajumpone = db.StageTwoes.Where(d => d.treeId == idt).First();//find jumping postion stagetwo id
                    int idto = getdatajumpone.previousPosition;
                    StageOne treeone = new StageOne();
                    treeone = db.StageOnes.Find(idto);
                    treeone.jump = 5;//update tree juping to stageOne

                    db.StageOnes.Attach(treeone);
                    db.Entry(treeone).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }
        public void tree5Pro(long jpos4, StageFour stageFour, StageFive stageFive, int package, string history)
        {
            var getdatas4 = rules.findTreeId((stageFour.treeLevel - 2), jpos4, 4);
            StageFour tree7 = new StageFour();
            tree7 = db.StageFours.Find(getdatas4.treeId);
            tree7.jump = 5;
            db.StageFours.Attach(tree7);
            db.Entry(tree7).State = EntityState.Modified;
            db.SaveChanges();

            rules.HavenextTable((stageFour.treeLevel - 2), jpos4, 4);
            rules.subFree((int)ReturnStage.level, ReturnStage.column, 5);
            stageFive.membershipNo = rules.sendDataTree(jpos4, (stageFour.treeLevel - 2), 4).membershipNo;
            stageFive.treeLevel = (int)rules.returnlevel();
            stageFive.treeColumn = (long)rules.returncolumn();
            stageFive.bcNo = getdatas4.bcNo;
            stageFive.entryDate = DateTime.Now;
            stageFive.previousPosition = getdatas4.treeId;
            stageFive.package = package;
            stageFive.jumpHistory = history;
            db.StageFives.Add(stageFive);
            db.SaveChanges();
            long freeposi = (long)rules.freePosition(stageFive.treeColumn, stageFive.treeLevel, 5);
            if (freeposi != 0)
            {
                var gedfreedata = db.StageFives.Where(d => d.treeColumn == freeposi && d.treeLevel == (stageFive.treeLevel - 3)).FirstOrDefault();
                if (rules.checkFreePosition(gedfreedata.membershipNo, gedfreedata.bcNo, gedfreedata.package, 3, gedfreedata.jumpHistory))
                {

                    string promo = rules.sendCodeFromPosi(stageFive.treeLevel - 3, freeposi, 5);
                    int trid = rules.sendResonId(stageFive.treeLevel - 3, freeposi, 5);
                    Create2(promo, 5, trid);
                }


            }
        }

        public void insertTree2(StageTwo stageTwo, int bcno, StageThree stageThree, StageFour stageFour, StageFive stageFive, StageTwo find2, int package, int aa, int pakstyle, string history)
        {
            stageTwo.membershipNo = Session["packagemember"].ToString();
            rules.subFree(find2.treeLevel, find2.treeColumn, 2);
            stageTwo.treeLevel = (int)rules.returnlevel();
            stageTwo.treeColumn = (long)rules.returncolumn();
            stageTwo.bcNo = packbc;
            stageTwo.entryDate = DateTime.Now;
            stageTwo.previousPosition = 0;
            stageTwo.package = package;
            stageTwo.jumpHistory = history;
            long jpos2 = (long)rules.jumpingtree(stageTwo.treeLevel, stageTwo.treeColumn, 2);
            db.StageTwoes.Add(stageTwo);
            db.SaveChanges();
            if (jpos2 != 0)
            {
                package = rules.findPackage(jpos2, (stageTwo.treeLevel - 2), 2);
                tree3Pro(jpos2, stageTwo, stageThree, stageFour, stageFive, package, "2");
            }
        }
        public void insertTree3(int bcno, StageThree stageThree, StageFour stageFour, StageFive stageFive, StageThree find3, int package, int aa, int pakstyle, string history)
        {
            stageThree.membershipNo = Session["packagemember"].ToString();

            rules.subFree(find3.treeLevel, find3.treeColumn, 3);
            stageThree.treeLevel = (int)rules.returnlevel();
            stageThree.treeColumn = (long)rules.returncolumn();
            stageThree.bcNo = packbc;
            stageThree.entryDate = DateTime.Now;
            stageThree.previousPosition = 0;
            stageThree.package = package;
            stageThree.jumpHistory = history;
            long jpos3 = (long)rules.jumpingtree(stageThree.treeLevel, stageThree.treeColumn, 3);
            db.StageThrees.Add(stageThree);
            db.SaveChanges();
            if (jpos3 != 0)
            {
                package = rules.findPackage(jpos3, (stageThree.treeLevel - 2), 3);
                tree4Pro(jpos3, stageThree, stageFour, stageFive, package, "3");
            }
            ///// this part is for testing..remove from original   //////////////////////////////////////////////
            long freeposi = (long)rules.freePosition(stageThree.treeColumn, stageThree.treeLevel, 3);
            if (freeposi != 0)
            {
                var gedfreedata = db.StageThrees.Where(d => d.treeColumn == freeposi && d.treeLevel == (stageThree.treeLevel - 3)).FirstOrDefault();
                if (rules.checkFreePosition(gedfreedata.membershipNo, gedfreedata.bcNo, gedfreedata.package, 3, gedfreedata.jumpHistory))
                {

                    string promo = rules.sendCodeFromPosi(stageThree.treeLevel - 3, freeposi, 3);
                    int trid = rules.sendResonId(stageThree.treeLevel - 3, freeposi, 3);
                    Create2(promo, 3, trid);
                }

            }
            /////////////////////////////////////////////////////////////////////////////////////////////
        }

        public dynamic Create2(string promo, int round, int trid)
        {
            string num = promo;
            string num2 = promo;
            int pakstyle = 2;
            StageOne tree = new StageOne();
            StageTwo tree2s = new StageTwo();
            StageThree tree3s = new StageThree();
            StageFour tree4s = new StageFour();
            StageFive tree5s = new StageFive();
            for (int aa = 0; aa < round; aa++)
            {
                int package = 0;
                Package newpk = new Package();
                while (true)
                {
                    try
                    {
                        newpk = rules.packdata();
                        package = newpk.packageId;
                        break;
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
                int getid=tree1Pro(num, num2, tree, tree2s, tree3s, tree4s, tree5s, 1, pakstyle, round, aa, 0,trid);
                StageOne tree6 = new StageOne();
                tree6 = db.StageOnes.Find(getid);
                tree6.freeLinks = trid;
                db.StageOnes.Attach(tree6);
                db.Entry(tree6).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult deletePosition(int id)
        {
            string memberid = Session["membershipNo"].ToString();

            var positionsd = db.PositionDetails.Where(d => d.positionId == id && d.membershipNo == memberid).FirstOrDefault();
            if (positionsd != null)
            {
                PositionDetail pod = new PositionDetail();
                pod = db.PositionDetails.Find(id);
                pod.positionStatus = "Canceled";
                db.PositionDetails.Attach(pod);
                db.Entry(pod).State = EntityState.Modified;
                db.SaveChanges();
                return Json(positionsd, JsonRequestBehavior.AllowGet);

            }
            return View();

        }

        // POST: Trees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "treeId,membershipNo,bcNo,treeLevel,treeColumn,jump,positionCode,introducePromoCode,entryDate,package,freeStatus,jumpHistory")] StageOne stageOne)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stageOne).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stageOne);
        }

        // GET: Trees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StageOne stageOne = db.StageOnes.Find(id);
            if (stageOne == null)
            {
                return HttpNotFound();
            }
            return View(stageOne);
        }

        // POST: Trees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StageOne stageOne = db.StageOnes.Find(id);
            db.StageOnes.Remove(stageOne);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult TreeTable()
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            Session["StageSession"] = "false";
            string membernumber = Session["membershipNo"].ToString();
            List<StageOne> stageOnesList = new List<StageOne>();
            stageOnesList = TableViewRules.sendDataBundleOne(1, Session["membershipNo"].ToString());
            int onelakamountcoin = db.BonusDetails.Where(d => d.bonusId == 23).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();
            int threelakamountcoin = db.BonusDetails.Where(d => d.bonusId == 24).OrderByDescending(d => d.bonusDetailId).Select(d => d.bonusAmount).FirstOrDefault();

            ViewBag.onelakh = onelakamountcoin;
            ViewBag.threelakh = threelakamountcoin;
            long apicoinmember = Task.Run(() => BonusCalculation.numberofcoin(membernumber)).Result;
            ViewBag.apicoin = apicoinmember;
            ViewBag.stageOnesList = stageOnesList;
            return View();
        }

        public ActionResult UserProfile()
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            Session["StageSession"] = "false";
            var membershipNo = Session["membershipNo"].ToString();
            ViewBag.UserProfileList = db.Users.Where(d => d.membershipNo == membershipNo).ToList();
            return View();
        }

        public ActionResult Rules()
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("../Users/Login");
            }
            Session["StageSession"] = "false";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
