namespace NotificationService.Models.PushNotification
{
    public class CreatePushNotificationDTO
    {
        public string Auth { get; set; }
        public string P256dh { get; set; }
        public string Client { get; set; }
        public string Endpoint { get; set; }

    }
    public class SendPushNotificationDTO
    {
        public string Client { get; set; }
        public string Message { get; set; }

    }

}
