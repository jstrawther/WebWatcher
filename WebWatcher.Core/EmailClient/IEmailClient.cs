using System.Threading.Tasks;

namespace WebWatcher.Core.EmailClient
{
    public interface IEmailClient
    {
        Task SendAsync(string from, string to, string subject, string body);
    }
}
