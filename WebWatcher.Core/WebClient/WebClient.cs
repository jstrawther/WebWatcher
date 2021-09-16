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
            var response = await _httpClient.GetAsync(website.Url);
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

