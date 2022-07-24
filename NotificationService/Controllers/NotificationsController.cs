using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services.NonRelational.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : Controller

    {
        private readonly IEmailService _emailService;
        public NotificationsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // POST api/<Notifications>
        [HttpPost("sendEmail")]
        public ActionResult<Response<string>> Post([FromBody] EmailDTO email) => Ok(_emailService.SendEmail(email));

        [HttpGet("emailTemplate")]
        public IActionResult MailTemplate() => View();
    }
}
