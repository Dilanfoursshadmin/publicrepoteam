using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tshirt.Context;

namespace Tshirt.Controllers
{
    public class OverviewController : Controller
    {
        // GET: Overview
        private static void Main(string [] args)
        {
            using (var dbContext=new tshirtContext())
            {

                dbContext.Database.Initialize(true);
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Progamming()
        {

            return View();
        }
        public ActionResult Classes()
        {

            return View();
        }
        public ActionResult Couple()
        {
            return View();
        }
        public ActionResult Family()
        {
            return View();
        }
        public ActionResult Other()
        {
            return View();
        }
        public ActionResult Games()
        {
            return View();
        }
        public ActionResult Customizeorder()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }
        public ActionResult BuyerRegiser()
        {
            return View();
        }
        public ActionResult CompanyRegister()
        {
            return View();
        }
        public ActionResult SuccessfullyRegister()
        {
            return View();
        }
    }
}