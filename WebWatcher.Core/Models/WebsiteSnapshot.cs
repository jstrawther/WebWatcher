using System;
namespace WebWatcher.Core.Models
{
    public class WebsiteSnapshot
    {
        public WebsiteSnapshot()
        {
        }

        public WebsiteSnapshot(Website website, string currentContent)
        {
            this.WebsiteId = website.Id;
            this.Content = currentContent;
            this.DateCreated = DateTime.Now;
        }

        public int Id { get; set; }

        public int WebsiteId { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public Website Website { get; set; }
    }
}
