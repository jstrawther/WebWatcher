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

        public async Task CheckAllWebsitesAsync()
        {
            // Get all websites 
            var websites = GetAllWebsites();

            // Process each website
            foreach(var website in websites)
            {
                // Get the current content of the website.
                var currentContent = await _webClient.GetContentAsync(website);

                // Get the latest snapshot and compare it to the current content.
                // If they don't match, save a new snapshot and notify subscribers.
                var latestSnapshot = _dataAccess.GetLatestSnapshot(website);
                if (latestSnapshot?.Content != currentContent)
                {
                    var snapshot = new WebsiteSnapshot(website, currentContent);
                    _dataAccess.AddSnapshot(snapshot);
                    await _notifier.NotifySubscribersAsync(website, snapshot);
                }
            }
        }

        public IEnumerable<Website> GetAllWebsites()
        {
            return _dataAccess.GetAllWebsites();
        }

        public int AddWebsiteToWatch(string websiteUrlToAdd, string elementSelector, string emailToNotify)
        {
            var website = new Website()
            {
                Url = websiteUrlToAdd,
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
    }
}
