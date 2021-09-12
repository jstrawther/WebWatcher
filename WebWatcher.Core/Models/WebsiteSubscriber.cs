using System;
namespace WebWatcher.Core.Models
{
    public class WebsiteSubscriber
    {
        public int Id { get; set; }

        public int WebsiteId { get; set; }

        public string EmailAddress { get; set; }

        public Website Website { get; set; }
    }
}
