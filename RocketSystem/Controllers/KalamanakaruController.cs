using Newtonsoft.Json;
using RocketSystem.Classes;
using RocketSystem.DbLink;
using RocketSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RocketSystem.Controllers
{
    public class KalamanakaruController : Controller
    {
        private DataAccessLayer db = new DataAccessLayer();

        // GET: Kalamanakaru
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult indexAdmin()
        {
            if (Session["adminId"] != null)
            {
                AdminBonusCalculation bc = new AdminBonusCalculation();
                var paidbonusviews = bc.paidpresentagebonus();
                ViewBag.list2 = paidbonusviews;

                var positiondetailss = bc.postionDetails();
                ViewBag.list3 = positiondetailss;

                var profitpastmonth = bc.profitpastmonth();
                ViewBag.list4 = profitpastmonth;

                var datewisepos = bc.datewiseposition();
                ViewBag.list5 = datewisepos;

                var topmember = bc.topmemberbonus();
                ViewBag.list6 = topmember;

                var onlytopmemberlist = bc.onlytopmemberbonus();
                ViewBag.list7 = onlytopmemberlist;

                var onlymiddlememberlist = bc.onlymiddlememberbonus();
                ViewBag.list8 = onlymiddlememberlist;

                var onlysmallememberlist = bc.onlysmallmemberbonus();
                ViewBag.list9 = onlysmallememberlist;

                var paidbonusvalue = bc.paidBonus();
                ViewBag.list10 = paidbonusvalue;

                var yearlybonus = bc.yearlyBonus();
                ViewBag.list11 = yearlybonus;

                var expectedbonus = bc.expecterIncome();
                ViewBag.list12 = expectedbonus;

                var introducerall = bc.introducepositionlist();
                ViewBag.list13 = introducerall;

                var thirtifiveintroducer = bc.toppackageintroducepositionlist();
                ViewBag.list14 = thirtifiveintroducer;

                var midlleintroducer = bc.middlepackageintroducepositionlist();
                ViewBag.list15 = midlleintroducer;

                var freeposition = bc.freepositionode();
                ViewBag.list16 = freeposition;

                var freepositionbonus = bc.freepositionbonus();
                ViewBag.list17 = freepositionbonus;

                var introducepersoncount = bc.membersprestage();
                ViewBag.list18 = introducepersoncount;

                var memberspresentvalue = bc.memberamountofpresentagebonus();
                ViewBag.list19 = memberspresentvalue;

                var freepositonbonus = bc.freepositontotalbonus();
                ViewBag.list20 = freepositonbonus;

                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult Login()
        {
            if (Session["adminId"] != null)
            {
                return RedirectToAction("../kalamanakaru/indexAdmin");
            }
            return View();

        }
        public ActionResult logout()
        {
            Session["adminId"] = null;
            return RedirectToAction("../kalamanakaru/login");
        }
        [HttpPost]
        public ActionResult Login(Kalamanakaru kalamanakaru)
        {
            string createpwd = SHA.GenerateSHA256String(kalamanakaru.userName + kalamanakaru.password);
            var check = db.kalamanakarus.Where(d => d.userName == kalamanakaru.userName && d.password == createpwd).FirstOrDefault();
            if (check != null)
            {
                Session["adminId"] = check.adminId;
                Session["username"] = kalamanakaru.userName;
                Session["adminName"] = check.adminName;
                return RedirectToAction("../kalamanakaru/indexAdmin");
            }
            else
            {

            }
            return View();

        }

        public ActionResult userdetails()
        {
            if (Session["adminId"] != null)
                return View();
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult rejectuser()
        {
            if (Session["adminId"] != null)
                return View();
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult bonusviewadmin()
        {
            ViewBag.listbonus = db.PaidBonuss.ToList();
            List<double> paidbonus = new List<double>();
            List<int> profit = new List<int>();
            int b = db.PaidBonuss.Count();
            double paidintroducebonus = 0;
            double paidpositionbonus = 0;
            double paidpresentagebonus = 0;
            double paid3rdstagebonus = 0;
            double paid5thstagebonus = 0;
            double paidsharebonus = 0;

            if (b != 0)
            {
                paidintroducebonus = db.PaidBonuss.Sum(d => d.paidIntoduceBonus);
                paidpositionbonus = db.PaidBonuss.Sum(d => d.paidpositionBonus);
                paidpresentagebonus = db.PaidBonuss.Sum(d => d.paidPresentageBonus);
                paid3rdstagebonus = db.PaidBonuss.Sum(d => d.paidThirdStageBonus);
                paid5thstagebonus = db.PaidBonuss.Sum(d => d.paidFifthStageBonus);
                paidsharebonus = db.PaidBonuss.Sum(d => d.paidshareBonus);
            }
            paidbonus.Add(paidintroducebonus);
            paidbonus.Add(paidpositionbonus);
            paidbonus.Add(paidpresentagebonus);
            paidbonus.Add(paid3rdstagebonus);
            paidbonus.Add(paid5thstagebonus);
            paidbonus.Add(paidsharebonus);

            ViewBag.paidbonuschart = paidbonus;
            int allpaidbonus = Convert.ToInt32(paidintroducebonus + paidpositionbonus + paidpresentagebonus + paid3rdstagebonus + paid5thstagebonus + paidsharebonus);
            if (allpaidbonus == 0) { allpaidbonus++; }
            int toppositionprice = db.StageOnes.Where(d => d.package == 1).Count() * 100000;
            int middlepositionprice = db.StageOnes.Where(d => d.package == 2).Count() * 350000;
            int smallpositionprice = db.StageOnes.Where(d => d.package == 1).Count() * 10000;
            int allprice = toppositionprice + middlepositionprice + smallpositionprice;

            int profits = allprice - allpaidbonus;
            profit.Add(allprice);
            profit.Add(allpaidbonus);
            profit.Add(profits);

            ViewBag.profitchart = profit;

            return View();
        }

        [HttpPost]
        public ActionResult bonusviewadmin(int? bcvalue1, int? bcvalue2)
        {

            int datahave = db.PaidBonuss.Count();
            int x = 1;
            if (datahave != 0)
            {
                x = db.PaidBonuss.Sum(d => d.paidBonusId);
            }
            int y = ((int)Math.Pow(x, 45) / 2);
            int z = Math.Abs(y * 2 + 5555555);
            int k = (z * 400) / 3;

            DateTime dating = DateTime.Now.Date;
            int month = dating.Month;
            int year = dating.Year;
            DateTime thismonth = new DateTime(year, month, 18).Date;
            DateTime paidmontdate = new DateTime(year, month, 30);
            DateTime paidmontdate1 = new DateTime(year, month, 10);
            DateTime paidmontdate2 = new DateTime(year, month, 20);
            int paidbonuscount = db.PaidBonuss.Where(d => d.paidBonusDateTime == paidmontdate).Count();

            if (z == bcvalue1 && thismonth == dating && bcvalue2 == null)
            {
                if (paidbonuscount == 0)
                {
                    BonusCalculation dyx = new BonusCalculation();
                    var calcultebonus = dyx.CalallbonusAdmin();
                    db.PaidBonuss.AddRange(calcultebonus);
                    db.SaveChanges();
                    var calculatepresentage = dyx.Presentagebonusadmin();
                    using (DataAccessLayer db = new DataAccessLayer())
                    {
                        foreach (var calculate in calculatepresentage)
                        {
                            var entry = db.Entry(calculate);
                            entry.State = EntityState.Modified;

                            db.SaveChanges();
                        }
                        
                    }
                    using (DataAccessLayer db=new DataAccessLayer())
                    {
                        var updateshareandprofitbonus = dyx.sharebonusandprofitpresentagenew();
                        foreach (var updateshare in updateshareandprofitbonus)
                        {
                            var entry1 = db.Entry(updateshare);
                            entry1.State = EntityState.Modified;

                            db.SaveChanges();
                        }
                    }
                    
                }
            }
            else if (bcvalue2 == k)
            {
                int countfirstcart = 0;
                if (bcvalue1 == 1)
                {
                    countfirstcart = db.PaidBonuss.Where(d => d.paidBonusDateTime == paidmontdate1).Count();
                }
                if (bcvalue1 == 2)
                {
                    countfirstcart = db.PaidBonuss.Where(d => d.paidBonusDateTime == paidmontdate2).Count();
                }
                if (countfirstcart == 0)
                {
                    BonusCalculation oneclases = new BonusCalculation();
                    int a = Convert.ToInt32(bcvalue1);
                    var bonusposition = oneclases.calculatepositionbonusall(a);
                    db.PaidBonuss.AddRange(bonusposition);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("bonusviewadmin");
        }

        public ActionResult companyprofit()
        {
            if (Session["adminId"] != null)
            {
                AdminBonusCalculation external = new AdminBonusCalculation();
                var customerbonus = external.coustomerwiseprofit();
                ViewBag.customerbonus = customerbonus;
            }
            else
                return RedirectToAction("../kalamanakaru/login");
            
            return View();
        }

        public ActionResult memberaction()
        {
            if (Session["adminId"] != null)
                return View();
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult csvdata()
        {
            if (Session["adminId"] != null)
            {
                var model = new ExcelDataModel();
                return View(model);
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        [HttpPost]
        public ActionResult csvdata(ExcelDataModel Excelmodel, User user)
        {
            if (user.name == "export")
            {
                FileStream fs = new FileStream(@"C:\csv\CsvData.txt", FileMode.CreateNew);
                StreamWriter streamWriter = new StreamWriter(fs, Encoding.GetEncoding(932));// creating csvData text file
                GenerateCsvFile.generateCsvHeader(streamWriter); // making the header of the csv file
                string startdate = DateTime.Now.ToString("dd MM yyyy hh:mm:ss tt");
                double totalAmount = 0;
                var paidBonus = db.PaidBonuss.ToList();//put where and get the paid bonus list for a month after changing paidbonusdatetime from string to datetime
                foreach (var value in paidBonus)//getting the current month user list to pay for (list of user to put in the csv file)
                {
                    using (DataAccessLayer db = new DataAccessLayer())
                    {
                        var users = db.Users.Where(x => x.membershipNo == value.memberId).ToList();//getting that member data from registration
                        totalAmount = totalAmount + GenerateCsvFile.generateCsvBody(streamWriter, users, value);
                    }
                }
                GenerateCsvFile.generateCsvFooter(streamWriter, paidBonus.Count, totalAmount.ToString());
                streamWriter.Close();
            }
            else
            {
                try
                {
                    if (Excelmodel.MyExcelFile == null)
                    {
                        return View(Excelmodel);
                    }
                    DataTable dt = ExcelDataHandling.GetDataTableFromSpreadsheet(Excelmodel.MyExcelFile.InputStream, false);
                    ExcelDataHandling.InsertDataToDatabase(dt);
                    string strContent = "<p>Excel File Successfully Uploaded</p>";
                    Excelmodel.MSExcelTable = strContent;
                }
                catch
                {
                    string strContent = "<p>Error While Uploading File</p>";
                    Excelmodel.MSExcelTable = strContent;
                }
            }
            return View("csvdata", Excelmodel);
        }

        public ActionResult viewUser()
        {
            if (Session["adminId"] != null)
                return View(db.UserLogins.ToList());
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        //View more Page 1
        public ActionResult Viewmore1()
        {
            return View();
        }

        //move to kalamanakaru
        public ActionResult loginAs(int? id)
        {
            if (Session["adminId"] != null)
            {
                var loginUser = db.UserLogins.Where(l => l.uID == id).First();
                var user = db.Users.Where(x => x.userName == loginUser.userName).FirstOrDefault();
                Session["MembershipNo"] = loginUser.membershipNo.ToString();
                Session["memberName"] = user.name.ToString();
                Session["username"] = loginUser.userName.ToString();
                int coin = db.StageOnes.Where(d => d.membershipNo == loginUser.membershipNo.ToString() && d.package == 1).Count() * 5000 + db.StageOnes.Where(d => d.membershipNo == loginUser.membershipNo.ToString() && d.package == 2).Count() * 17500;
                Session["coin"] = coin;
                return RedirectToAction("Index", "Trees");
            }
            else
                return RedirectToAction("../kalamanakaru/login");

        }
        // move to kalamanakaru
        [HttpPost]
        public ActionResult userDetails(int id)
        {
            if (Session["adminId"] != null)
            {
                var userDetail = db.Users.Where(d => d.userId == id).ToList();
                return Json(userDetail, JsonRequestBehavior.AllowGet);
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }
        // move to kalamanakaru
        public ActionResult userSuspend(int id)
        {
            if (Session["adminId"] != null)
            {
                UserLogin user = new UserLogin();
                user = db.UserLogins.Find(id);
                user.status = 1;
                db.UserLogins.Attach(user);
                db.Entry(user).State = EntityState.Modified;
                int result = db.SaveChanges();
                if (result == 1)
                {
                    UserSuspendHistory userSuspendHistory = new UserSuspendHistory();
                    userSuspendHistory.adminId = Convert.ToInt32(Session["adminId"]);
                    userSuspendHistory.userId = id;
                    userSuspendHistory.status = 1;
                    userSuspendHistory.dateHistory = DateTime.Now;
                    db.UserSuspendHistorys.Add(userSuspendHistory);
                    db.SaveChanges();
                    return RedirectToAction("viewUser", "Kalamanakaru");
                }
                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        // move to kalamanakaru
        public ActionResult userUnSuspend(int id)
        {
            if (Session["adminId"] != null)
            {
                UserLogin user = new UserLogin();
                user = db.UserLogins.Find(id);
                user.status = 0;
                db.UserLogins.Attach(user);
                db.Entry(user).State = EntityState.Modified;
                int result = db.SaveChanges();
                if (result == 1)
                {
                    UserSuspendHistory userSuspendHistory = new UserSuspendHistory();
                    userSuspendHistory.adminId = Convert.ToInt32(Session["adminId"]);
                    userSuspendHistory.userId = id;
                    userSuspendHistory.status = 0;
                    userSuspendHistory.dateHistory = DateTime.Now;
                    db.UserSuspendHistorys.Add(userSuspendHistory);
                    db.SaveChanges();
                    return RedirectToAction("viewUser", "Kalamanakaru");
                }
                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult temporaryposition()
        {
            if (Session["adminId"] != null)
            {
                int year = DateTime.Now.Year;
                int thatmonth = DateTime.Now.Month;
                int yearnext = DateTime.Now.Year;
                if (thatmonth == 12)
                {
                    yearnext = DateTime.Now.AddYears(1).Year;
                }
                int nextmonth = DateTime.Now.AddMonths(1).Month;

                DateTime firstDay = new DateTime(year, thatmonth, 1);
                DateTime lastDay = new DateTime(yearnext, nextmonth, 1);
                var positiondetails = db.PositionDetails.Where(d => d.registerDate >= firstDay && d.registerDate < lastDay).ToList();
                ViewBag.list1 = positiondetails;
                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult allpaid()
        {
            if (Session["adminId"] != null)
            {
                int year = DateTime.Now.Year;
                int thatmonth = DateTime.Now.Month;
                int yearnext = DateTime.Now.Year;
                if (thatmonth == 12)
                {
                    yearnext = DateTime.Now.AddYears(1).Year;
                }
                int nextmonth = DateTime.Now.AddMonths(1).Month;

                DateTime firstDay = new DateTime(year, thatmonth, 1);
                DateTime lastDay = new DateTime(yearnext, nextmonth, 1);

                var positiondetails = db.PositionDetails.Where(d => d.paymentStatus == d.positionCount && d.registerDate >= firstDay && d.registerDate < lastDay).ToList();
                ViewBag.list1 = positiondetails;
                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult systemupdate1()
        {
            if (Session["adminId"] != null)
            {
                int year = DateTime.Now.Year;
                int thatmonth = DateTime.Now.Month;
                int yearnext = DateTime.Now.Year;
                if (thatmonth == 12)
                {
                    yearnext = DateTime.Now.AddYears(1).Year;
                }
                int nextmonth = DateTime.Now.AddMonths(1).Month;

                DateTime firstDay = new DateTime(year, thatmonth, 1);
                DateTime lastDay = new DateTime(yearnext, nextmonth, 1);

                var positiondetails = db.PositionDetails.Where(d => d.paymentStatus == d.systemUpdate && d.systemUpdate > 0 && d.registerDate >= firstDay && d.registerDate < lastDay).ToList();
                ViewBag.list1 = positiondetails;
                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult systemupdate2()
        {
            if (Session["adminId"] != null)
            {
                int year = DateTime.Now.Year;
                int thatmonth = DateTime.Now.Month;
                int yearnext = DateTime.Now.Year;
                if (thatmonth == 12)
                {
                    yearnext = DateTime.Now.AddYears(1).Year;
                }
                int nextmonth = DateTime.Now.AddMonths(1).Month;

                DateTime firstDay = new DateTime(year, thatmonth, 1);
                DateTime lastDay = new DateTime(yearnext, nextmonth, 1);

                var positiondetails = db.PositionDetails.Where(d => d.paymentStatus == 0 && d.registerDate >= firstDay && d.registerDate < lastDay).ToList();
                ViewBag.list1 = positiondetails;
                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult systemupdate3()
        {
            if (Session["adminId"] != null)
            {
                int year = DateTime.Now.Year;
                int thatmonth = DateTime.Now.Month;
                int yearnext = DateTime.Now.Year;
                if (thatmonth == 12)
                {
                    yearnext = DateTime.Now.AddYears(1).Year;
                }
                int nextmonth = DateTime.Now.AddMonths(1).Month;

                DateTime firstDay = new DateTime(year, thatmonth, 1);
                DateTime lastDay = new DateTime(yearnext, nextmonth, 1);

                var positiondetails = db.PositionDetails.Where(d => d.systemUpdate == 0 && d.registerDate >= firstDay && d.registerDate < lastDay).ToList();
                ViewBag.list1 = positiondetails;
                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult nopayment()
        {
            if (Session["adminId"] != null)
            {
                int year = DateTime.Now.Year;
                int thatmonth = DateTime.Now.Month;
                int yearnext = DateTime.Now.Year;
                if (thatmonth == 12)
                {
                    yearnext = DateTime.Now.AddYears(1).Year;
                }
                int nextmonth = DateTime.Now.AddMonths(1).Month;

                DateTime firstDay = new DateTime(year, thatmonth, 1);
                DateTime lastDay = new DateTime(yearnext, nextmonth, 1);

                var positiondetails = db.PositionDetails.Where(d => d.systemUpdate == 0 && d.paymentStatus == 0 && d.registerDate >= firstDay && d.registerDate < lastDay).ToList();
                ViewBag.list1 = positiondetails;
                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult rejected()
        {
            if (Session["adminId"] != null)
            {
                AdminBonusCalculation bc = new AdminBonusCalculation();
                var rejectedposition = bc.rejectedPosition();
                ViewBag.list1 = rejectedposition;
                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        [HttpPost]
        public ActionResult rejected(PositionDetail positionDetail)
        {
            AddRejectedPosition.AddPosition(positionDetail.positionId);
            return RedirectToAction("rejected");
        }
        public ActionResult remainingPayment()
        {
            if (Session["adminId"] != null)
            {
                int year = DateTime.Now.Year;
                int thatmonth = DateTime.Now.Month;
                int yearnext = DateTime.Now.Year;
                if (thatmonth == 12)
                {
                    yearnext = DateTime.Now.AddYears(1).Year;
                }
                int nextmonth = DateTime.Now.AddMonths(1).Month;

                DateTime firstDay = new DateTime(year, thatmonth, 1);
                DateTime lastDay = new DateTime(yearnext, nextmonth, 1);

                var payment = db.PositionDetails.Where(d => d.positionCount > d.paymentStatus && d.paymentStatus > 0 && d.registerDate >= firstDay && d.registerDate < lastDay).ToList();
                ViewBag.payment = payment;
                return View();
            }
            else
            {
                return RedirectToAction("../kalamanakaru/login");
            }
        }
        public ActionResult remainingSystemUpdate()
        {
            if (Session["adminId"] != null)
            {
                int year = DateTime.Now.Year;
                int thatmonth = DateTime.Now.Month;
                int yearnext = DateTime.Now.Year;
                if (thatmonth == 12)
                {
                    yearnext = DateTime.Now.AddYears(1).Year;
                }
                int nextmonth = DateTime.Now.AddMonths(1).Month;

                DateTime firstDay = new DateTime(year, thatmonth, 1);
                DateTime lastDay = new DateTime(yearnext, nextmonth, 1);

                var payment = db.PositionDetails.Where(d => d.paymentStatus > d.systemUpdate && d.systemUpdate > 0 && d.registerDate >= firstDay && d.registerDate < lastDay).ToList();
                ViewBag.payment = payment;
                return View();
            }
            else
            {
                return RedirectToAction("../kalamanakaru/login");
            }

        }

        public ActionResult suspend()
        {
            if (Session["adminId"] != null)
                return View();
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult active()
        {

            if (Session["adminId"] != null)
                return View();
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult edituserpro()
        {
            if (Session["adminId"] != null)
                return View();
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        //Tree view
        public ActionResult treeviewStg1()
        {

            if (Session["adminId"] != null)
            {
                Session["AdminStage"] = "AdminStage1";
                return View(db.StageOnes.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).ToList());
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult treeviewStg2()
        {
            if (Session["adminId"] != null)
            {
                Session["AdminStage"] = "AdminStage2";
                return View(db.StageTwoes.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).ToList());
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult treeviewStg3()
        {
            if (Session["adminId"] != null)
            {
                Session["AdminStage"] = "AdminStage3";
                return View(db.StageThrees.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).ToList());
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult treeviewStg4()
        {
            if (Session["adminId"] != null)
            {
                Session["AdminStage"] = "AdminStage4";
                return View(db.StageFours.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).ToList());
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult treeviewStg5()
        {
            if (Session["adminId"] != null)
            {
                Session["AdminStage"] = "AdminStage5";
                return View(db.StageFives.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).ToList());
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult Index2(int? id)
        {
            if (Session["adminId"] != null)
            {
                if (id == 1)
                {
                    return Json(db.StageOnes.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).Select(d => new { tid = d.treeId, xx = d.treeColumn, yy = d.treeLevel, bn = d.bcNo, ft = d.freeStatus, idp = d.introducePromoCode, jp = d.jump, mn = d.membershipNo, pk = d.package, pos = d.positionCode }).ToList(), JsonRequestBehavior.AllowGet);
                }
                else if (id == 2)
                {
                    return Json(db.StageTwoes.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).Select(d => new { tid = d.treeId, xx = d.treeColumn, yy = d.treeLevel, bn = d.bcNo, jp = d.jump, mn = d.membershipNo, pk = d.package }).ToList(), JsonRequestBehavior.AllowGet);
                }
                else if (id == 3)
                {
                    return Json(db.StageThrees.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).Select(d => new { tid = d.treeId, xx = d.treeColumn, yy = d.treeLevel, bn = d.bcNo, jp = d.jump, mn = d.membershipNo, pk = d.package }).ToList(), JsonRequestBehavior.AllowGet);
                }
                else if (id == 4)
                {
                    return Json(db.StageFours.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).Select(d => new { tid = d.treeId, xx = d.treeColumn, yy = d.treeLevel, bn = d.bcNo, jp = d.jump, mn = d.membershipNo, pk = d.package }).ToList(), JsonRequestBehavior.AllowGet);
                }
                else if (id == 5)
                {
                    return Json(db.StageFives.OrderBy(q => q.treeLevel).ThenBy(q => q.treeColumn).Select(d => new { tid = d.treeId, xx = d.treeColumn, yy = d.treeLevel, bn = d.bcNo, jp = d.jump, mn = d.membershipNo, pk = d.package }).ToList(), JsonRequestBehavior.AllowGet);
                }
                return View();
            }
            else
                return RedirectToAction("../kalamanakaru/login");
        }

        public ActionResult rejectedCsv()
        {
            if (Session["adminId"] != null)
            {
                var getdatalist = new PositionDetail();
                getdatalist.positionId = 0;
                getdatalist.membershipNo = "0";

                ViewBag.getdatalist = getdatalist;
                ViewBag.csvAmount = db.CsvDatas.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("../kalamanakaru/login");
            }
        }
        [HttpPost]
        public ActionResult rejectedCsv(string Namebirthday)
        {
            var b = 0;
            string x = Namebirthday;
            string[] arr = x.Split(' ');
            string name = arr[0];
            int year = Convert.ToInt32(arr[1].Substring(0, 4));
            int month = Convert.ToInt32(arr[1].Substring(4, 2));
            int date = Convert.ToInt32(arr[1].Substring(6, 2));
            DateTime birthday = new DateTime(year, month, date);
            int count = db.Users.Where(d => d.name == name && d.dateOfBirth == birthday).Join(db.PositionDetails.Where(d => d.positionStatus == "pending"), o => o.membershipNo, m => m.membershipNo, (o, m) => new { m.membershipNo, m.positionId, m.positionCount, m.paymentStatus }).Count();
            if (count != 0)
            {
                var getdatalist = db.Users.Where(d => d.name == name && d.dateOfBirth == birthday).Join(db.PositionDetails, o => o.membershipNo, m => m.membershipNo, (o, m) => new { m.membershipNo, m.positionId, m.positionCount, m.paymentStatus }).ToList();
                return RedirectToAction("rejectedCsv");
            }
            else
            {
                var getdatalist = new PositionDetail();
                ViewBag.csvAmount = db.CsvDatas.ToList();
                getdatalist.positionId = 0;
                getdatalist.membershipNo = name;



                ViewBag.getdatalist = getdatalist;
                return View();
            }

        }

        //public ActionResult CheckPositionCoolingOff()
        //{
        //    //var coolingoff = db.suspendPositions.GroupBy(d => d.membershipNo).Join(db.StageOnes, a => a.Key, b => b.membershipNo, (a, b) => new { b.membershipNo, b.package, b.positionCode, b.treeColumn, b.treeId, b.bcNo, b.entryDate, b.freeStatus }).ToList();
        //    if (Session["adminId"] != null)
        //    {
        //        DateTime lastday = DateTime.Now.AddDays(-30);
        //        var docoolinoff = db.StageOnes.Where(d => d.entryDate <= DateTime.Now && d.entryDate > lastday && d.suspendStatus == 0).GroupBy(d => d.membershipNo).ToList();
        //        ViewBag.list1 = docoolinoff;

        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("../kalamanakaru/login");
        //    }
        //}
        //[HttpPost]
        //public ActionResult checkPositionCoolingOff(string membership)
        //{
        //    DateTime lastday = DateTime.Now.AddDays(-30);
        //    var docoolinoff = db.StageOnes.Where(d => d.entryDate <= DateTime.Now && d.entryDate > lastday && d.suspendStatus == 0 && d.membershipNo == membership).Select(d => d.treeId).ToList();
        //    return View();
        //}

        public ActionResult changeBonusValue()
        {
            if (Session["adminId"] != null)
            {
                List<PaidBonus> dList = new List<PaidBonus>();//How to get cooling off memebership number and change stageones Cooling off table
                var bonusdata = db.StageOnes.GroupBy(d => d.membershipNo).Join(db.PaidBonuss, a => a.Key, b => b.memberId, (a, b) => new
                {
                    ID = b.memberId,
                    BonusDate = b.paidBonusDateTime,
                    BonusId = b.paidBonusId,
                    BcNumber = b.bcNumber,
                    IntroduceBonus = b.paidIntoduceBonus,
                    PresentageBonus = b.paidPresentageBonus,
                    Paidthirtdstagebonus = b.paidThirdStageBonus,
                    PaidFifthStageBonus = b.paidFifthStageBonus,
                    PaidPositionBonus = b.paidpositionBonus,
                    PaidShareBonus = b.paidshareBonus,
                    PostitionHistry = b.positionHistory,
                    CompleteorNo = b.complteorno,
                    StageTreeId = b.stageonetreeid
                }).ToList();

                var nextlist = bonusdata.Select(d => new PaidBonus
                {
                    memberId = d.ID,
                    paidBonusDateTime = d.BonusDate,
                    paidBonusId = d.BonusId,
                    bcNumber = d.BcNumber,
                    paidIntoduceBonus = d.IntroduceBonus,
                    paidPresentageBonus = d.PresentageBonus,
                    paidThirdStageBonus = d.Paidthirtdstagebonus,
                    paidFifthStageBonus = d.PaidFifthStageBonus,
                    paidpositionBonus = d.PaidPositionBonus,
                    paidshareBonus = d.PaidShareBonus,
                    positionHistory = d.PostitionHistry,
                    complteorno = d.CompleteorNo,
                    stageonetreeid = d.StageTreeId
                }).ToList();
                dList.AddRange(nextlist);
                ViewBag.list1 = dList;

                return View();
            }
            else
            {
                return RedirectToAction("../kalamanakaru/login");
            }
        }

        [HttpPost]
        public ActionResult ChangeBonusValue(double fifteenlevel, double positionbonus, double presentagebonus, double sharebonus, double thirdrocket, double fifthrocket, int id)
        {
            var objec = db.PaidBonuss.Where(d => d.paidBonusId == id).FirstOrDefault();
            //objec.paidBonusDateTime = 2019-08-09;


            objec.paidIntoduceBonus = fifteenlevel;
            objec.paidPresentageBonus = presentagebonus;
            objec.paidThirdStageBonus = thirdrocket;
            objec.paidFifthStageBonus = fifthrocket;
            objec.paidpositionBonus = positionbonus;
            objec.paidshareBonus = sharebonus;

            var entry = db.Entry(objec);
            entry.State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("changeBonusValue");
        }

        public ActionResult issueBonusValue()
        {
            List<BonusDetail> issubonusbonus = new List<BonusDetail>();

            for (int i = 1; i < 26; i++)
            {
                var isuubonus1 = new BonusDetail();
                var isuuvalue = db.BonusDetails.OrderByDescending(d => d.bonusDetailId).Where(d => d.bonusId == i).ToList();
                if (isuuvalue.Count != 0)
                {
                    isuubonus1.bonusDetailId = isuuvalue[0].bonusDetailId;
                    isuubonus1.bonusId = isuuvalue[0].bonusId;
                    isuubonus1.description = isuuvalue[0].description;
                    isuubonus1.bonusAmount = isuuvalue[0].bonusAmount;
                    isuubonus1.changeBonusDate = isuuvalue[0].changeBonusDate;
                }
                issubonusbonus.Add(isuubonus1);
            }
            ViewBag.isuebonusvalue = issubonusbonus;

            return View();
        }
        [HttpPost]
        public ActionResult issueBonusValue(int bonusvalue, int positionid)
        {
            var bonus = new BonusDetail();
            bonus.bonusId = positionid;
            bonus.description = positionid.ToString();
            bonus.bonusAmount = bonusvalue;
            bonus.changeBonusDate = DateTime.Now;
            db.BonusDetails.Add(bonus);
            db.SaveChanges();
            return RedirectToAction("issueBonusValue");
        }
        public dynamic callCoin(string callmethod, ApiMyCoin x)
        {
            if (callmethod == "call")
            {
                callmethod = "notcall";
                return RedirectToAction("numberofcoin");
            }
            else
            {
                return x;
            }
        }

        //public async Task<List<ApiMyCoin>> numberofcoin()
        //{
        //    var data = db.Users.First();
        //    //string membershipno = Session["membershipNo"].ToString();
        //    string membershipno = "002175";

        //    string mydate = DateTime.Now.ToString("yyyy-MM-dd");
        //    string final = SHA.GenerateSHA256String((membershipno + mydate));
        //    string path = "https://fourssh.org/users/sendCointCount?membershipno=" + membershipno + "&key=" + final;

        //    var httpClient = new HttpClient();
        //    var json = await httpClient.GetStringAsync(path);
        //    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
        //    object routes_list = json_serializer.DeserializeObject(json);
        //    ViewBag.Organisations = db.CompaniesTypes.OrderBy(d => d.CompanyId).ToList();
        //    var model = JsonConvert.DeserializeObject<ApiMyCoin>(json);

        //    //ViewBag.foursshdata = model;
        //    return RedirectToAction("callCoin", model);



        //}
        public async Task<ActionResult> getAllCoin()
        {
            string path = "https://fourssh.org/users/sendCointTotalCount";

            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(path);
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            object routes_list = json_serializer.DeserializeObject(json);
            ViewBag.Organisations = db.CompaniesTypes.OrderBy(d => d.CompanyId).ToList();
            var model = JsonConvert.DeserializeObject<ApiMyCoin>(json);
            ViewBag.foursshdata = model;
            return View(model);
        }
    }
}