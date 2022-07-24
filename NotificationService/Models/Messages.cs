using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace NotificationService.Models
{
    public class Message
    {

        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public List<byte[]> Attachments { get; set; }
        public Message(EmailDTO emailDTO)
        {
            To = new List<MailboxAddress>();
            To.AddRange(emailDTO.To.Select(x => new MailboxAddress(x.Key, x.Value)));

            Subject = emailDTO.Subject;
            Content = emailDTO.Content;
            Attachments = emailDTO.Attachments;
        }
    }
}
