using NotificationService.Models;
using NotificationService.Models.DbContexts;
using NotificationService.Models.Domains;
using NotificationService.Models.PushNotification;
using NotificationService.Services.NonRelational.Interfaces;
using NotificationService.Services.Relational.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using WebPush;

namespace NotificationService.Services.Relational.Implementations
{
    public class PushNotificationRepository : IPushNotificationRepository
    {
        private const string ClientRequired = "client is required";
        private const string ClientAlreadyExists = "client already exists";
        private const string ClientNotSuscribed = "Client is not subscriped to push notification";
        private const string ErrorMessage = "An error occured, contact admin";
        private readonly List<string> ResponseType = new List<string>();
        private readonly NoficationDb _context;
        private readonly PushNotificationConfig _pushNotificationConfig;
        private readonly IServiceResponse _response;

        public PushNotificationRepository(NoficationDb context, IServiceResponse response,
            PushNotificationConfig pushNotificationConfig)
        {
            _context = context;
            _pushNotificationConfig = pushNotificationConfig;
            _response = response;
        }
        public Response<List<string>> Create(CreatePushNotificationDTO data)
        {
            if (data.Client == null)
            {
                return _response.FailedResponse(ResponseType, ClientRequired);
            }

            if (GetClientNames().Contains(data.Client))
            {
                return _response.FailedResponse(ResponseType, ClientAlreadyExists);
            }
            var subscription = new PushSubscription(data.Endpoint, data.P256dh, data.Auth);
            var status = SaveSubscription(data.Client, subscription);
            return status ? _response.SuccessResponse(GetClientNames()) :
                _response.FailedResponse(ResponseType, ErrorMessage);
        }
        public Response<List<string>> Get()
        {
            return _response.SuccessResponse(GetClientNames());
        }

        public Response<List<string>> Notify(SendPushNotificationDTO pushNotification)
        {
            if (pushNotification.Client == null)
            {
                return _response.FailedResponse(ResponseType, ClientRequired);
            }
            PushSubscription subscription = GetSubscription(pushNotification.Client);
            if (subscription == null)
            {
                return _response.FailedResponse(ResponseType, ClientNotSuscribed);
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
                return _response.FailedResponse(ResponseType, ErrorMessage);
            }

            return _response.SuccessResponse(GetClientNames());
        }
        public Response<PushSubscription> GetClientSubscription(string client)
        {
            return _response.SuccessResponse(GetSubscription(client));
        }


        private PushSubscription GetSubscription(string client)
        {
            var result = _context.Clients.Where(x => x.Name == client).FirstOrDefault();
            return new PushSubscription()
            {
                Auth = result.Auth,
                Endpoint = result.Endpoint,
                P256DH = result.P256dh
            };
        }

        private bool SaveSubscription(string client, PushSubscription subscription)
        {
            var currentClient = new Client()
            {
                Name = client,
                Auth = subscription.Auth,
                Endpoint = subscription.Endpoint,
                P256dh = subscription.P256DH
            };
            _context.Clients.Add(currentClient);
            return _context.SaveChanges() > 0;
        }
        private List<string> GetClientNames()
        {
            var list = _context.Clients.Select(x => x.Name).ToList();
            return list;
        }

    }
}
