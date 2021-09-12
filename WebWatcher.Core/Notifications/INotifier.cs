using System.Threading.Tasks;
using WebWatcher.Core.Models;

namespace WebWatcher.Core.Notifications
{
    public interface INotifier
    {
        Task NotifySubscribersAsync(Website website, WebsiteSnapshot snapshot);
    }
}
