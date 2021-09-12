using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebWatcher.Core.DataAccess;
using WebWatcher.Core.Models;

namespace WebWatcher.EF
{
    public class DataAccess : IDataAccess
    {
        private readonly WebWatcherDbContext _dbContext;

        public DataAccess(WebWatcherDbContext webWatcherDbContext)
        {
            this._dbContext = webWatcherDbContext;
        }

        public WebsiteSnapshot AddSnapshot(WebsiteSnapshot snapshot)
        {
            _dbContext.WebsiteSnapshots.Add(snapshot);
            _dbContext.SaveChanges();
            if (snapshot.Id == 0) throw new InvalidOperationException();
            return snapshot;
        }

        public WebsiteSubscriber AddSubscriber(WebsiteSubscriber subscriber)
        {
            _dbContext.WebsiteSubscribers.Add(subscriber);
            _dbContext.SaveChanges();
            if (subscriber.Id == 0) throw new InvalidOperationException();
            return subscriber;
        }

        public Website AddWebsite(Website website)
        {
            _dbContext.Websites.Add(website);
            _dbContext.SaveChanges();
            if (website.Id == 0) throw new InvalidOperationException();
            return website;
        }

        public IEnumerable<Website> GetAllWebsites()
        {
            return _dbContext.Websites.Include(x => x.Subscribers).ToList();
        }

        public WebsiteSnapshot GetLatestSnapshot(Website website)
        {
            return _dbContext.WebsiteSnapshots
                .Where(x => x.WebsiteId == website.Id)
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefault();
        }

        public Website GetWebsiteById(int websiteId)
        {
            return _dbContext.Websites.Find(websiteId);
        }
    }
}
