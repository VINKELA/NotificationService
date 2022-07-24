using NotificationService.Models;

namespace NotificationService.Services.NonRelational.Interfaces
{
    public interface IEmailService
    {
        Response<string> SendEmail(EmailDTO message);
    }
}
