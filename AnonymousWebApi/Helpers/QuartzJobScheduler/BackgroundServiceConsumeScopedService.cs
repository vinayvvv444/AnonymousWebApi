using AnonymousWebApi.Data.EFCore;
using AnonymousWebApi.Data.EFCore.Repository.Master;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnonymousWebApi.Helpers.QuartzJobScheduler
{
    public class BackgroundServiceConsumeScopedService : BackgroundService, IHostedService
    {
        public IServiceScopeFactory _serviceScopeFactory;
        public BackgroundServiceConsumeScopedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scoped = scope.ServiceProvider.GetRequiredService<AnonymousDBContext>();
                //var scoped1 = scope.ServiceProvider.GetRequiredService<StdSchedulerFactory>();

                //Do your stuff
            }
            return Task.CompletedTask;
        }
    }
    public interface IScoped { }

    public class Scoped : IScoped { }
}
