using System.Threading.Tasks;
using WebWatcher.Core.EmailClient;
using WebWatcher.Core.Models;

namespace WebWatcher.Core.Notifications
{
    public class Notifier : INotifier
    {
        private readonly IEmailClient _emailClient;

        public Notifier(IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        public async Task NotifySubscribersAsync(Website website, WebsiteSnapshot snapshot)
        {
            foreach(var subscriber in website.Subscribers)
            {
                var from = "WebWatcher <no-reply@jstrawther.ddns.net>"; // todo: get this from a config file or something
                var to = subscriber.EmailAddress;
                var subject = $"WebWatcher Notification :: Website Updated ";
                var body = $"The following website has been updated: {website.Url}";
                await _emailClient.SendAsync(from, to, subject, body);
            }
        }
    }
}
