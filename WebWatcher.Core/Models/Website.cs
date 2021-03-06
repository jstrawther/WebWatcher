using System.Collections.Generic;

namespace WebWatcher.Core.Models
{
    public class Website
    {
        public int Id { get; set; }

        public string DisplayUrl { get; set; }

        public string ContentUrl { get; set; }

        public string ElementSelector { get; set; }

        public List<WebsiteSnapshot> Snapshots { get; set; } = new List<WebsiteSnapshot>();

        public List<WebsiteSubscriber> Subscribers { get; set; } = new List<WebsiteSubscriber>();
    }
}

