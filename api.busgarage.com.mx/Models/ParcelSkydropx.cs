using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.busgarage.com.mx.Models
{
    public class ParcelSkydropx
    {
        public float weight { get; set; }
        public string distance_unit { get; set; }
        public string mass_unit { get; set; }
        public float height { get; set; }
        public float width { get; set; }
        public float length { get; set; }
    }
}