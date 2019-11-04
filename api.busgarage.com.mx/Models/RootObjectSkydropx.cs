using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.busgarage.com.mx.Models
{
    public class RootObjectSkydropx
    {
        public AddressFromSkydropx address_from { get; set; }
        public List<ParcelSkydropx> parcels { get; set; }
        public AddressToSkydropx address_to { get; set; }
    }
}