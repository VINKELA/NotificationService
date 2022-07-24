using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services.NonRelational.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Notifications : ControllerBase
    {
        private readonly IEmailService _emailService;
        public Notifications(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // POST api/<Notifications>
        [HttpPost("sendEmail")]
        public IActionResult Post([FromBody] EmailDTO email) => Ok(_emailService.SendEmailAsync(new Message(email)));
    }
}
