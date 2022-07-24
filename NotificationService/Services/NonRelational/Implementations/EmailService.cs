using MailKit.Net.Smtp;
using MimeKit;
using NotificationService.Models;
using NotificationService.Services.NonRelational.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace NotificationService.Services.NonRelational.Implementations
{

    public class EmailService : IEmailService
    {
        private const string SuccessMessage = "Message with subject {0} sent successfully to {1} recipient";
        private const string EmailFailed = "Email Failed";
        private readonly EmailConfig _emailConfig;
        private readonly IServiceResponse _response;

        public EmailService(EmailConfig emailConfig, IServiceResponse response)
        {
            _emailConfig = emailConfig;
            _response = response;
        }
        public Response<string> SendEmail(EmailDTO message)
        {
            var emailMessage = CreateEmailMessage(message);
            var result = SendAsync(emailMessage);
            return result ? _response.SuccessResponse(string.Format(SuccessMessage, message.Subject,
                message.Destinations.Count())) : _response.FailedResponse(EmailFailed);

        }

        private MimeMessage CreateEmailMessage(EmailDTO message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From, _emailConfig.From));
            var recipents = message.Destinations.Select(x => InternetAddress.Parse(x));
            emailMessage.To.AddRange(recipents);
            emailMessage.Subject = message.Subject;
            var cc = message.CcDestinations.Select(x => InternetAddress.Parse(x));
            emailMessage.Cc.AddRange(cc);
            var bcc = message.BccDestination.Select(x => InternetAddress.Parse(x));
            emailMessage.Bcc.AddRange(bcc);
            var templateUrl = message.TemplateUrl ?? _emailConfig.MailTemplate;
            var template = @"";
            using (var webClient = new WebClient())
            {
                template = webClient.DownloadString(templateUrl);
            }

            template = template.Replace("{title}", message.Subject).Replace("{message}", message.Message);

            if (message.TemplateCustomization != null && message.TemplateCustomization.Count() > 0)
            {
                foreach (var templateCustomization in message.TemplateCustomization)
                    template = template.Replace(templateCustomization.Key, templateCustomization.Value);
            }

            var bodyBuilder = new BodyBuilder { HtmlBody = template };
            if (message.Attachments != null && message.Attachments.Count() > 0)
            {
                var attachmentsList = message.Attachments;
                foreach (var attachment in from attachment in attachmentsList
                                           let fileInformation = new FileInfo(attachment)
                                           where fileInformation.Exists
                                           select attachment)
                {
                    bodyBuilder.Attachments.Add(attachment);
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
        private bool SendAsync(MimeMessage emailMesage)
        {
            bool result;
            using SmtpClient client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.From, _emailConfig.Password);
                client.Send(emailMesage);
                result = true;
            }
            catch (Exception ex)
            {
                //This needs to be logged actually
                Console.Out.WriteLine("" + ex.Message);
                throw;

            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
            return result;
        }

    }
}
