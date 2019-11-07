using api.busgarage.com.mx.Entity;
using api.busgarage.com.mx.Models;
using api.busgarage.com.mx.Resources;
using Newtonsoft.Json;
using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Utils;

namespace api.busgarage.com.mx.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PaymentController : ApiController
    {
        [HttpPost]
        [Route("Payment/ProcessOrder")]
        public async Task<HttpResponseMessage> ProcessOrder([FromBody] PaymentOrder json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                OpenpayAPI api = new OpenpayAPI(OpenpayResources.OpenpayPrivateKey, OpenpayResources.MerchantID);

                Customer customer = new Customer();
                customer.Name = json.Name;
                customer.LastName = json.Name;
                customer.PhoneNumber = json.PhoneNumber;
                customer.Email = json.Email;

                ChargeRequest request = new ChargeRequest();
                request.Method = json.Method;
                request.SourceId = json.TokenId;
                request.Amount = json.Amount;
                request.Description = OpenpayResources.OpenpayDescription;
                request.DeviceSessionId = json.DeviceSessionId;
                request.Customer = customer;
                request.UseCardPoints = json.UseCardPoints ?? "false";

                Charge charge = api.ChargeService.Create(request);

                if (charge.ErrorMessage == null)
                {
                    List<KartProducts> kartProducts = new List<KartProducts>();
                    string orderNumber = String.Empty;
                    string paymentReference = json.Method == "store" ? charge.PaymentMethod.Reference : String.Empty;

                    AddOrder(json.JsonOrder, charge.Id, paymentReference, ref kartProducts, ref orderNumber);
                    statusCode = HttpStatusCode.OK;

                    MailingUtils.SendOrderEmail(charge.Status == "completed", kartProducts, orderNumber, paymentReference, json.Email, "Orden Busgarage");

                    dict.Add("message", "Order created successfully");
                    dict.Add("OpenpayResponse", charge);
                }
                else
                {
                    dict.Add("message", charge.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                if (dict.Keys.Count > 0)
                {
                    dict = new Dictionary<string, object>();
                }
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        public void AddOrder(string jsonString, string chargeId, string paymentReference, ref List<KartProducts> kartProducts, ref string orderNumber)
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();

            try
            {
                CheckoutOrder json = JsonConvert.DeserializeObject<CheckoutOrder>(jsonString);

                cat_Karts kart = new cat_Karts()
                {
                    Kart_Json_Config = json.Kart_Json,
                    Kart_Creation_Date = DateTime.Now
                };
                entity.cat_Karts.Add(kart);
                entity.SaveChanges();

                kartProducts = JsonConvert.DeserializeObject<List<KartProducts>>(json.Kart_Json);

                string exteriorNumber = json.Address_Number2.Length > 0 ? "Ext. " + json.Address_Number2 : "";
                var order = new cat_Orders()
                {
                    Kart_Id = kart.Kart_Id,
                    Order_Client_Name = json.Order_Client_Name,
                    Order_Client_Email = json.Order_Client_Email,
                    Order_Client_Phone = json.Order_Client_Phone,
                    Order_Client_Address1 = $"{json.Address_Street} Num. {json.Address_Number1} ",
                    Order_Client_Address2 = $"{exteriorNumber} Col. {json.Order_Client_Suburb}",
                    Order_Client_Province = json.Order_Client_Province,
                    Order_Client_City = json.Order_Client_City,
                    Order_Client_Zip = json.Order_Client_Zip,
                    Order_Client_Comments = json.Order_Client_Comments,
                    Order_Creation_Date = DateTime.Now,
                    Order_Delivered_Date = null,
                    Order_Openpay_ChargeId = chargeId,
                    Order_Tracking_Id = String.Empty
                };

                entity.cat_Orders.Add(order);
                entity.SaveChanges();

                orderNumber = $"{DateTime.Now.ToString("ddMMyyyy")}{order.Order_Id}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Obsolete("AddOrderToSkydropx is deprecated, please don't use until have access to production")]
        private async Task<string> AddOrderToSkydropx(cat_Orders order)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api-demo.srenvio.com/v1/shipments");
            request.Method = "Post";
            request.KeepAlive = true;
            request.ContentType = "appication/json";
            request.Headers.Add("Authorization", "Token TWeS6uzxmQZ1d4RiKk3wd3Jj2UhygINXvXNxNrmBWHct");

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                List<ParcelSkydropx> parcel = new List<ParcelSkydropx>()
                {
                    new ParcelSkydropx()
                    {
                        weight = 0.5F,
                        distance_unit = "CM",
                        mass_unit = "KG",
                        height = 29.7F,
                        width = 5F,
                        length = 21F
                    }
                };

                AddressFromSkydropx addressFrom = new AddressFromSkydropx()
                {
                    province = "CDMX",
                    city = "Benito Juarez",
                    name = "Francisco Escobar",
                    zip = "03700",
                    country = "Mexico",
                    address1 = "St 17",
                    company = "Busgarage",
                    address2 = "Col. Mixcoac",
                    phone = "5540732391",
                    email = "jfescobar18@hotmail.com"
                };

                AddressToSkydropx addressTo = new AddressToSkydropx()
                {
                    province = order.Order_Client_Province,
                    city = order.Order_Client_City,
                    name = order.Order_Client_Name,
                    zip = order.Order_Client_Zip,
                    country = "MXN",
                    address1 = order.Order_Client_Address1,
                    company = "-",
                    address2 = order.Order_Client_Address2,
                    phone = order.Order_Client_Phone,
                    email = order.Order_Client_Email,
                    references = order.Order_Client_Comments,
                    contents = "Productos Busgarage"
                };

                RootObjectSkydropx rootObject = new RootObjectSkydropx()
                {
                    address_from = addressFrom,
                    parcels = parcel,
                    address_to = addressTo
                };

                string json = JsonConvert.SerializeObject(rootObject);
                streamWriter.Write(json);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string myResponse = string.Empty;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                myResponse = sr.ReadToEnd();
            }

            await Task.CompletedTask;
            return myResponse;
        }
    }
}
