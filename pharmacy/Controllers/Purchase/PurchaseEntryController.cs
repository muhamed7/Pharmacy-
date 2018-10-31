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

    public class PurchaseEntryController : Controller
    {
        Models.medicalEntities db = new medicalEntities();
        private PurchaseEntryService service = new PurchaseEntryService();

        public ActionResult Index()
        {
            var vm = new pharmacy.Models.Purchase();

            // var vm = new PurchaseEntryVM();
            return View(vm);
        }


        /// <summary>
        /// Populate Supplier DropDown List
        /// </summary>
        /// <returns>Json data of supplier's list</returns>
        /// <remarks>The value is cached for 3 minutes so that it doesn't have to query to database again.</remarks>

        public JsonResult PopulateSupplier()
        {
            //holds list of suppliers
            List<Supplier> _supplierList = new List<Supplier>();
            //queries all the suppliers for its ID and Name property.
            var SelectSupplier = (from s in db.Supplier
                                  select new { s.ID, s.Name }).ToList();

            //save list of suppliers to the _supplierList
            foreach (var item in SelectSupplier)
            {
                _supplierList.Add(new Supplier
                {
                    ID = item.ID,
                    Name = item.Name
                });
            }
            //returns the Json result of _supplierList
            return Json(_supplierList, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Populate Items DropDownList
        /// </summary>
        /// <returns>Json data of Item's list</returns>
        /// <remarks>The value is cached for 3 minutes so that it doesn't have to query to database again.</remarks>

        public JsonResult PopulateItem()
        { //holds the list of item

            List<Item> _item = new List<Item>();

            //queries all the items in the database
            var itemList = (from i in db.Item
                            select new { i.ID, i.Name }).ToList();

            //save the list of items to the _item.
            foreach (var item in itemList)
            {
                _item.Add(new Item
                {
                    ID = item.ID,
                    Name = item.Name
                });
            }
            //returns the list of item in Json form 
            return Json(_item, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Post action for Saving data to database
        /// </summary>
        /// <param name="p">View model holding the entered data.</param>
        /// <returns>Returns value indicating if the data has been saved or failed to save.</returns>
        /// <remarks> Gets data as ViewModel rather than FormCollection.</remarks>
        [HttpPost]
        public JsonResult SavePurchase(pharmacy.Models.Purchase p)
        {
            bool status = false;

            if (p != null)
            {
                //new purchase object using the data from the viewmodel : PurchaseEntryVM
                pharmacy.Models.Purchase purchase = new Models.Purchase
                {
                    ID = p.ID,
                    Date = p.Date,
                    SupplierID = p.SupplierID,
                    Amount = p.Amount,
                    Discount = p.Discount,
                    Tax = p.Tax,
                    GrandTotal = p.GrandTotal,
                    IsPaid = p.IsPaid,
                    Description = p.Description,
                    LastUpdated = DateTime.Now
                };

                purchase.PurchaseItem = new List<PurchaseItem>();
                //populating the PurchaseItems from the PurchaseItems within ViewModel : PurchaseEntryVM
                foreach (var i in p.PurchaseItem)
                {
                    purchase.PurchaseItem.Add(i);
                }

                //add purchase 
                // finally save changes.
                service.AddPurchaseAndPurchseItems(purchase);
                service.InsertOrUpdateInventory(p.PurchaseItems);

                //if everything is sucessful, set status to true.
                status = true;
            }
            // return the status in form of Json
            return new JsonResult { Data = new { status = status } };
        }
    }
}
