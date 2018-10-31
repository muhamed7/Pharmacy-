using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using pharmacy.Models;

namespace pharmacy.Controllers.Sales
{
    public class SalesEntryController : Controller
    {
        private SalesEntryRepository repo = new SalesEntryRepository();
        private SalesEntryService service = new SalesEntryService();

        // GET: SalesEntry
        public ActionResult Index()
        {
            return View(repo.GetAllStock().Where(x => x.ExpiryDate > DateTime.Now && x.Qty != 0));
        }

        ////has fully functional UI 
        //public ActionResult IndexTest()
        //{
        //    return View(repo.GetAllStock());
        //}


        ////Another test attempt
        //public ActionResult IndexTest2()
        //{

        //    return View(repo.GetAllStock());
        //}

        [HttpPost]
        public JsonResult SerializeFormData(FormCollection _collection)
        {
            if (_collection != null)
            {
                string[] _stockID, _qty, _rate, _amt;
                //for salesItem
                _stockID = _collection["StockID"].Split(',');
                _qty = _collection["Qty"].Split(',');
                _rate = _collection["Rate"].Split(',');
                _amt = _collection["Amount"].Split(',');
                //for sales
                decimal _total = Convert.ToDecimal(_collection["Total"]);
                decimal _discount = Convert.ToDecimal(_collection["Discount"]);
                decimal _grandTotal = Convert.ToDecimal(_collection["GrandTotal"]);
                DateTime _date = DateTime.Now;

                //instance of the global class
                MvcApplication app = new MvcApplication();
                pharmacy.Models.Sale _sales = new pharmacy.Models.Sale()
                {
                    Date = _date,
                    Amount = _total,
                    Discount = _discount,
                    GrandTotal = _grandTotal,
                    Tax = 0,
                    //  UserID = User.Identity.GetUserId(),
                    Remarks = "-"
                };





                //insert into sales, sales-items, stock
                int salesID = service.InsertSales(_sales);
                service.InsertSalesItem(salesID, _stockID, _qty, _rate, _amt);
                service.UpdateStock(_stockID, _qty);

                return Json(salesID);


            }
            return Json("null");
        }

        public ActionResult SalesInfo()
        {
            return View(service.GetAllSalesInfo());
        }


        //Generates Sales Invoice
        public ActionResult SalesInvoice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _sales = service.GetSales(id);
            //check if id supplied is present or not.
            if (_sales == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                return View(_sales);
            }

        }
    }
}
