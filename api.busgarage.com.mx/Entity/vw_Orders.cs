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
    
    public partial class vw_Orders
    {
        public int Order_Id { get; set; }
        public int Kart_Id { get; set; }
        public string Kart_Json_Config { get; set; }
        public string Order_Client_Name { get; set; }
        public string Order_Client_Email { get; set; }
        public string Order_Client_Phone { get; set; }
        public string Order_Client_Address1 { get; set; }
        public string Order_Client_Address2 { get; set; }
        public string Order_Client_Province { get; set; }
        public string Order_Client_City { get; set; }
        public string Order_Client_Zip { get; set; }
        public string Order_Client_Comments { get; set; }
        public System.DateTime Order_Creation_Date { get; set; }
        public Nullable<System.DateTime> Order_Delivered_Date { get; set; }
        public string Order_Openpay_ChargeId { get; set; }
        public string Order_Payment_Status { get; set; }
        public string Order_Tracking_Id { get; set; }
    }
}
