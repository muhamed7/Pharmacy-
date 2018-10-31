using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pharmacy.Models;
using pharmacy.Models.Reports;
using System.IO;

namespace pharmacy.Controllers
{
    public class ReportsController : Controller
    {
        Models.medicalEntities db = new medicalEntities();
        // GET: Reports
        public ActionResult Index()
        {
            return View(db.Stock.ToList());
        }

        //********************** for low stock items : Default **************************
        public ActionResult Stocks()
        {
            // return View();           
            return View(db.Stock.ToList());
        }



        [HttpPost]
        public ActionResult Stocks(StockSearchVM vm)
        {
            var filter = new StocksFilterRepository();
            var model = filter.FilterStocks(vm);
            return View(model);
        }

        public ActionResult TestStocks(StockSearchVM vm)
        {
            var filter = new StocksFilterRepository();
            var model = filter.FilterStocks(vm);
            return PartialView("_StocksPartialView", model);
        }


        //************************* End ***********************************
        public ActionResult DailySales()
        {

            var list = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, DateTime.Now) == 0).ToList();
            return View(list);
        }
        public ActionResult DailySalesFor(DateTime getDate)
        {
            var list = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, getDate) == 0).ToList();
            return PartialView("_DailySalesPartialView", list);
        }

        //Staffs
        public ActionResult EmployeesReports()
        {
            return null;
        }

        /// <summary>
        /// Display monthly Sales for each day in a month
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthlySalesByDate()
        {
            int year = 2018;
            int month = 12;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var days = Enumerable.Range(1, daysInMonth);
            var query = db.Sales.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month).OrderBy(x => x.Date).Select(g => new
            {
                Day = g.Date.Value.Day,
                Total = g.GrandTotal
            });
            var model = new SalesVM
            {
                Date = new DateTime(year, month, 1),
                Days = days.GroupJoin(query, d => d, q => q.Day, (d, q) => new DayTotalVM
                {
                    Day = d,
                    Total = Convert.ToDecimal(q.Sum(x => x.Total))
                }).ToList()
            };
            return View(model);
        }



        //Returns Monthly sales based on a parameter
        [HttpPost]
        public ActionResult MonthlySalesByDate(string _year, string _month)
        {
            //assign incoming values to the variables
            int year = 0, month = 0;
            //check if year is null
            if (string.IsNullOrWhiteSpace(_year) && _month != null)
            {
                year = DateTime.Now.Date.Year;
                month = Convert.ToInt32(_month.Trim());
            }
            else
            {
                year = Convert.ToInt32(_year.Trim());
                month = Convert.ToInt32(_month.Trim());
            }
            //calculate ttal number of days in a particular month for a that year 
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var days = Enumerable.Range(1, daysInMonth);
            var query = db.Sales.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month).OrderBy(x => x.Date.Value.Day).Select(g => new
            {
                Day = g.Date.Value.Day,
                Total = g.GrandTotal
            });
            var model = new SalesVM
            {
                Date = new DateTime(year, month, 1),
                Days = days.GroupJoin(query, d => d, q => q.Day, (d, q) => new DayTotalVM
                {
                    Day = d,
                    Total = Convert.ToDecimal(q.Sum(x => x.Total))
                }).ToList()
            };
            return View(model);
        }




        public List<DayTotalVM> MonthlySalesByDate_forCharts(int yr, int mnt)
        {
            int year = yr;
            int month = mnt;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var days = Enumerable.Range(1, daysInMonth);
            var query = db.Sales.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month).Select(g => new
            {
                Day = g.Date.Value.Day,
                Total = g.GrandTotal
            });
            SalesVM model = new SalesVM
            {
                Date = new DateTime(year, month, 1),
                Days = days.GroupJoin(query, d => d, q => q.Day, (d, q) => new DayTotalVM
                {
                    Day = d,
                    Total = Convert.ToDecimal(q.Sum(x => x.Total))
                }).ToList()
            };
            return model.Days.ToList();
        }
        //********************************************************************
        /// <summary>
        /// Yearly Sales
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult YearlySalesByMonth(string year)
        {
            int _year = 0;
            int _toYear = 0;
            if (string.IsNullOrWhiteSpace(year))
            {
                _year = DateTime.Now.Year;
                _toYear = _year + 1;
            }
            else
            {
                _year = Convert.ToInt32(year);
                _toYear = _year + 1;
            }

            //    int _year = DateTime.Now.Year;
            //    int _toYear = _year + 1;
            var query = db.Sales.Where(s => (s.Date.Value.Year >= _year && s.Date.Value.Year < _toYear));
            // YearlyReportVM _Model = new YearlyReportVM();
            List<MonthTotalVM> _Model = new List<MonthTotalVM>();

            for (int i = 1; i < 13; i++)
            {
                decimal _grandTotal = 0;
                decimal temp = 0;
                temp = query.Where(x => x.Date.Value.Month == i).Sum(x => (decimal?)(x.GrandTotal)) ?? 0;

                _grandTotal = temp;

                MonthTotalVM model = new MonthTotalVM()
                {
                    Year = _year,
                    Month = i,
                    GrandTotal = _grandTotal
                };
                _Model.Add(model);
            }

            return View(_Model.ToList());
        }



        public List<MonthTotalVM> YearlySalesByMonth_forCharts(string year)
        {
            int _year = 0;
            int _toYear = 0;
            if (string.IsNullOrWhiteSpace(year))
            {
                _year = DateTime.Now.Year;
                _toYear = _year + 1;
            }
            else
            {
                _year = Convert.ToInt32(year);
                _toYear = _year + 1;
            }

            var query = db.Sales.Where(s => (s.Date.Value.Year >= _year && s.Date.Value.Year < _toYear));
            List<MonthTotalVM> _Model = new List<MonthTotalVM>();

            for (int i = 1; i < 13; i++)
            {
                decimal _grandTotal = 0;
                decimal temp = 0;
                temp = query.Where(x => x.Date.Value.Month == i).Sum(x => (decimal?)(x.GrandTotal)) ?? 0;

                _grandTotal = temp;

                MonthTotalVM model = new MonthTotalVM()
                {
                    Year = _year,
                    Month = i,
                    GrandTotal = _grandTotal
                };
                _Model.Add(model);
            }
            return _Model.ToList();
        }

        //*********************************************************************
        /// <summary>
        /// Purchase Report
        /// </summary>
        /// <returns></returns>
        public ActionResult Purchase()
        {

            return View(db.Purchase.ToList());
        }


        [HttpPost]
        public ActionResult Purchase(PurchaseSearchVM vm)
        {
            var filter = new PurchaseFilterRepository();
            var model = filter.FilterPurchase(vm);
            return View(model);
        }

        //*********************************************************************       


    }
}