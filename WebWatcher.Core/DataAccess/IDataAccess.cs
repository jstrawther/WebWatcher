using System;
using System.Collections.Generic;
using WebWatcher.Core.Models;

namespace WebWatcher.Core.DataAccess
{
    public interface IDataAccess
    {
        IEnumerable<Website> GetAllWebsites();

        Website GetWebsiteById(int websiteId);

        Website AddWebsite(Website website);

        void DeleteWebsite(Website website);

        WebsiteSnapshot GetLatestSnapshot(Website website);

        WebsiteSnapshot AddSnapshot(WebsiteSnapshot snapshot);

        WebsiteSubscriber AddSubscriber(WebsiteSubscriber subscriber);

    }
}
