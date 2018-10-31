using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pharmacy.Models.Reports
{
    public class PurchaseFilterRepository
    {

        Models.medicalEntities db = new medicalEntities();

        public PurchaseFilterRepository()
        {

        }

        public IQueryable<Purchase> FilterPurchase(PurchaseSearchVM vm)
        {
            var result = db.Purchase.AsQueryable();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.option))
                {
                    if (vm.option == "today")
                    {
                        result = result.Where(p => p.Date.Value.Date == DateTime.Now.Date);

                    }
                    else if (vm.option == "yesterday")
                    {
                        result = result.Where(p => p.Date.Value.Date == DateTime.Now.Date.AddDays(-1));
                    }
                    else if (vm.option == "thisWeek")
                    {
                        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                        result = result.Where(x => x.Date >= startDayOfWeek && x.Date <= endDayOfWeek);
                        // result = result.Where(p=> p.Date.);
                    }
                    else if (vm.option == "thisMonth")
                    {

                        result = result.Where(p => p.Date.Value.Month == month);
                    }
                    else if (vm.option == "lastMonth")
                    {
                        result = result.Where(p => p.Date.Value.Month == month - 1);
                    }
                    else if (vm.option == "thisYear")
                    {
                        result = result.Where(p => p.Date.Value.Year == year);

                    }
                    else if (vm.option == "lastYear")
                    {
                        result = result.Where(p => p.Date.Value.Year == year - 1);
                    }
                }

                if (!string.IsNullOrEmpty(vm.supplier))
                {
                    var value = Convert.ToInt32(vm.supplier);
                    //query here
                    result = result.Where(p => p.SupplierID == value);
                }

                if (vm.fromDate != null || vm.toDate != null)
                {
                    if (vm.fromDate != null && vm.toDate == null)
                    {
                        //query here
                        result = result.Where(p => p.Date >= vm.fromDate);
                    }
                    else if (vm.fromDate == null && vm.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.Date <= vm.fromDate);
                    }
                    else if (vm.fromDate != null && vm.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.Date >= vm.fromDate && p.Date <= vm.toDate);
                    }
                }

                if (!string.IsNullOrEmpty(vm.IsPaid))
                {
                    //query here
                    if (vm.IsPaid == "paid")
                    {
                        result = result.Where(p => p.IsPaid == "1");
                    }
                    else
                    {
                        result = result.Where(p => p.IsPaid == "0");
                    }

                }
            }
            return result;
        }
    }
}
