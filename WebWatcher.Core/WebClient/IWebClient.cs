using System;
using System.Threading.Tasks;
using WebWatcher.Core.Models;

namespace WebWatcher.Core.WebClient
{
    public interface IWebClient
    {
        Task<string> GetContentAsync(Website website);
    }
}
