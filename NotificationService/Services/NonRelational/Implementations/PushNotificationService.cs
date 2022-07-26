using NotificationService.Models;
using NotificationService.Models.PushNotification;
using NotificationService.Services.NonRelational.Interfaces;
using NotificationService.Services.Relational.Interfaces;
using System;
using System.Collections.Generic;
using WebPush;

namespace NotificationService.Services.NonRelational.Implementations
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly PushNotificationConfig _pushNotificationConfig;
        private readonly IPushNotificationRepository _pushNotificationRepository;
        private readonly IServiceResponse _reponse;
        private readonly List<string> responseType = new List<string>();


        public PushNotificationService(PushNotificationConfig pushNotificationConfig,
            IPushNotificationRepository pushNotificationRepository, IServiceResponse response)
        {
            _pushNotificationConfig = pushNotificationConfig;
            _pushNotificationRepository = pushNotificationRepository;
            _reponse = response;
        }

        public Response<List<string>> Notify(SendPushNotificationDTO pushNotification)
        {
            if (pushNotification.Client == null)
            {
                return _reponse.FailedResponse(responseType, "client is required");
            }
            PushSubscription subscription = _pushNotificationRepository
                .GetClientSubscription(pushNotification.Client).Data;
            if (subscription == null)
            {
                return _reponse.FailedResponse(responseType, "subscription is required");
            }

            var subject = _pushNotificationConfig.Subject;
            var publicKey = _pushNotificationConfig.PublicKey;
            var privateKey = _pushNotificationConfig.PrivateKey;

            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);

            var webPushClient = new WebPushClient();
            try
            {
                webPushClient.SendNotification(subscription, pushNotification.Message, vapidDetails);
            }
            catch (Exception exception)
            {
                // Log error
                Console.WriteLine(exception);
                return _reponse.FailedResponse(responseType, "An error occured contact administrator");
            }
            var list = _pushNotificationRepository.Get().Data;
            return _reponse.SuccessResponse(list);
        }

    }
}
