using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BK.HMACAuthentication
{
    public class BadgeKeeperWebClient
    {
        private HttpClient WebClient = null;

        public BadgeKeeperWebClient(string appId, string appKey)
        {
            WebClient = MakeHttpClient(appId, appKey);
        }

        public async Task<string> MakeGetRequest(string path)
        {
            try
            {
                string response = await WebClient.GetStringAsync(path);
                return MakeSuccessResponse(response);
            }
            catch (System.Exception e)
            {
                string error = e.Message + (e.InnerException == null ? "" : e.InnerException.Message);
                return MakeErrorResponse(error);
            }
        }

        public async Task<string> MakePostRequest(string path, string body)
        {
            try
            {
                string result = string.Empty;
                var response = await WebClient.PostAsync(path, new StringContent(body, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    result = await response.Content.ReadAsStringAsync();
                    return MakeSuccessResponse(result);
                }
                else
                {
                    string error = "Error occurred, the status code is: " + response.StatusCode;
                    return MakeErrorResponse(error);
                }
            }
            catch (System.Exception e)
            {
                string error = e.Message + (e.InnerException == null ? "" : e.InnerException.Message);
                return MakeErrorResponse(error);
            }
        }

        private HttpClient MakeHttpClient(string appId, string appKey)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HMACDelegateHandler customDelegatingHandler = new HMACDelegateHandler(appId, appKey);
            HttpClient client = HttpClientFactory.Create(customDelegatingHandler);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private string MakeErrorResponse(string error)
        {
            string result = "{ \"isError\": true, \"errorMessage\": \"" + error + "\", \"result\": null }";
            return result;
        }

        private string MakeSuccessResponse(string response)
        {
            string result = "{ \"isError\": false, \"errorMessage\": null, \"result\": " + response + " }";
            return result;
        }
    }
}
