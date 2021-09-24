using System.Net.Http;
using System.Threading.Tasks;

namespace WebWatcher.Core.WebClient
{
    public class WebClient : IWebClient
    {
        private readonly HttpClient _httpClient;

        public WebClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> GetAsync(string url)
        {
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }
    }
}

