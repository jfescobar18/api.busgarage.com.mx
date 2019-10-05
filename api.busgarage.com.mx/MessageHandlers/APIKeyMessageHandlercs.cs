using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace MessageHandlers
{
    public class APIKeyMessageHandlercs : DelegatingHandler
    {
        private const string APIKeyToCheck = "i0WKPFVfNDETs2RsSeo0Tj3OpjYkOSX4";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            IEnumerable<string> requestHeaders;

            bool checkAPIKeyExists = httpRequestMessage.Headers.TryGetValues("APIKey", out requestHeaders);

            bool validKey = checkAPIKeyExists ? requestHeaders.FirstOrDefault().Equals(APIKeyToCheck) : false;

            if (!validKey)
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden, "Invalid API Key");
            }

            var response = await base.SendAsync(httpRequestMessage, cancellationToken);
            return response;
        }
    }
}