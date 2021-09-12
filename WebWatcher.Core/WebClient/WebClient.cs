using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebWatcher.Core.Models;

namespace WebWatcher.Core.WebClient
{
    public class WebClient : IWebClient
    {
        private readonly HttpClient _httpClient;

        public WebClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetContentAsync(Website website)
        {
            var response = await _httpClient.GetAsync(website.Url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            if(!string.IsNullOrEmpty(website.ElementSelector))
            {
                // TODO: filter by element selector
            }

            return content;            
        }
    }
}

