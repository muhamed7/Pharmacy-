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
    public class DrugGenericNamesController : Controller
    {
        Models.medicalEntities db = new medicalEntities();

        // GET: DrugGenericNames
        public ActionResult Index()
        {
            return View(db.DrugGenericName.ToList());
        }

        // GET: DrugGenericNames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DrugGenericName drugGenericName = db.DrugGenericName.Find(id);
            if (drugGenericName == null)
            {
                return HttpNotFound();
            }
            return View(drugGenericName);
        }

        // GET: DrugGenericNames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DrugGenericNames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,GenericName,Description")] DrugGenericName drugGenericName)
        {
            if (ModelState.IsValid)
            {
                //check if values is duplicate
                int count = DuplicateCount(drugGenericName);
                if (count > 0)
                {
                    ViewBag.DuplicateError = "Already Exists!!";
                    return View(drugGenericName);
                }
                else
                {
                    db.DrugGenericName.Add(drugGenericName);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(drugGenericName);
        }
    

        // GET: DrugGenericNames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DrugGenericName drugGenericName = db.DrugGenericName.Find(id);
            if (drugGenericName == null)
            {
                return HttpNotFound();
            }
            return View(drugGenericName);
        }

        // POST: DrugGenericNames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,GenericName,Description")] DrugGenericName drugGenericName)
        {
            if (ModelState.IsValid)
            {
                //get old generic name 
                var original = db.DrugGenericName.Find(drugGenericName.ID);

                //compare old name to new 
                if (original.GenericName != drugGenericName.GenericName)
                {
                    //check for duplication
                    int count = DuplicateCount(drugGenericName);


                    if (count > 0)
                    {
                        //error message
                        ViewBag.DuplicateError = "Generic name already Exists!!";
                        return View(drugGenericName);
                    }
                }

                db.Entry(original).CurrentValues.SetValues(drugGenericName);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(drugGenericName);
        }

        // GET: DrugGenericNames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DrugGenericName drugGenericName = db.DrugGenericName.Find(id);
            if (drugGenericName == null)
            {
                return HttpNotFound();
            }
            return View(drugGenericName);
        }

        // POST: DrugGenericNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DrugGenericName drugGenericName = db.DrugGenericName.Find(id);
            db.DrugGenericName.Remove(drugGenericName);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public int DuplicateCount(DrugGenericName drugGenericName)
        {
            List<DrugGenericName> _checkUnique = (from d in db.DrugGenericName
                                                  where d.GenericName == drugGenericName.GenericName
                                                  select d).ToList();
            return _checkUnique.Count;
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
