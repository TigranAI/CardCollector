using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CardCollector.Resources;

namespace CardCollector.Others
{
    public class RequestBuilder
    {
        private string _url = "";
        private Dictionary<string, string> _params = new ();

        public RequestBuilder SetUrl(string url)
        {
            _url = url;
            return this;
        }

        public RequestBuilder AddParam(string key, object value)
        {
            _params.Add(key, value.ToString() ?? "");
            return this;
        }

        public async Task<HttpStatusCode> Send()
        {
            HttpClient httpClient = Constants.DEBUG ? GetUnsafeHttpClient() : new HttpClient();
            var url = $"https://{AppSettings.SITE_URL}/{_url}";
            
            var content = new FormUrlEncodedContent(_params);
            var response = await httpClient.PostAsync(url, content);
            return response.StatusCode;
        }

        private HttpClient GetUnsafeHttpClient()
        {
            return new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = delegate { return true; }
            });
        }
    }
}