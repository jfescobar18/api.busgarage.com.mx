using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.busgarage.com.mx.Models
{
    public class PaymentOrder
    {
        public string JsonOrder { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Method { get; set; }
        public string TokenId { get; set; }
        public decimal Amount { get; set; }
        public string DeviceSessionId { get; set; }
        public string UseCardPoints { get; set; }
    }
}