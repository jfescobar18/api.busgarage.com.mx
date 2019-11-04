using api.busgarage.com.mx.Entity;
using Newtonsoft.Json;
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
    public class AdminContentController : ApiController
    {
        #region Slider Images
        [HttpPost]
        [Route("AdminContent/AddSliderImage")]
        public async Task<HttpResponseMessage> AddSliderImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            List<string> filenames = new List<string>();

            try
            {
                FileUtils.UploadImage(HttpContext.Current.Request, "SliderImages", ref statusCode, ref dict, ref filenames);

                var sliderImage = new cat_Slider_Images()
                {
                    Slider_Image_Img = "SliderImages/" + filenames[0]
                };

                entity.cat_Slider_Images.Add(sliderImage);
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
        [Route("AdminContent/UpdateSliderImage")]
        public async Task<HttpResponseMessage> UpdateSliderImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            List<string> filenames = new List<string>();

            try
            {
                string jsonString = HttpContext.Current.Request.Form[0];
                cat_Slider_Images json = JsonConvert.DeserializeObject<cat_Slider_Images>(jsonString);

                var sliderImage = entity.cat_Slider_Images.SingleOrDefault(x => x.Slider_Image_Id == json.Slider_Image_Id);

                FileUtils.ReplaceFile(sliderImage.Slider_Image_Img, HttpContext.Current.Request, "SliderImages", ref statusCode, ref dict, ref filenames);

                sliderImage.Slider_Image_Img = "SliderImages/" + filenames[0];
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
        [Route("AdminContent/DeleteSliderImage")]
        public async Task<HttpResponseMessage> DeleteSliderImage([FromBody]cat_Slider_Images json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var sliderImage = entity.cat_Slider_Images.SingleOrDefault(x => x.Slider_Image_Id == json.Slider_Image_Id);

                FileUtils.DeleteFile(sliderImage.Slider_Image_Img);

                entity.cat_Slider_Images.Remove(sliderImage);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Slider image deleted successfully");

            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpGet]
        [Route("AdminContent/GetSliderImages")]
        public async Task<List<cat_Slider_Images>> GetSliderImages()
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();

            await Task.CompletedTask;
            return entity.cat_Slider_Images.ToList();
        }
        #endregion

        #region About Us Sections
        [HttpPost]
        [Route("AdminContent/AddAboutUsSection")]
        public async Task<HttpResponseMessage> AddAboutUsSection([FromBody] cat_About_Us_Sections json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var aboutUsSection = new cat_About_Us_Sections()
                {
                    About_Us_Section_Title = json.About_Us_Section_Title,
                    About_Us_Section_Content = json.About_Us_Section_Content
                };

                entity.cat_About_Us_Sections.Add(aboutUsSection);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Section added successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/UpdateAboutUsSection")]
        public async Task<HttpResponseMessage> UpdateAboutUsSection([FromBody] cat_About_Us_Sections json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var aboutUsSection = entity.cat_About_Us_Sections.SingleOrDefault(x => x.About_Us_Section_Id == json.About_Us_Section_Id);

                aboutUsSection.About_Us_Section_Title = json.About_Us_Section_Title;
                aboutUsSection.About_Us_Section_Content = json.About_Us_Section_Content;
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Section updated successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpPost]
        [Route("AdminContent/DeleteAboutUsSection")]
        public async Task<HttpResponseMessage> DeleteAboutUsSection([FromBody] cat_About_Us_Sections json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var aboutUsSection = entity.cat_About_Us_Sections.SingleOrDefault(x => x.About_Us_Section_Id == json.About_Us_Section_Id);

                entity.cat_About_Us_Sections.Remove(aboutUsSection);
                entity.SaveChanges();

                statusCode = HttpStatusCode.OK;
                dict.Add("message", "Section deleted successfully");
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpGet]
        [Route("AdminContent/GetAboutUsSections")]
        public async Task<List<cat_About_Us_Sections>> GetAboutUsSections()
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();

            await Task.CompletedTask;
            return entity.cat_About_Us_Sections.ToList();
        }
        #endregion

        #region Offers Image
        [HttpPost]
        [Route("AdminContent/UpdateOffersImage")]
        public async Task<HttpResponseMessage> UpdateOffersImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            List<string> filenames = new List<string>();

            try
            {
                var offersImage = entity.cat_Offers_Image.SingleOrDefault(x => x.Offers_Banner_Id == 1);

                if(offersImage == null)
                {
                    FileUtils.UploadImage(HttpContext.Current.Request, "OfferImage", ref statusCode, ref dict, ref filenames);

                    offersImage = new cat_Offers_Image()
                    {
                        Offers_Banner_Img = "OfferImage/" + filenames[0]
                    };

                    entity.cat_Offers_Image.Add(offersImage);
                }
                else
                {
                    FileUtils.ReplaceFile(offersImage.Offers_Banner_Img, HttpContext.Current.Request, "OfferImage", ref statusCode, ref dict, ref filenames);

                    offersImage.Offers_Banner_Img = "OfferImage/" + filenames[0];
                }
                entity.SaveChanges();

            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }

        [HttpGet]
        [Route("AdminContent/GetOffersImage")]
        public async Task<List<cat_Offers_Image>> GetOffersImage()
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            await Task.CompletedTask;
            return entity.cat_Offers_Image.ToList();
        }
        #endregion

        #region Sepomex
        [HttpGet]
        [Route("AdminContent/GetSuburbs/{zip}")]
        public async Task<List<vw_Sepomex_Info>> GetSuburbs(string zip)
        {
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            await Task.CompletedTask;

            var list = entity.vw_Sepomex_Info.Where(x => x.d_codigo == zip).Select(x => new {
                x.d_asenta,
                x.D_mnpio,
                x.d_estado,
                x.d_ciudad }).ToList();

            List<vw_Sepomex_Info> suburbList = list.Select(x => new vw_Sepomex_Info()
            {
                d_asenta = x.d_asenta,
                D_mnpio = x.D_mnpio,
                d_estado = x.d_estado,
                d_ciudad = x.d_ciudad
            }).ToList();

            return suburbList;
        }
        #endregion
    }
}
