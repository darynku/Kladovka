using Kladovka.Clients;
using Notification.Contracts;
using Notification.Options;
using System.Net.Http.Headers;

namespace Notification.Clients
{
    public class EmailNotificationClient : BaseApiClient, IEmailNotificationClient
    {
        private readonly NotificationClientOptions _options;
        public EmailNotificationClient(HttpClient httpClient, NotificationClientOptions options) : base(httpClient) 
        {
            _options = options;
        }

        protected override AuthenticationHeaderValue? CreateAuhenticationHeaders()
        {
            return base.CreateAuhenticationHeaders();
        }

        public async Task SendEmailAsync(EmailNotificationData data, CancellationToken cancellationToken)
        {
            var url = $"{_options.Url}/email/send";
            await PostAsync(url, data, cancellationToken);
        }
    }
}
