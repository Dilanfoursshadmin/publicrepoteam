using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RocketSystem.Classes;
using RocketSystem.DbLink;
using RocketSystem.Models;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace RocketSystem.Controllers
{
    public class UsersController : Controller
    {
        private DataAccessLayer db = new DataAccessLayer();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult UserRegistration()
        {
            ViewBag.Organisations = db.CompaniesTypes.OrderBy(d => d.CompanyId).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult UserRegistration(User user, TempUseLogin tempUseLogin)
        {
            if (ModelState.IsValid)
            {
                var Existingmemcheck = db.Users.Where(e => e.katakanaName == user.katakanaName && e.dateOfBirth == user.dateOfBirth).FirstOrDefault();
                if (Existingmemcheck == null)
                {
                    int maxMemberId = Convert.ToInt32(db.Users.ToList().Max(x => x.membershipNo));
                    int NewMemberId = maxMemberId + 1;
                    if (maxMemberId < 4000)
                    {
                        NewMemberId = 4000;
                    }
                    var NewMembershipNo = NewMemberId.ToString().PadLeft(6, '0');
                    user.membershipNo = NewMembershipNo;
                    db.Users.Add(user);
                    db.SaveChanges();
                    // ***************
                    var NewUserID = db.Users.Where(e => e.membershipNo == NewMembershipNo).FirstOrDefault();
                    //****************
                    String shapswd = SHA.GenerateSHA256String(NewUserID.membershipNo + tempUseLogin.password);
                    UserLogin userLogin = new UserLogin();
                    userLogin.membershipNo = NewMembershipNo;
                    userLogin.userName = NewMembershipNo;
                    userLogin.password = shapswd;
                    userLogin.questionOne = tempUseLogin.questionOne;
                    userLogin.answerOne = tempUseLogin.answerOne;
                    userLogin.questionTwo = tempUseLogin.questionTwo;
                    userLogin.answerTwo = tempUseLogin.answerTwo;
                    userLogin.status = 0;
                    userLogin.datetime = DateTime.Now;
                    userLogin.uID = NewUserID.userId;

                    db.UserLogins.Add(userLogin);
                    db.SaveChanges();

                    Session["membershipNo"] = NewMembershipNo.ToString();
                    Session["memberName"] = user.name.ToString();
                    //int coin = db.StageOnes.Where(d => d.membershipNo == userLogin.membershipNo.ToString() && d.package == 1).Count() * 5000 + db.StageOnes.Where(d => d.membershipNo == userLogin.membershipNo.ToString() && d.package == 2).Count() * 17500;
                    //long coin = Task.Run(() => BonusCalculation.numberofcoin(userLogin.membershipNo.ToString())).Result;
                    //long apitotalcoin = Task.Run(() => BonusCalculation.getAllCoin()).Result;

                    Session["coin"] = "";
                    Session["apitotalcoin"] = "";
                    Session["tempnum"] = "1";

                    return RedirectToAction("ViewMembershipNo");
                }
                return RedirectToAction("login");
            }
            else
            {
                ViewBag.Organisations = db.CompaniesTypes.OrderBy(d => d.CompanyId).ToList();
                return View();
            }
        }

        public ActionResult ViewMembershipNo()
        {
            return View();
        }

        public ActionResult findUser(string username)
        {
            ViewBag.Organisations = db.CompaniesTypes.OrderBy(d => d.CompanyId).ToList();
            var findusername = db.UserLogins.Where(d => d.userName == username).FirstOrDefault();
            return Json(findusername, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            Session["membershipNo"] = "";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                string membershipno = userLogin.membershipNo; // username or membershipNo of user type 
                string password = userLogin.password;

                var exismembershipNo = db.UserLogins.Where(x => x.membershipNo == membershipno).FirstOrDefault();
                if (exismembershipNo != null)
                {
                    String shapswd = SHA.GenerateSHA256String(membershipno + password);
                    var loginUser = db.UserLogins.Where(x => x.membershipNo == membershipno && x.password == shapswd).FirstOrDefault();
                    var listofmem = db.Users.Where(x => x.membershipNo == membershipno).FirstOrDefault();
                    if (loginUser != null)
                    {
                        if (loginUser.status == 0)
                        {
                            Session["membershipNo"] = loginUser.membershipNo.ToString();
                            Session["memberName"] = listofmem.name.ToString();
                            long coin = Task.Run(() => BonusCalculation.numberofcoin(userLogin.membershipNo.ToString())).Result;
                            long apitotalcoin = Task.Run(() => BonusCalculation.getAllCoin()).Result;

                            Session["coin"] = coin;
                            Session["apitotalcoin"] = apitotalcoin;
                            return RedirectToAction("Index", "Trees");
                        }
                        else
                        {
                            ViewBag.RejectU = 1;
                        }
                    }
                }
                else
                {
                    string mydate = DateTime.Now.ToString("yyyy-MM-dd");
                    string gethash = SHA.GenerateSHA256String((membershipno + password));
                    string final = SHA.GenerateSHA256String((gethash + mydate));
                    //string path = "http://192.168.1.14/users/accesOldMember?membershipno=" + membershipno + "&findhash=" + final;
                    string path = "https://fourssh.org/users/accesOldMember?membershipno=" + membershipno + "&findhash=" + final;
                    var httpClient = new HttpClient();
                    var json = await httpClient.GetStringAsync(path);
                    if (json != "")
                    {
                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        object routes_list = json_serializer.DeserializeObject(json);

                        var model = JsonConvert.DeserializeObject<MemberDetail>(json);
                        model.password = gethash; //final or gethash New add 2019-10-29
                        User user = new User();
                        var NewMembershipNo = model.MemberNo;
                        user.membershipNo = NewMembershipNo;
                        user.name = model.name;
                        DateTime birthday = Convert.ToDateTime(model.birthday);// Use current time.
                        user.dateOfBirth = birthday;
                        user.telephoneNo = model.telephoneNo;
                        user.mobileNo = model.mobileNo;
                        user.postalCode = model.postalNo;
                        user.katakanaName = model.nameInKana;
                        user.nickName = model.nickName;
                        user.gender = model.gender;
                        user.addressOne = model.address1;
                        user.faxNo = model.faxNo;
                        user.webEmail = model.email;
                        user.mobileEmail = model.email;
                        user.accountName = model.nameOfTheAccountHolder;
                        user.accountNameKatakana = model.nameOfTheAccountHolderInKana;
                        user.bankNameKatakana = model.branchNameKana;
                        user.transferDestinationBank = model.transferDestinationBankCode;
                        user.branchNameKatakana = model.branchNameKana;
                        user.transferDestinationBranchCode = model.transferDestinationBranchCode;
                        user.accountClassification = model.accountClassification;
                        user.transferAccountNumber = model.transferAccountNumber;
                        user.userName = membershipno;
                        user.accountClassification2 = model.accountClassification2;
                        user.companyType = model.companyType;
                        user.foursshMember = model.MemberNo;

                        db.Users.Add(user);
                        db.SaveChanges();
                        // ***************
                        var NewUserID = db.Users.Where(e => e.membershipNo == NewMembershipNo).FirstOrDefault();
                        //****************
                        userLogin.membershipNo = NewMembershipNo;
                        userLogin.userName = membershipno;
                        userLogin.password = model.password;
                        userLogin.status = 0;
                        userLogin.datetime = DateTime.Now;
                        userLogin.uID = NewUserID.userId;

                        db.UserLogins.Add(userLogin);
                        db.SaveChanges();

                        Session["membershipNo"] = NewMembershipNo.ToString();
                        Session["memberName"] = user.name.ToString();
                        //Session["username"] = user.userName.ToString();
                        long coin = Task.Run(() => BonusCalculation.numberofcoin(userLogin.membershipNo.ToString())).Result;
                        long apitotalcoin = Task.Run(() => BonusCalculation.getAllCoin()).Result;

                        Session["coin"] = coin;
                        Session["apitotalcoin"] = apitotalcoin;

                        return RedirectToAction("Index", "Trees");
                    }
                    ViewBag.RejectU = 2;
                    return View();
                }

                return View();
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            //if (Session["membershipNo"] != null)
            //{
            //    return RedirectToAction("Index");
            //}
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword fpwd)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    fpwd.ip = Request.UserHostAddress;
                    DateTime today = DateTime.Today;
                    fpwd.date = today;
                    ViewBag.forget = "";
                    string fuid = fpwd.memNo.ToString();
                    var UserList = db.UserLogins.Where(e => e.membershipNo == fuid.ToString()).ToList();
                    int ucount = db.UserLogins.Where(e => e.membershipNo == fuid.ToString()).ToList().Count();
                    if (ucount > 0)
                    {
                        Session["tempId"] = UserList[0].membershipNo;
                        Session["membershipNo"] = null;
                        Session["username"] = UserList[0].membershipNo;
                        return RedirectToAction("UserSecurity");
                    }
                    else
                    {
                        ViewBag.forget = "無効なユーザー名";
                    }
                }
                catch (Exception)
                {
                    ViewBag.forget = "無効なユーザー名";
                }
            }
            return View();
        }

        public ActionResult UserSecurity()
        {
            if (Session["membershipNo"] != null)
            {
                return RedirectToAction("login");

            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UserSecurity(SecurityQuestion securityQuestion, ForgetQuestion forgetQuestion)
        {
            if (ModelState.IsValid)
            {
                string mno = Session["tempId"].ToString();
                int result = db.UserLogins.Where(d => d.membershipNo == mno && d.answerOne == forgetQuestion.sq1 && d.answerTwo == forgetQuestion.sq2).Count();
                if (result == 1)
                {
                    Session["ecode"] = "yes";
                    return RedirectToAction("ChangePassword");
                }
                else
                {
                    ViewBag.ermessage = "答えは間違えています。";
                    return View();
                }
            }
            return View();
        }

        public ActionResult ChangePassword()
        {
            if (Session["membershipNo"] == null)
            {
                if (Session["membershipNo"] != null || Session["tempId"] != null)
                {
                    try
                    {
                        if (Session["membershipNo"] == null && Session["ecode"].ToString() == "yes")
                        {
                            return View();
                        }
                        if (Session["membershipNo"] != null)
                        {
                            return View();

                        }
                        else
                        {
                            return RedirectToAction("Login");
                        }
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel userpwd)
        {
            if (ModelState.IsValid)
            {
                using (DataAccessLayer db = new DataAccessLayer())
                {
                    if (Session["membershipNo"] == null)
                    {
                        string uid = "0";
                        UserLogin user2 = new UserLogin();

                        int ucount = 0;
                        if (Session["membershipNo"] == null)
                        {

                            uid = Session["username"].ToString();
                        }
                        else if (Session["tempId"] == null)
                        {
                            string suid = Session["username"].ToString() + userpwd.oldPassword;
                            string oldone = SHA.GenerateSHA256String(suid);
                            uid = Session["username"].ToString();
                            ucount = db.UserLogins.Where(d => d.userName == uid && d.password == oldone).Count();
                        }

                        try
                        {
                            var getdata = db.UserLogins.Where(d => d.membershipNo == uid).ToList();

                            user2 = db.UserLogins.Find(getdata[0].userLogID);
                            ViewBag.errors = "";
                            if (Session["tempId"] != null || ucount == 1)
                            {
                                ViewBag.errors = "";
                                if (userpwd.Password == userpwd.password1)
                                {
                                    if (userpwd.Password.Length >= 8)
                                    {
                                        if (ucount == 0)
                                        {
                                            Session["membershipNo"] = Session["tempId"];
                                            Session["username"] = getdata[0].userName;
                                            //Session["nickname"] = getdata[0].nickName.ToString();
                                            string yy = Session["membershipNo"].ToString();
                                            Session["tempId"] = null;
                                        }
                                        user2.password = SHA.GenerateSHA256String(uid + userpwd.Password);
                                        //user2.logType = 1;
                                        ViewBag.errors = "";
                                        db.UserLogins.Attach(user2);

                                        db.Entry(user2).State = EntityState.Modified;

                                        db.SaveChanges();

                                        return RedirectToAction("login");
                                    }
                                    else
                                    {
                                        ViewBag.errors = "パスワードは短すぎます。";
                                        return View();
                                    }

                                }
                                else
                                {
                                    ViewBag.errors = "パスワードが一致しません。";
                                    return View(userpwd);
                                }
                            }
                            else
                            {
                                ViewData["Message"] = "旧パスワードが間違えています。";
                                return View();
                            }
                        }
                        catch (Exception)
                        {
                            ViewBag.errors = "Error!!!";
                            return View();
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            else
            {
                ViewData["Message"] = "";
                return View();
            }
        }


        public ActionResult sQuectionResult()// Search for security question of the person logged in user
        {
            string mno = Session["tempId"].ToString();
            var answers = db.UserLogins.Where(d => d.membershipNo == mno.ToString()).ToList();
            int sq1 = Convert.ToInt32(answers[0].questionOne);
            int sq2 = Convert.ToInt32(answers[0].questionTwo);
            var xps = db.SecurityQuestions.Where(d => d.securityQuestionId == sq1 || d.securityQuestionId == sq2).ToList();
            return Json(xps, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ForgotMembershipNo()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotMembershipNo(FogotMemNo fogotMemNo)
        {
            if (ModelState.IsValid)
            {
                var Checkmemberlist = db.Users.Where(e => e.katakanaName == fogotMemNo.katakanaName && e.dateOfBirth == fogotMemNo.dateOfBirth).FirstOrDefault();
                if (Checkmemberlist != null)
                {
                    string mem = Checkmemberlist.membershipNo;
                    Session["membershipNo"] = mem;
                    Session["tempnum"] = "0";
                    return RedirectToAction("ViewMembershipNo");
                }
                ViewBag.forget = "Invalid name or birthday";
                return View();
            }
            return View();
        }

        //***************************************EditProfileDetail************************
        public ActionResult EditProfileDetail()
        {
            //if (Session["membershipNo"] == null)
            //{
            //    return RedirectToAction("Index");
            //}
            string memberid = Session["membershipNo"].ToString();

            ViewBag.Organisations = db.CompaniesTypes.OrderBy(d => d.CompanyId).ToList();
            User user = new User();
            var getdata = db.Users.Where(d => d.membershipNo == memberid).ToList();

            return View(getdata);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfileDetail(User user)
        {
            if (Session["membershipNo"] == null)
            {
                return RedirectToAction("Index");
            }
            if (user.name != null && user.mobileNo != null && user.addressOne != null && user.postalCode != null)
            {
                string memberid = Session["membershipNo"].ToString();
                var getdata = db.Users.Where(d => d.membershipNo == memberid).ToList();
                User memberDetail2 = new User();
                memberDetail2 = db.Users.Find(getdata[0].userId);
                memberDetail2.name = user.name;
                memberDetail2.katakanaName = user.katakanaName;
                memberDetail2.nickName = user.nickName;
                memberDetail2.dateOfBirth = user.dateOfBirth;
                memberDetail2.postalCode = user.postalCode;
                memberDetail2.addressOne = user.addressOne;
                memberDetail2.telephoneNo = user.telephoneNo;
                memberDetail2.mobileNo = user.mobileNo;
                memberDetail2.faxNo = user.faxNo;
                memberDetail2.webEmail = user.webEmail;
                memberDetail2.mobileEmail = user.mobileEmail;
                memberDetail2.gender = user.gender;
                memberDetail2.bankNameKatakana = user.bankNameKatakana;
                memberDetail2.branchNameKatakana = user.branchNameKatakana;
                memberDetail2.transferDestinationBank = user.transferDestinationBank;
                memberDetail2.transferDestinationBranchCode = user.transferDestinationBranchCode;
                memberDetail2.accountClassification = user.accountClassification;
                memberDetail2.accountClassification2 = user.accountClassification2;
                memberDetail2.transferAccountNumber = user.transferAccountNumber;
                memberDetail2.accountNameKatakana = user.accountNameKatakana;
                memberDetail2.companyType = user.companyType;

                db.Users.Attach(memberDetail2);
                db.Entry(memberDetail2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserProfile", "Trees");
            }
            return RedirectToAction("EditProfileDetail");
        }

        //***************************************login As Fourssh  AND GET DATA************************

        public ActionResult loginAsFourssh()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> loginAsFourssh(LoginAsFourssh loginAsFourssh)
        {
            if (ModelState.IsValid)
            {
                var exismembershipNo = db.Users.Where(x => x.foursshMember == loginAsFourssh.membershipNo).FirstOrDefault();
                if (exismembershipNo == null)
                {
                    ViewBag.Existing = "";
                    string membershipno = loginAsFourssh.membershipNo;
                    string password = loginAsFourssh.password;
                    string mydate = DateTime.Now.ToString("yyyy-MM-dd");
                    string gethash = SHA.GenerateSHA256String((membershipno + password));
                    string final = SHA.GenerateSHA256String((gethash + mydate));
                    string path = "http://192.168.1.14/users/accesOldMember?membershipno=" + membershipno + "&findhash=" + final;
                    var httpClient = new HttpClient();
                    var json = await httpClient.GetStringAsync(path);
                    if (json != "")
                    {
                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        object routes_list = json_serializer.DeserializeObject(json);
                        ViewBag.Organisations = db.CompaniesTypes.OrderBy(d => d.CompanyId).ToList();
                        var model = JsonConvert.DeserializeObject<MemberDetail>(json);
                        TempData["ModelName"] = model;
                        return RedirectToAction("UserRegistration");
                    }
                    return RedirectToAction("loginAsFourssh");
                }
                ViewBag.Existing = "Existing Member";
                return View();
            }
            return View();
        }

        public ActionResult accesOldMember(string membershipno, string findhash)
        {
            var checkdata = db.UserLogins.Where(a => a.membershipNo == membershipno).FirstOrDefault();
            if (checkdata != null)
            {

                string gethash = checkdata.password;
                string mydate = DateTime.Now.ToString("yyyy-MM-dd");
                string final = SHA.GenerateSHA256String((gethash + mydate));
                if (final == findhash)
                {
                    var getdata = db.Users.Where(d => d.membershipNo == membershipno).FirstOrDefault();
                    return Json(getdata, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userId,name,membershipNo,dateOfBirth,telephoneNo,mobileNo,postalCode,katakanaName,nickName,gender,addressOne,addressTwo,addressThree,faxNo,webEmail,mobileEmail,accountName,accountNameKatakana,bankNameKatakana,transferDestinationBank,branchNameKatakana,transferDestinationBranchCode,accountClassification,transferAccountNumber,userName,password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,name,membershipNo,dateOfBirth,telephoneNo,mobileNo,postalCode,katakanaName,nickName,gender,addressOne,addressTwo,addressThree,faxNo,webEmail,mobileEmail,accountName,accountNameKatakana,bankNameKatakana,transferDestinationBank,branchNameKatakana,transferDestinationBranchCode,accountClassification,transferAccountNumber,userName,password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
