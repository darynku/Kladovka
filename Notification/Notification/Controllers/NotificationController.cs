using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Contracts;
using Notification.Handlers;

namespace Notification.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(ISender _sender) : ControllerBase
    {

        [HttpPost("email/send")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Send(EmailNotificationData data, CancellationToken cancellationToken)
        {
            var request = new SendEmailCommand(data);
            await _sender.Send(request, cancellationToken);
            return NoContent();
        }
    }
}
