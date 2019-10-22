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
    public class AdminCredentialsController : ApiController
    {
        [HttpPost]
        [Route("AdminCredentials/Login")]
        public async Task<HttpResponseMessage> Login([FromBody] cat_Admin_Login json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CMS_BusgarageEntities entity = new CMS_BusgarageEntities();
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            try
            {
                var hash512 = CredentialUtils.SHA512(json.Admin_Login_Password);
                var adminUser = entity.cat_Admin_Login.SingleOrDefault(x => x.Admin_Login_Username == json.Admin_Login_Username && x.Admin_Login_Password == hash512);
                statusCode = HttpStatusCode.OK;

                if (adminUser != null)
                {
                    dict.Add("message", adminUser.Admin_Login_Id);
                }
                else
                {
                    dict.Add("message", 0);
                }
            }
            catch (Exception ex)
            {
                dict.Add("message", ex.Message);
            }

            await Task.CompletedTask;
            return Request.CreateResponse(statusCode, dict);
        }
    }
}
