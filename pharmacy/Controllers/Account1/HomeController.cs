using pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;


namespace pharmacy.Controllers
{
    public class HomeController : Controller
    {
        Models.medicalEntities db = new medicalEntities();

        public ActionResult Index()
        {
            return View();
        }





        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }




        public JsonResult ForMorris()
        {
            string year = Convert.ToString(DateTime.Today.Year);

            ReportsController rc = new ReportsController();
            var model = rc.YearlySalesByMonth_forCharts(year).ToList();

            var count = model.Select(i => new Object[] { i.GrandTotal }).ToList();
            return Json(count, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ForMorris2()
        {
            ReportsController rc = new ReportsController();
            var model = rc.MonthlySalesByDate_forCharts(DateTime.Today.Year, DateTime.Today.Month);

            var value = model.Select(i => new Object[] { i.Day.ToString(), i.Total }).ToArray();
            return Json(value, JsonRequestBehavior.AllowGet);
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