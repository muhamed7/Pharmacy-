//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pharmacy.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SalesItem
    {
        public int ID { get; set; }
        public Nullable<int> StockID { get; set; }
        public Nullable<int> SalesID { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Amount { get; set; }
    
        public virtual Sale Sale { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
