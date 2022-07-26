namespace NotificationService.Models.Domains
{
    public class Client : Base
    {

        public string Auth { get; set; }
        public string P256dh { get; set; }
        public string Name { get; set; }
        public string Endpoint { get; set; }
    }
}
