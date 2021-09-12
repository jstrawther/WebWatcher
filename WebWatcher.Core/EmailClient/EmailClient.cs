using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebWatcher.Core.EmailClient
{
    public class EmailClient : IEmailClient
    {
        private readonly string _sendgridApiKey;

        public EmailClient(string sendgridApiKey)
        {
            if(string.IsNullOrWhiteSpace(sendgridApiKey))
            {
                throw new ArgumentNullException(nameof(sendgridApiKey));
            }

            _sendgridApiKey = sendgridApiKey;
        }

        public async Task SendAsync(string from, string to, string subject, string body)
        {
            var client = new SendGridClient(_sendgridApiKey);

            // TODO: get from address from config file.
            var fromAddress = new EmailAddress("jstrawther@resourcedata.com", "WebWatcher");
            var toAddress = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, body, body);
            msg.AddHeader("X-Entity-Ref-ID", DateTime.Now.ToString());
            await client.SendEmailAsync(msg);
        }
    }
}
