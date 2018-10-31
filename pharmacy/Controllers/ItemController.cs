using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using pharmacy.Models;

namespace pharmacy.Controllers
{
    public class ItemController : Controller
    {
        Models.medicalEntities db = new medicalEntities();

        // GET: Item
        public ActionResult Index()
        {
            var items = db.Item.Include(i => i.DrugGenericName).Include(i => i.Manufacturer).OrderByDescending(i => i.ID);
            return View(items.ToList());
        }

        // GET: Item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.DrugGenericNameID = new SelectList(db.DrugGenericName, "ID", "GenericName");
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer, "ID", "ManufacturerName");
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,DrugGenericNameID,ManufacturerID,Categeory,AlertQty,Description,LastUpdated")] Item item)
        {
            if (ModelState.IsValid)
            {
                int count = DuplicateCount(item);
                if (count > 0)
                {
                    ViewBag.DuplicateError = "Item already exists!!";
                    ViewBag.DrugGenericNameID = new SelectList(db.DrugGenericName, "ID", "GenericName", item.DrugGenericNameID);
                    ViewBag.ManufacturerID = new SelectList(db.Manufacturer, "ID", "ManufacturerName", item.ManufacturerID);
                    return View(item);
                }


                else
                {
                    item.LastUpdated = DateTime.Today;
                    db.Item.Add(item);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            ViewBag.DrugGenericNameID = new SelectList(db.DrugGenericName, "ID", "GenericName", item.DrugGenericNameID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer, "ID", "ManufacturerName", item.ManufacturerID);
            return View(item);
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.DrugGenericNameID = new SelectList(db.DrugGenericName, "ID", "GenericName", item.DrugGenericNameID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer, "ID", "ManufacturerName", item.ManufacturerID);
            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,DrugGenericNameID,ManufacturerID,Categeory,AlertQty,Description,LastUpdated")] Item item)
        {
            if (ModelState.IsValid)
            {
                var original = db.Item.Find(item.ID);

                if (original.Name != item.Name)
                {
                    int count = DuplicateCount(item);

                    if (count > 0)
                    {
                        ViewBag.DuplicateError = "Item already exists!!";
                        return View(item);
                    }
                }

                db.Entry(original).CurrentValues.SetValues(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DrugGenericNameID = new SelectList(db.DrugGenericName, "ID", "GenericName", item.DrugGenericNameID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer, "ID", "ManufacturerName", item.ManufacturerID);
            return View(item);
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Item.Find(id);
            db.Item.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public int DuplicateCount(Item item)
        {
            List<Item> _item = (from i in db.Item
                                where i.Name == item.Name
                                select i).ToList();
            return _item.Count;
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
