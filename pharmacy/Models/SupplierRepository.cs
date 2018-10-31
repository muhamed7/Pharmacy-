using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pharmacy.Models
{
    public class SupplierRepository
    {
        Models.medicalEntities db = new medicalEntities();
        public int SupplierDuplicationCheck(Supplier supplier)
        {
            //check if the input supplier name already exists
            List<Supplier> _supplier = (from s in db.Supplier
                                        where s.Name == supplier.Name
                                        select s).ToList();
            return _supplier.Count;
        }
    }
}