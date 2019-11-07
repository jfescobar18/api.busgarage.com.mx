using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.busgarage.com.mx.Models
{
    public class KartProducts
    {
        public int Product_Id { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Product_Kart_Id { get; set; }
    }
}