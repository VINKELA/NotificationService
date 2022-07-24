using MimeKit;
using System.Collections.Generic;

namespace NotificationService.Models
{
    public class Message
    {

        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public List<byte[]> Attachments { get; set; }
    }
}
