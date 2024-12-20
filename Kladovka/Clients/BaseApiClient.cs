using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Kladovka.Clients
{
    public class BaseApiClient
    {
        public BaseApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
        protected HttpClient HttpClient { get; }
        
        protected async Task PostAsync(string url, object body, CancellationToken cancellationToken)
        {
            try
            {
                using var respose = HttpClient.SendAsync(CreatePostRequest(url, body));
                if (!respose.Result.IsSuccessStatusCode)
                {
                    var error = await respose.Result.Content.ReadAsStringAsync(cancellationToken);
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error while sending post request on {url}, {ex.Message}", ex);
            }
        }

        private void FillRequest(HttpRequestMessage message)
        {
            var auth = CreateAuhenticationHeaders();
            if(auth != null)
            {
                message.Headers.Authorization = auth;
            }
        }
        protected virtual AuthenticationHeaderValue? CreateAuhenticationHeaders()
        {
            return null;
        }
        protected HttpRequestMessage CreatePostRequest(string url, object body)
        {
            var message = CreateRequest(url, body, HttpMethod.Post);
            return message;

        }
        protected HttpRequestMessage CreateRequest(string url, object body, HttpMethod method)
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            FillRequest(message);
            return message;
        }
    }
}
