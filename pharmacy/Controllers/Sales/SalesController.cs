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
    public class SalesController : Controller
    {
         Models.medicalEntities db = new medicalEntities();

        // GET: Sales
        public ActionResult Index()
        {
            return View(db.Sales.OrderByDescending(s => s.ID).ToList());
        }

        // GET: Sales/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var _salesItems = (from si in db.SalesItem
        //                       where si.SalesID == id
        //                       select si).ToList();

        //    if (_salesItems == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(_salesItems.ToList());
        //}

        // GET: Sales/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Sales/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Date,Amount,Discount,Tax,GrandTotal,UserID,Remarks")] Sale sale)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Sales.Add(sale);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(sale);
        //}

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pharmacy.Models.Sale sales = db.Sales.Find(id);
            if (sales == null)
            {
                return HttpNotFound();
            }
            return View(sales);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Sales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Date,Amount,Discount,Tax,GrandTotal,UserID,Remarks")]pharmacy.Models.Sale sales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sales);
        }

        // GET: Sales/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Sale sale = db.Sales.Find(id);
        //    if (sale == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sale);
        //}

        //// POST: Sales/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Sale sale = db.Sales.Find(id);
        //    db.Sales.Remove(sale);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
