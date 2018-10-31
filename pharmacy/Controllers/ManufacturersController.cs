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
    public class ManufacturersController : Controller
    {
        Models.medicalEntities db = new medicalEntities();

        ManufacturerRepository repo = new ManufacturerRepository();
        private string oldManufacturerName = "";

        // GET: Manufacturers
        public ActionResult Index()
        {
            return View("Index", db.Manufacturer.OrderByDescending(m => m.ID).ToList());
        }

        // GET: Manufacturers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacturer manufacturer = db.Manufacturer.Find(id);
            if (manufacturer == null)
            {
                return HttpNotFound();
            }
            return View(manufacturer);
        }

        // GET: Manufacturers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ManufacturerName,Description")] Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                int count = repo.ManufacturerDuplicationCheck(manufacturer);

                if (count > 0)
                {
                    ViewBag.DuplicateError = "Already Exists!!";
                    return View  (manufacturer);
                }
                else
                {
                    db.Manufacturer.Add(manufacturer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(manufacturer);
        }

        public JsonResult CreateSubmitAjax([Bind(Include = "ID,ManufacturerName,Description")] Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                //check if duplication exists
                int count = repo.ManufacturerDuplicationCheck(manufacturer);
                //if yes throw an error message
                if (count > 0)
                {
                    ViewBag.DuplicateError = "Already Exists!!";
                    return Json("duplicate", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //else add new manufacturer and return to Index
                    db.Manufacturer.Add(manufacturer);
                    db.SaveChanges();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        // GET: Manufacturers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacturer manufacturer = db.Manufacturer.Find(id);
            if (manufacturer == null)
            {
                return HttpNotFound();
            }
            oldManufacturerName = manufacturer.ManufacturerName;
            return View(manufacturer);
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ManufacturerName,Description")] Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                //Get old object 
                var original = db.Manufacturer.Find(manufacturer.ID);

                //Compare old name with modified name 
                if (original.ManufacturerName != manufacturer.ManufacturerName)
                {  //check if Dublicate exists
                    int count = repo.ManufacturerDuplicationCheck(manufacturer);
                    //if yes throw erorr 
                    if (count >0)
                    {
                        ViewBag.DuplicateError = "Already Exists!!";
                        return View(manufacturer);
                    }
                }
                db.Entry(original).CurrentValues.SetValues(manufacturer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manufacturer);
        }

        // GET: Manufacturers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacturer manufacturer = db.Manufacturer.Find(id);
            if (manufacturer == null)
            {
                return HttpNotFound();
            }
            return View(manufacturer);
        }

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Manufacturer manufacturer = db.Manufacturer.Find(id);
                db.Manufacturer.Remove(manufacturer);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                //displays page with error message
                // return RedirectToAction("ReferenceError", "Extra");
                Response.Write(e.Message);
            }

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
