using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebWatcher.Core;
using WebWatcher.Core.DataAccess;
using WebWatcher.Core.EmailClient;
using WebWatcher.Core.Models;
using WebWatcher.Core.Notifications;
using WebWatcher.Core.WebClient;
using WebWatcher.EF;

namespace WebWatcher.Console
{
    [Command(Name = "WebWatcher", Description = "A utility to watch websites and send notifications when they change")]
    [HelpOption("-h")]
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await new HostBuilder()
                .ConfigureLogging((context, builder) =>
                {
                    //builder.AddConsole();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<IDataAccess, DataAccess>()
                            .AddDbContext<WebWatcherDbContext>(
                                options => options.UseSqlite(
                                    "Data Source=WebWatcher.sqlite", b => b.MigrationsAssembly("WebWatcher.Console")))
                            .AddScoped<IWebClient, WebClient>()
                            .AddHttpClient()
                            .AddScoped<INotifier, Notifier>()
                            .AddScoped<IEmailClient, EmailClient>(_ =>
                            {
                                var sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                                if (string.IsNullOrWhiteSpace(sendGridApiKey))
                                {
                                    throw new InvalidOperationException("SENDGRID_API_KEY environment variable not defined");
                                }
                                return new EmailClient(sendGridApiKey);
                            })
                            .AddScoped<WebWatcherClient>();
                })
                .RunCommandLineApplicationAsync<Program>(args);
        }

        private readonly WebWatcherClient _client;

        public Program(WebWatcherClient webWatcherClient)
        {
            _client = webWatcherClient;
        }

        [Option("-l|--list-websites", Description = "List all websites currently being watched")]
        public bool ListWebsites { get; set; }

        [Option("-a|--add-website", Description = "Add a website to watch")]
        public string WebsiteToAdd { get; }

        [Option("-s|--selector", Description = "Element selector")]
        public string ElementSelector { get; }

        [Option("-n|--notify-email", Description = "Email address to notify. Required if --add-url is specified.")]
        public string EmailToNotify { get; }

        [Option("-rm|--remove-website", Description = "Remove a website. Prompts for the ID to remove.")]
        public bool RemoveWebsite { get; set; }

        private async Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default)
        {
            if (ListWebsites)
            {
                ListAllWebsites();
                return 0;
            }

            if (RemoveWebsite)
            {
                PromptToRemoveWebsite();
                return 0;
            }

            if (!string.IsNullOrEmpty(WebsiteToAdd))
            {
                if (string.IsNullOrEmpty(EmailToNotify))
                {
                    System.Console.Error.WriteLine("Use -n|--notify-email to specify an email address to notify.");
                    return 1;
                }                
                return AddWebsiteToWatch(WebsiteToAdd, ElementSelector, EmailToNotify);
            }

            // If EmailToNotify is specified on its own, list registered websites and prompt user to select one.
            if (!string.IsNullOrEmpty(EmailToNotify))
            {
                int websiteId = PromptForWebsiteId();
                AddEmailToNotify(websiteId, EmailToNotify);
            }

            return await CheckAllWebsitesAsync();
        }

        private async Task<int> CheckAllWebsitesAsync()
        {
            await _client.CheckAllWebsitesAsync();

            return 0;
        }

        private void ListAllWebsites()
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            var serializer = new XmlSerializer(typeof(Website));

            var websites = _client.GetAllWebsites();
            foreach(var website in websites)
            {

                using (var stream = new StringWriter())
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, website, emptyNamespaces);
                    var websiteXml = stream.ToString();
                    System.Console.WriteLine(websiteXml);
                    System.Console.WriteLine();
                }
            }
        }

        private int PromptForWebsiteId()
        {
            int? websiteId = null;

            var websites = _client.GetAllWebsites();
            while(websiteId == null)
            {                
                foreach(var website in websites)
                {
                    System.Console.WriteLine($"{website.Id}: {website.Url}");
                }

                websiteId = Prompt.GetInt("Enter website Id: ");
            }
            return websiteId.Value;
        }

        private int AddWebsiteToWatch(string websiteUrlToAdd, string elementSelector, string emailToNotify)
        {
            return _client.AddWebsiteToWatch(websiteUrlToAdd, elementSelector, emailToNotify);
        }

        private int AddEmailToNotify(int websiteIdToAdd, string emailToNotify)
        {
            return _client.AddEmailToNotify(websiteIdToAdd, emailToNotify);
        }

        private void PromptToRemoveWebsite()
        {
            var websiteId = PromptForWebsiteId();
            var website = _client.GetWebsiteById(websiteId);
            if(website != null)
            {
                var confirmRemove = Prompt.GetYesNo($"Are you sure you want to remove {website.Url} from the watch list?", defaultAnswer: false);
                if (confirmRemove)
                {
                    _client.DeleteWebsite(website);
                }
            }            
        }
    }
}