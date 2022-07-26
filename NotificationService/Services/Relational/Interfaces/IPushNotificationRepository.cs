using NotificationService.Models;
using NotificationService.Models.PushNotification;
using System.Collections.Generic;
using WebPush;

namespace NotificationService.Services.Relational.Interfaces
{
    public interface IPushNotificationRepository
    {
        Response<List<string>> Get();
        Response<List<string>> Notify(SendPushNotificationDTO pushNotification);
        Response<List<string>> Create(CreatePushNotificationDTO data);
        public Response<PushSubscription> GetClientSubscription(string client);

    }
}
