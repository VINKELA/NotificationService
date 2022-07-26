using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NotificationService.MiddleWares;
using NotificationService.Models.DbContexts;
using NotificationService.Models.Email;
using NotificationService.Models.PushNotification;
using NotificationService.Services.NonRelational.Implementations;
using NotificationService.Services.NonRelational.Interfaces;
using NotificationService.Services.Relational.Implementations;
using NotificationService.Services.Relational.Interfaces;

namespace NotificationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NoficationDb>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("NotificationDb")));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notification Service", Version = "v1" });
            });
            var emailConfig = Configuration
              .GetSection("EmailConfiguration")
              .Get<EmailConfig>();
            var pushNotificationConfig = Configuration
              .GetSection("VAPID")
              .Get<PushNotificationConfig>();

            services.AddSingleton(emailConfig);
            services.AddSingleton(pushNotificationConfig);
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IServiceResponse, ServiceReponse>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<IPushNotificationRepository, PushNotificationRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification Service v1"));

            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
