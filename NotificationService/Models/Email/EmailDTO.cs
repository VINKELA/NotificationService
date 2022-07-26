using System.Collections.Generic;

namespace NotificationService.Models
{
    public class EmailDTO
    {
        public string[] Destinations { get; set; }
        public string[] CcDestinations { get; set; }
        public string[] BccDestination { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string TemplateUrl { get; set; }
        public List<byte[]> Attachments { get; set; }
        public Dictionary<string, string> TemplateCustomization { get; set; }
        public string SendApp { get; set; }

    }
}
