using Microsoft.EntityFrameworkCore;
using WebWatcher.Core.Models;

namespace WebWatcher.EF
{
    public class WebWatcherDbContext : DbContext
    {
        public WebWatcherDbContext(DbContextOptions<WebWatcherDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Website> Websites { get; set; }

        public DbSet<WebsiteSnapshot> WebsiteSnapshots { get; set; }

        public DbSet<WebsiteSubscriber> WebsiteSubscribers { get; set; }
    }
}
