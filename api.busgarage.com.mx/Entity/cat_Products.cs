//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace api.busgarage.com.mx.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class cat_Products
    {
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal Product_Price { get; set; }
        public Nullable<decimal> Product_Disscount { get; set; }
        public Nullable<int> Category_Id { get; set; }
        public string Product_Img { get; set; }
        public string Product_Description { get; set; }
        public string Product_Configurations { get; set; }
        public Nullable<System.DateTime> Product_Creation_Date { get; set; }
        public int Product_Stock { get; set; }
    
        public virtual cat_Categories cat_Categories { get; set; }
    }
}