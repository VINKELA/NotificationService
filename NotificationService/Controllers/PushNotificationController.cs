using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Models.PushNotification;
using NotificationService.Services.NonRelational.Interfaces;
using NotificationService.Services.Relational.Interfaces;
using System.Collections.Generic;
using WebPush;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushNotificationController : ControllerBase
    {
        private readonly IPushNotificationRepository _pushNotificationRepository;
        private readonly IPushNotificationService _pushNotificationService;
        public PushNotificationController(IPushNotificationRepository pushNotificationRepository,
            IPushNotificationService pushNotification)
        {
            _pushNotificationRepository = pushNotificationRepository;
            _pushNotificationService = pushNotification;

        }
        // GET: api/<PushNotifications>
        [HttpGet]
        public Response<List<string>> GetClients()
        {
            return _pushNotificationRepository.Get();
        }

        // GET api/<PushNotifications>/5
        [HttpGet("{client}")]
        public Response<PushSubscription> GetSuscription(string client) =>
            _pushNotificationRepository.GetClientSubscription(client);

        // POST api/<PushNotifications>
        [HttpPost]
        public Response<List<string>> Post([FromBody] CreatePushNotificationDTO data) =>
            _pushNotificationRepository.Create(data);

        // PUT api/<PushNotifications>/5
        [HttpPost("notify")]
        public Response<List<string>> Notify([FromBody] SendPushNotificationDTO notificationDTO) =>
            _pushNotificationService.Notify(notificationDTO);
    }
}
