using AnonymousWebApi.Data.Contracts;
using AnonymousWebApi.Data.DomainModel.Master;
using AnonymousWebApi.Data.EFCore;
using AnonymousWebApi.Data.EFCore.Repository.Master;
using AnonymousWebApi.Models.Master;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Helpers.QuartzJobScheduler
{
    [DisallowConcurrentExecution]
    public class NotificationJob : IJob
    {
        private readonly ILogger<NotificationJob> _logger;
        private readonly ILoggerManager _nlogger;
         private readonly IMapper _mapper;
        public IServiceScopeFactory _serviceScopeFactory;
        // private CountryRepository _countryRepository;
        public NotificationJob(ILogger<NotificationJob> logger,
            ILoggerManager nlogger,
            IServiceScopeFactory serviceScopeFactory,
            IMapper mapper)
        {
            _logger = logger;
            _nlogger = nlogger;
            _mapper = mapper;
            _serviceScopeFactory = serviceScopeFactory;
            // _countryRepository = countryRepository;
        }
        public Task Execute(IJobExecutionContext context)
        {
            CountryModel model = new CountryModel()
            {
                Name = "Fiji5",
                CountryCode = "ff5",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedUser = "abc5",
                UpdatedUser = "def5"
            };
            //     _countryRepository.Add(_mapper.Map<CountryModel, Country>(model)).ConfigureAwait(false);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AnonymousDBContext>();
               // var countryRepository = scope.ServiceProvider.GetRequiredService<CountryRepository>();
               // countryRepository.AddSync(_mapper.Map<CountryModel, Country>(model));
                //dbContext.Add(_mapper.Map<CountryModel, Country>(model));
                //dbContext.SaveChanges();
            }
                _logger.LogInformation("Hello world!");
            _nlogger.LogError("Schedule job run");
            return Task.CompletedTask;
        }
    }
}
