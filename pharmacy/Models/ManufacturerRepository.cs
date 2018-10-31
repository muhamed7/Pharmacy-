using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pharmacy.Models
{
    public class ManufacturerRepository
    {
        Models.medicalEntities db = new medicalEntities();
        public int ManufacturerDuplicationCheck(Manufacturer manufacturer)
        {
            List<Manufacturer> _manufacturer = (from m in db.Manufacturer
                                                where m.ManufacturerName == manufacturer.ManufacturerName
                                                select m).ToList();
            return _manufacturer.Count;
        }
    }
}