using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RocketSystem.Classes;
using RocketSystem.DbLink;
using RocketSystem.Models;

namespace RocketSystem.Controllers
{
    public class PositionDetailsController : Controller
    {
        private DataAccessLayer db = new DataAccessLayer();

        // GET: PositionDetails
        public ActionResult Index()
        {
            return View(db.PositionDetails.ToList());
        }

        // GET: PositionDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionDetail positionDetail = db.PositionDetails.Find(id);
            if (positionDetail == null)
            {
                return HttpNotFound();
            }
            return View(positionDetail);
        }

        // GET: PositionDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PositionDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PositionDetail positionDetail)
        {
            TemporaryPosition temporaryPosition = new TemporaryPosition();
            var x = Session["IntroducerExists"].ToString();
            if (ModelState.IsValid)
            {
                string[] pakcount = positionDetail.package.Split(',');
                for (int start = 0; start < pakcount.Length; start++)
                {
                    positionDetail.registerDate = DateTime.Now;
                    positionDetail.introducePromoCode = Session["introducePromoCode"].ToString();
                    string[] packdetails = pakcount[start].Split('-');
                    int packages = Convert.ToInt32(packdetails[1]);
                    positionDetail.positionCount = packages;
                    positionDetail.package = packdetails[0];
                    positionDetail.depositDate = positionDetail.depositDate;
                    positionDetail.paymentStatus = 0;
                    positionDetail.systemUpdate = 0;
                    positionDetail.positionStatus = "pending";
                    db.PositionDetails.Add(positionDetail);
                    db.SaveChanges();
                    for (int ax = 0; ax < packages; ax++)
                    {
                        temporaryPosition.positionId = positionDetail.positionId;
                        temporaryPosition.positionCode = TemporyRules.sendTempCode(positionDetail.membershipNo, positionDetail.registerDate);
                        db.TemporaryPositions.Add(temporaryPosition);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            return View(positionDetail);
        }

        public ActionResult insertPositionDetails(PositionDetail positionDetail)
        {
            TemporaryPosition temporaryPosition = new TemporaryPosition();
            string[] pakcount = positionDetail.package.Split(',');
            for (int start = 0; start < pakcount.Length; start++)
            {
                positionDetail.registerDate = DateTime.Now;
                positionDetail.introducePromoCode = Session["introducePromoCode"].ToString();
                positionDetail.membershipNo = Session["membershipNo"].ToString();
                string[] packdetails = pakcount[start].Split('-');
                int xp;
                if (!int.TryParse(packdetails[1], out xp))
                {
                    return RedirectToAction("../trees/index");
                }
                else if (xp < 0)
                {

                }
                int packages = Convert.ToInt32(packdetails[1]);
                positionDetail.positionCount = packages;
                positionDetail.package = packdetails[0];
                positionDetail.depositDate = positionDetail.depositDate;
                positionDetail.paymentStatus = 0;
                positionDetail.systemUpdate = 0;
                positionDetail.positionPriority = Convert.ToInt32(packdetails[2]);
                positionDetail.positionStatus = "pending";
                db.PositionDetails.Add(positionDetail);
                db.SaveChanges();
                for (int ax = 0; ax < packages; ax++)
                {
                    temporaryPosition.positionId = positionDetail.positionId;
                    temporaryPosition.positionCode = TemporyRules.sendTempCode(positionDetail.membershipNo, positionDetail.registerDate);
                    db.TemporaryPositions.Add(temporaryPosition);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Trees");
        }

        public ActionResult getCurrentPackages()
        {
            var currentPackages = db.Packages.Where(dt => dt.packageStartDate <= DateTime.Today && dt.packageEndDate >= DateTime.Today).ToList();
            return Json(currentPackages, JsonRequestBehavior.AllowGet);
        }

        // GET: PositionDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionDetail positionDetail = db.PositionDetails.Find(id);
            if (positionDetail == null)
            {
                return HttpNotFound();
            }
            return View(positionDetail);
        }

        // POST: PositionDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "positionId,membershipNo,introducePromoCode,registerDate,depositDate,package,packageStyle,positionCount,paymentStatus,systemUpdate")] PositionDetail positionDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(positionDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(positionDetail);
        }

        // GET: PositionDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionDetail positionDetail = db.PositionDetails.Find(id);
            if (positionDetail == null)
            {
                return HttpNotFound();
            }
            return View(positionDetail);
        }

        // POST: PositionDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PositionDetail positionDetail = db.PositionDetails.Find(id);
            db.PositionDetails.Remove(positionDetail);
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
