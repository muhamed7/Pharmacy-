using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using pharmacy.Models;

namespace pharmacy.Controllers.Purchase
{
    public class PurchaseController : Controller
    {
        Models.medicalEntities db = new medicalEntities();

        SupplierRepository repo = new SupplierRepository();
        // GET: Purchase
        public ActionResult Index()
        {
            var purchases = db.Purchase.Include(p => p.SupplierID);
            return View(purchases.ToList());
        }

        // GET: Purchase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _purchase = from p in db.PurchaseItem
                            where p.PurchaseID == id
                            select p;
            if (_purchase == null)
            {
                return HttpNotFound();
            }
            return View(_purchase.ToList());
        }

        //// GET: Purchase/Create
        //public ActionResult Create()
        //{
        //    ViewBag.SupplierID = new SelectList(db.Supplier, "ID", "Name");
        //    return View();
        //}

        //// POST: Purchase/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Date,SupplierID,Amount,Discount,Tax,GrandTotal,IsPaid,LastUpdated,Description")] Purchase purchase)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Purchase.Add(purchase);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.SupplierID = new SelectList(db.Supplier, "ID", "Name", purchase.SupplierID);
        //    return View(purchase);
        //}

        // GET: Purchase/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pharmacy.Models.Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = new SelectList(db.Supplier, "ID", "Name", purchase.SupplierID);
            return View(purchase);
        }

        // POST: Purchase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Date,SupplierID,Amount,Discount,Tax,GrandTotal,IsPaid,LastUpdated,Description")]pharmacy.Models.Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierID = new SelectList(db.Supplier, "ID", "Name", purchase.SupplierID);
            return View(purchase);
        }

        // GET: Purchase/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pharmacy.Models.Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            pharmacy.Models.Purchase purchase = db.Purchase.Find(id);
            db.Purchase.Remove(purchase);
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
