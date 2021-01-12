using AnonymousWebApi.Data.Contracts;
using AnonymousWebApi.Data.Contracts.ContratModels;
using AnonymousWebApi.Data.EFCore.Repository.Master;
using AnonymousWebApi.Helpers.EmailService;
using AnonymousWebApi.Helpers.QuartzJobScheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Configuration;

namespace AnonymousWebApi.Helpers.ExtensionMethods
{
    public static class ServiceExtensions
    {
        //public static IConfiguration Configuration { get; }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<StateRepository>();
            services.AddScoped<DistrictRepository>();
        }

        public static void ConfigureQuartzServices(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, CustomQuartzJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<NotificationJob>();
            //services.AddScoped<NotificationJob>();
            // services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Notification Job", "0/1 * * * * ?"));
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Notification Job", "0 44 14 ? * *"));
            services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Notification Job", "0/10 * * * * ?"));
            // services.AddHostedService<BackgroundServiceConsumeScopedService>();
            services.AddHostedService<CustomQuartzHostedService>();

            // for sending email job
            //services.AddSingleton<IJobFactory, CustomQuartzJobFactory>();
            //services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            //services.AddSingleton<EmailJob>();
            //services.AddSingleton(new JobMetadata(Guid.NewGuid(), typeof(EmailJob), "Email Job", "0/10 * * * * ?"));
            //services.AddHostedService<CustomQuartzHostedService>();
        }

        public static void ConfigureEmailServices(this IServiceCollection services, IConfiguration Configuration)
        {
            var emailConfig = Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddScoped<IEmailSender, EmailSender>();
        }

    }
}
