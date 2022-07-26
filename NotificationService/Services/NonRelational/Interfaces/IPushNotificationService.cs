using NotificationService.Models;
using NotificationService.Models.PushNotification;
using System.Collections.Generic;

namespace NotificationService.Services.NonRelational.Interfaces
{
    public interface IPushNotificationService
    {
        Response<List<string>> Notify(SendPushNotificationDTO pushNotification);
    }
}
