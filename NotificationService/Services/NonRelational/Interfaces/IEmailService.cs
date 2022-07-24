using NotificationService.Models;
using System.Threading.Tasks;

namespace NotificationService.Services.NonRelational.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);

    }
}
