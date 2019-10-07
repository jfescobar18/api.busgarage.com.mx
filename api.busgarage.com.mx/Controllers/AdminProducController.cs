using api.busgarage.com.mx.Entity;
using System;
using System.Collections.Generic;
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
    public class AdminProducController : ApiController
    {
        #region Categories
        [HttpPost]
        [Route("AdminContent/AddCategory")]
        public async Task<HttpResponseMessage> AddCategory([FromBody] cat_Categories json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var category = new cat_Categories()
                {
                    Category_Name = json.Category_Name
                };

                entity.cat_Categories.Add(category);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Category added successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/UpdateCategory")]
        public async Task<HttpResponseMessage> UpdateCategory([FromBody] cat_Categories json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var category = entity.cat_Categories.SingleOrDefault(x => x.Category_Id == json.Category_Id);

                category.Category_Name = json.Category_Name;
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Category updated successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/DeleteCategory")]
        public async Task<HttpResponseMessage> DeleteCategory([FromBody] int Category_Id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var category = entity.cat_Categories.SingleOrDefault(x => x.Category_Id == Category_Id);

                entity.cat_Categories.Remove(category);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Category deleted successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpGet]
        [Route("AdminContent/GetCategories")]
        public async Task<List<cat_Categories>> GetCategories()
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();

            await Task.CompletedTask;
            return entity.cat_Categories.ToList();
        }
        #endregion

        #region Products
        [HttpPost]
        [Route("AdminContent/AddProduct")]
        public async Task<HttpResponseMessage> AddProduct([FromBody] cat_Products json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            List<string> filenames = new List<string>();

            try
            {
                FileUtils.UploadImage(HttpContext.Current.Request, "ProductImages", ref statusCode, ref dict, ref filenames);

                var product = new cat_Products()
                {
                    Product_Name = json.Product_Name,
                    Product_Price = json.Product_Price,
                    Product_Disscount = json.Product_Disscount,
                    Category_Id = json.Category_Id,
                    Product_Img = filenames[0],
                    Product_Description = json.Product_Description,
                    Product_Configurations = json.Product_Configurations,
                    Product_Creation_Date = DateTime.Now,
                    Product_Stock = json.Product_Stock
                };

                entity.cat_Products.Add(product);
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/UpdateProduct")]
        public async Task<HttpResponseMessage> UpdateProduct([FromBody] cat_Products json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            List<string> filenames = new List<string>();

            try
            {
                var product = entity.cat_Products.SingleOrDefault(x => x.Product_Id == json.Product_Id);

                if(HttpContext.Current.Request.Files.Count > 0)
                {
                    FileUtils.ReplaceFile(product.Product_Img, HttpContext.Current.Request, "ProductImages", ref statusCode, ref dict, ref filenames);
                    product.Product_Img = filenames[0];
                }

                product.Product_Name = json.Product_Name;
                product.Product_Price = json.Product_Price;
                product.Product_Disscount = json.Product_Disscount;
                product.Category_Id = json.Category_Id;
                product.Product_Description = json.Product_Description;
                product.Product_Configurations = json.Product_Configurations;
                product.Product_Creation_Date = DateTime.Now;
                product.Product_Stock = json.Product_Stock;
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/DeleteProduct")]
        public async Task<HttpResponseMessage> DeleteProduct([FromBody] int Product_Id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var product = entity.cat_Products.SingleOrDefault(x => x.Product_Id == Product_Id);

                FileUtils.DeleteFile(product.Product_Img);

                entity.cat_Products.Remove(product);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Product deleted successfully");

            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpGet]
        [Route("AdminContent/GetProducts")]
        public async Task<List<cat_Products>> GetProducts()
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();

            await Task.CompletedTask;
            return entity.cat_Products.ToList();
        }
        #endregion

        #region Kart
        [HttpPost]
        [Route("AdminContent/AddKart")]
        public async Task<HttpResponseMessage> AddKart([FromBody] cat_Karts json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var kart = new cat_Karts()
                {
                    Kart_Json_Config = json.Kart_Json_Config,
                    Kart_Creation_Date = DateTime.Now
                };

                entity.cat_Karts.Add(kart);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Kart added successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/UpdateKart")]
        public async Task<HttpResponseMessage> UpdateKart([FromBody] cat_Karts json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var kart = entity.cat_Karts.SingleOrDefault(x => x.Kart_Id == json.Kart_Id);

                kart.Kart_Json_Config = json.Kart_Json_Config;
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Kart updated successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/DeleteKart")]
        public async Task<HttpResponseMessage> DeleteKart([FromBody] int Kart_Id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var kart = entity.cat_Karts.SingleOrDefault(x => x.Kart_Id == Kart_Id);

                entity.cat_Karts.Remove(kart);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Kart deleted successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpGet]
        [Route("AdminContent/GetKart/{Kart_Id}")]
        public async Task<List<cat_Karts>> GetKart(int Kart_Id)
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();

            await Task.CompletedTask;
            return entity.cat_Karts.Where(x => x.Kart_Id == Kart_Id).ToList();
        }
        #endregion

        #region Orders
        [HttpPost]
        [Route("AdminContent/AddOrder")]
        public async Task<HttpResponseMessage> AddOrder([FromBody] cat_Orders json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var order = new cat_Orders()
                {
                    Kart_Id = json.Kart_Id,
                    Order_Client_Name = json.Order_Client_Name,
                    Order_Client_Email = json.Order_Client_Email,
                    Order_Client_Phone = json.Order_Client_Phone,
                    Order_Client_Address1 = json.Order_Client_Address1,
                    Order_Client_Address2 = json.Order_Client_Address2,
                    Order_Client_Province = json.Order_Client_Province,
                    Order_Client_City = json.Order_Client_City,
                    Order_Client_Zip = json.Order_Client_Zip,
                    Order_Client_Comments = json.Order_Client_Comments,
                    Order_Creation_Date = DateTime.Now,
                    Order_Delivered_Date = json.Order_Delivered_Date
                };

                entity.cat_Orders.Add(order);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Order added successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/UpdateOrder")]
        public async Task<HttpResponseMessage> UpdateOrder([FromBody] cat_Orders json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var order = entity.cat_Orders.SingleOrDefault(x => x.Order_Id == json.Order_Id);

                order.Kart_Id = json.Kart_Id;
                order.Order_Client_Name = json.Order_Client_Name;
                order.Order_Client_Email = json.Order_Client_Email;
                order.Order_Client_Phone = json.Order_Client_Phone;
                order.Order_Client_Address1 = json.Order_Client_Address1;
                order.Order_Client_Address2 = json.Order_Client_Address2;
                order.Order_Client_Province = json.Order_Client_Province;
                order.Order_Client_City = json.Order_Client_City;
                order.Order_Client_Zip = json.Order_Client_Zip;
                order.Order_Client_Comments = json.Order_Client_Comments;
                order.Order_Creation_Date = DateTime.Now;
                order.Order_Delivered_Date = json.Order_Delivered_Date;
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Order updated successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/DeleteOrder")]
        public async Task<HttpResponseMessage> DeleteOrder([FromBody] int Order_Id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var order = entity.cat_Orders.SingleOrDefault(x => x.Order_Id == Order_Id);

                entity.cat_Orders.Remove(order);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Order deleted successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpGet]
        [Route("AdminContent/GetOders")]
        public async Task<List<cat_Orders>> GetOders()
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();

            await Task.CompletedTask;
            return entity.cat_Orders.ToList();
        }
        #endregion

        #region Product_Galery_Images
        [HttpPost]
        [Route("AdminContent/AddProductGaleryImage")]
        public async Task<HttpResponseMessage> AddProductGaleryImage([FromBody] cat_Product_Galery_Images json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            List<string> filenames = new List<string>();

            try
            {
                FileUtils.UploadImage(HttpContext.Current.Request, "ProductGaleryImageImages", ref statusCode, ref dict, ref filenames);

                var productGaleryImage = new cat_Product_Galery_Images()
                {
                    Product_Id = json.Product_Id,
                    Product_Galery_Image_Img = filenames[0],
                    Product_Galery_Image_Order = json.Product_Galery_Image_Order
                };

                entity.cat_Product_Galery_Images.Add(productGaleryImage);
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/UpdateProductGaleryImage")]
        public async Task<HttpResponseMessage> UpdateProductGaleryImage([FromBody] cat_Product_Galery_Images json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            List<string> filenames = new List<string>();

            try
            {
                var productGaleryImage = entity.cat_Product_Galery_Images.SingleOrDefault(x => x.Product_Galery_Image_Id == json.Product_Galery_Image_Id);

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    FileUtils.ReplaceFile(productGaleryImage.Product_Galery_Image_Img, HttpContext.Current.Request, "ProductGaleryImageImages", ref statusCode, ref dict, ref filenames);
                    productGaleryImage.Product_Galery_Image_Img = filenames[0];
                }

                productGaleryImage.Product_Galery_Image_Order = json.Product_Galery_Image_Order;
                entity.SaveChanges();
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/DeleteProductGaleryImage")]
        public async Task<HttpResponseMessage> DeleteProductGaleryImage([FromBody] int Product_Galery_Image_Id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var productGaleryImage = entity.cat_Product_Galery_Images.SingleOrDefault(x => x.Product_Galery_Image_Id == Product_Galery_Image_Id);

                FileUtils.DeleteFile(productGaleryImage.Product_Galery_Image_Img);

                entity.cat_Product_Galery_Images.Remove(productGaleryImage);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Product galery image deleted successfully");

            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpGet]
        [Route("AdminContent/GetProductGaleryImages/{Product_Galery_Image_Id}")]
        public async Task<List<cat_Product_Galery_Images>> GetProductGaleryImages(int Product_Galery_Image_Id)
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();

            await Task.CompletedTask;
            return entity.cat_Product_Galery_Images.Where(x => x.Product_Galery_Image_Id == Product_Galery_Image_Id).ToList();
        }
        #endregion
    }
}
