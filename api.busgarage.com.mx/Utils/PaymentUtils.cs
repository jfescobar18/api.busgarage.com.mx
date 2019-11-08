using api.busgarage.com.mx.Resources;
using Openpay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Utils
{
    public static class PaymentUtils
    {
        public static string GetPaymentStatus(string transaction_id)
        {
            try
            {
                OpenpayAPI api = new OpenpayAPI(OpenpayResources.OpenpayPrivateKey, OpenpayResources.MerchantID);
                var Charge = api.ChargeService.Get(transaction_id);
                return Charge.Status;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}