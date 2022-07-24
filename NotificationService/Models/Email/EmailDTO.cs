using System.Collections.Generic;

namespace NotificationService.Models
{
    public class EmailDTO
    {
        public Dictionary<string, string> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<byte[]> Attachments { get; set; }

    }
}
