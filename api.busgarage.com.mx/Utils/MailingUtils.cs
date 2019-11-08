using api.busgarage.com.mx;
using api.busgarage.com.mx.Entity;
using api.busgarage.com.mx.Models;
using api.busgarage.com.mx.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utils
{
    public class MailingUtils
    {
        public static async void SendOrderEmail(bool orderWasPaid, List<KartProducts> kartProducts, string orderNumber, string paymentReference, string To, string Subject)
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(orderWasPaid ? "~/Mailing/orderConfirmedEmail.html" : "~/Mailing/paymentOderEmail.html")))
            {
                string body = reader.ReadToEnd();

                string Products = String.Empty;
                foreach (var product in kartProducts)
                {
                    string ProductName = entity.cat_Products.Where(x => x.Product_Id == product.Product_Id).Select(x => x.Product_Name).FirstOrDefault();
                    string Price = String.Format("{0:C}", product.Price);

                    Products += $"<li style='text-align: left;'>{ProductName} {Price}</li>";
                }
                string Total = String.Format("{0:C}", kartProducts.Sum(x => x.Price));
                string OrderURL = $"https://sandbox-dashboard.openpay.mx/paynet-pdf/{OpenpayResources.MerchantID}/{paymentReference}";

                body = body.Replace("{orderNumber}", orderNumber);
                body = body.Replace("{Products}", Products);
                body = body.Replace("{Total}", Total);
                body = body.Replace("{OrderURL}", OrderURL);

                SendEmail(body, To, Subject);

                await Task.CompletedTask;
            }
        }

        public static async void SendTrackingIdEmail(string To, string TrackingId, string ShippingService)
        {
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Mailing/trackingIdEmail.html")))
            {
                string body = reader.ReadToEnd();
                string TrackingURL = string.Empty;

                switch (ShippingService.ToLower())
                {
                    case "fedex":
                        TrackingURL = $"<a class=\"mcnButton\" title=\"Rastrear Envío\" href=\"https://www.fedex.com/apps/fedextrack/index.html?tracknumbers={TrackingId}&cntry_code=mx\" target=\"_blank\" style=\"font-weight: bold;letter-spacing: normal;line-height: 100%;text-align: center;text-decoration: none;color: #FFFFFF;\">Rastrear Envío</a>";
                        break;
                    case "estafeta":
                        TrackingURL = $"<a class=\"mcnButton\" title=\"Rastrear Envío\" href=\"https://www.estafeta.com/Herramientas/Rastreo\" target=\"_blank\" style=\"font-weight: bold;letter-spacing: normal;line-height: 100%;text-align: center;text-decoration: none;color: #FFFFFF;\">Rastrear Envío</a>";
                        break;
                    default:
                        TrackingURL = $"<a class=\"mcnButton\" title=\"Rastrear Envío\" href=\"#\" target=\"_blank\" style=\"font-weight: bold;letter-spacing: normal;line-height: 100%;text-align: center;text-decoration: none;color: #FFFFFF;\">Rastrear Envío</a>";
                        break;
                }

                body = body.Replace("{TrackingId}", TrackingId);
                body = body.Replace("{ShippingService}", ShippingService);
                body = body.Replace("{TrackingURL}", TrackingURL);

                SendEmail(body, To, "Número de guía");
            }

            await Task.CompletedTask;
        }

        private static void SendEmail(string bodyEmail, string To, string Subject)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = EmailResources.Host;
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(EmailResources.Username, EmailResources.Password);

            MailMessage mail = new MailMessage(EmailResources.Username, To);
            mail.IsBodyHtml = true;
            mail.AlternateViews.Add(alternateView(bodyEmail));
            mail.BodyEncoding = Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.From = new MailAddress(EmailResources.Username, @"Busgarage 🏁");
            mail.Subject = Subject;
            mail.Body = bodyEmail;
            client.Send(mail);
        }

        private static AlternateView alternateView(string bodyEmail)
        {
            AlternateView vw = AlternateView.CreateAlternateViewFromString(bodyEmail, null, System.Net.Mime.MediaTypeNames.Text.Html);

            LinkedResource logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Mailing/img/logo.png"), System.Net.Mime.MediaTypeNames.Image.Jpeg);
            LinkedResource instagram = new LinkedResource(HttpContext.Current.Server.MapPath("~/Mailing/img/instagram.png"), System.Net.Mime.MediaTypeNames.Image.Jpeg);
            LinkedResource facebook = new LinkedResource(HttpContext.Current.Server.MapPath("~/Mailing/img/facebook.png"), System.Net.Mime.MediaTypeNames.Image.Jpeg);
            LinkedResource website = new LinkedResource(HttpContext.Current.Server.MapPath("~/Mailing/img/website.png"), System.Net.Mime.MediaTypeNames.Image.Jpeg);
            LinkedResource email = new LinkedResource(HttpContext.Current.Server.MapPath("~/Mailing/img/email.png"), System.Net.Mime.MediaTypeNames.Image.Jpeg);
            LinkedResource youtube = new LinkedResource(HttpContext.Current.Server.MapPath("~/Mailing/img/youtube.png"), System.Net.Mime.MediaTypeNames.Image.Jpeg);

            logo.ContentId = "logo";
            instagram.ContentId = "instagram";
            facebook.ContentId = "facebook";
            website.ContentId = "website";
            email.ContentId = "email";
            youtube.ContentId = "youtube";

            vw.LinkedResources.Add(logo);
            vw.LinkedResources.Add(instagram);
            vw.LinkedResources.Add(facebook);
            vw.LinkedResources.Add(website);
            vw.LinkedResources.Add(email);
            vw.LinkedResources.Add(youtube);

            return vw;
        }
    }
}