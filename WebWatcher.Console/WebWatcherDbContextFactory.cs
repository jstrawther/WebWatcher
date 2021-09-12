using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebWatcher.EF;

namespace WebWatcher.Console
{
    public class WebWatcherDbContextFactory : IDesignTimeDbContextFactory<WebWatcherDbContext>
    {
        public WebWatcherDbContext CreateDbContext(string[] args)
        {
            var dbContextOptions = new DbContextOptionsBuilder<WebWatcherDbContext>()
                .UseSqlite("Data Source=WebWatcher.sqlite", b => b.MigrationsAssembly("WebWatcher.Console"))
                .Options;

            var dbContext = new WebWatcherDbContext(dbContextOptions);

            return dbContext;
        }
    }
}
