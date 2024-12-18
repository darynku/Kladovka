using Microsoft.Extensions.Options;
using Notification.Options;
using System.Net.Mail;
using Notification.Helpers;
using System.Net;
namespace Notification.Services
{
    public class EmailSendingService : IEmailSendingService
    {
        private readonly MailOptions _options;
        private readonly ILogger<EmailSendingService> _logger;

        public EmailSendingService(
            ILogger<EmailSendingService> logger,
            IOptions<MailOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task SendEmail(string email, string subject, string message, CancellationToken cancellationToken)
        {
            var port = _options.Port.ToInt();
            if (port is null)
            {
                throw new ArgumentNullException("Пустое значение smtp порта");
            }
            using SmtpClient client = new(_options.Host, port.Value);

            client.Timeout = 10;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;

            if (string.IsNullOrEmpty(_options.Username) || string.IsNullOrEmpty(_options.Password))
            {
                client.Credentials = new NetworkCredential(_options.Username, _options.Password);
            }

            MailMessage mailMessage = new(_options.From, email)
            {
                Subject = _options.Subject,
                Body = message,
                IsBodyHtml = true
            };

            try
            {
                await client.SendMailAsync(mailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке письма на '{Email}'", email);
            }
        }
    }
}
