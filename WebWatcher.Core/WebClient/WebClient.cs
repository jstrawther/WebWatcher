using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebWatcher.Core.Models;

namespace WebWatcher.Core.WebClient
{
    public class WebClient : IWebClient
    {
        private readonly HttpClient _httpClient;

        public WebClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // TODO: return a processing result object with errors/warnings.
        // Warn if xpath expression fails to match element.
        public async Task<string> GetContentAsync(Website website)
        {
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
            var urlToFetch = website.ContentUrl ?? website.DisplayUrl;
            var response = await _httpClient.GetAsync(urlToFetch);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // filter by optional element selector xpath
            if (!string.IsNullOrEmpty(website.ElementSelector))
            {
                var filteredContent = HtmlParser.SelectNode(content, website.ElementSelector);
                if (!string.IsNullOrEmpty(filteredContent))
                {
                    content = filteredContent;
                }
            }

            return content;            
        }
    }
}

