using Kladovka.Contracts.Handlers;
using Notification.Services;

namespace Notification.Handlers
{
    public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand>
    {
        private readonly IEmailSendingService _emailSendingService;
        public async Task Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            return await _emailSendingService.SendEmail(request.Email, )
        }
    }
}
