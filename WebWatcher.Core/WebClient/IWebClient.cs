using System.Threading.Tasks;

namespace WebWatcher.Core.WebClient
{
    public interface IWebClient
    {
        Task<string> GetAsync(string url);
    }
}
