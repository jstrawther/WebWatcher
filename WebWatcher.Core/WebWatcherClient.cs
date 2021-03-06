using System.Collections.Generic;
using System.Threading.Tasks;
using WebWatcher.Core.DataAccess;
using WebWatcher.Core.Models;
using WebWatcher.Core.Notifications;
using WebWatcher.Core.WebClient;

namespace WebWatcher.Core
{
    public class WebWatcherClient
    {
        private readonly IDataAccess _dataAccess;
        private readonly IWebClient _webClient;
        private readonly INotifier _notifier;

        public WebWatcherClient(
            IDataAccess dataAccess,
            IWebClient webClient,
            INotifier notifier)
        {
            _dataAccess = dataAccess;
            _webClient = webClient;
            _notifier = notifier;
        }

        public async Task CheckAllWebsitesAsync(bool sendNotifications)
        {
            // Get all websites 
            var websites = GetAllWebsites();

            // Process each website
            foreach(var website in websites)
            {
                // determine url to check
                var url = website.ContentUrl ?? website.DisplayUrl;

                // Get the current content of the website.
                var currentContent = await _webClient.GetAsync(url);

                // filter by optional element selector xpath
                if (!string.IsNullOrEmpty(website.ElementSelector))
                {
                    var filteredContent = HtmlParser.SelectNode(currentContent, website.ElementSelector);

                    // TODO: warn if HtmlParser fails to find content with ElementSelector

                    if (!string.IsNullOrEmpty(filteredContent))
                    {
                        currentContent = filteredContent;
                    }
                }

                // Get the latest snapshot and compare it to the current content.
                // If they don't match, save a new snapshot and notify subscribers.
                var latestSnapshot = _dataAccess.GetLatestSnapshot(website);
                if (latestSnapshot?.Content != currentContent)
                {
                    var snapshot = new WebsiteSnapshot(website, currentContent);
                    _dataAccess.AddSnapshot(snapshot);

                    if(sendNotifications)
                    {
                        await _notifier.NotifySubscribersAsync(website, snapshot);
                    }
                }
            }
        }

        public IEnumerable<Website> GetAllWebsites()
        {
            return _dataAccess.GetAllWebsites();
        }

        public Website GetWebsiteById(int id)
        {
            return _dataAccess.GetWebsiteById(id);
        }

        public int AddWebsiteToWatch(string websiteUrlToAdd, string contentUrl, string elementSelector, string emailToNotify)
        {
            var website = new Website()
            {
                DisplayUrl = websiteUrlToAdd,
                ContentUrl = contentUrl,
                ElementSelector = elementSelector,
                Subscribers = new List<WebsiteSubscriber>()
                {
                    new WebsiteSubscriber
                    {
                        EmailAddress = emailToNotify
                    }
                }
            };
            _dataAccess.AddWebsite(website);

            return website.Id != 0 ? 0 : 1;
        }

        public int AddEmailToNotify(int websiteId, string emailToNotify)
        {
            var website = _dataAccess.GetWebsiteById(websiteId);
            if(website == null)
            {
                return 1;
            }

            var subscriber = _dataAccess.AddSubscriber(new WebsiteSubscriber
            {
                WebsiteId = website.Id,
                EmailAddress = emailToNotify
            });

            return subscriber.Id != 0 ? 0 : 1;
        }

        public void DeleteWebsite(Website website)
        {
            _dataAccess.DeleteWebsite(website);
        }
    }
}
