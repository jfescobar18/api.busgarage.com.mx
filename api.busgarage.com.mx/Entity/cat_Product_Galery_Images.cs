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
    
    public partial class cat_Product_Galery_Images
    {
        public int Product_Galery_Image_Id { get; set; }
        public int Product_Id { get; set; }
        public string Product_Galery_Image_Img { get; set; }
        public int Product_Galery_Image_Order { get; set; }
    
        public virtual cat_Products cat_Products { get; set; }
    }
}