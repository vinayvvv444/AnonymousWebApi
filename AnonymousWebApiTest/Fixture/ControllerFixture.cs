using AnonymousWebApi.Controllers;
using AnonymousWebApiTest.Mock.Entities;
using System;
using AnonymousWebApi.Data.DomainModel.Master;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AnonymousWebApi.MappingProfiles;
using AnonymousWebApi.Data.EFCore.Repository.Master;
using AnonymousWebApi.Data.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AnonymousWebApi.Data.Contracts.Master;
using AnonymousWebApi.Data.Contracts.ContratModels;
using Microsoft.Extensions.Caching.Memory;

namespace AnonymousWebApiTest.Fixture
{
    public class ControllerFixture : IDisposable
    {
        public AnonymousDbContextMock anonymousDbContextMock { get; set; }

        private IMapper mapper { get; set; }

        public CountryRepository countryRepository { get; set; }

        public StateRepository stateRepository { get; set; }

        public DistrictRepository districtRepository { get; set; }

        public ILoggerManager logger { get; set; }

        public ILogger<MasterController> loggerNew { get; set; }
        public IMemoryCache memoryCache { get; set; }

        public IConfiguration configuration { get; set; }

        public ICountryCommandText countryCommandText { get; set; }
        public MasterController masterController { get; private set; }

        public ControllerFixture()
        {
            #region Create mock/memory database

            anonymousDbContextMock = new AnonymousDbContextMock();

            anonymousDbContextMock.MasterCountry.AddRange(new Country[] {
                new Country()
                {
                    Id = 12345,
                    Name = "Sample Country",
                    CountryCode = "Samp",
                    CreatedDate = DateTime.Now,
                    CreatedUser = Guid.NewGuid().ToString(),
                    UpdatedDate = DateTime.Now,
                    UpdatedUser = Guid.NewGuid().ToString()
                },
                new Country()
                {
                    Id = 67890,
                    Name = "Sample Country new",
                    CountryCode = "Samp new",
                    CreatedDate = DateTime.Now,
                    CreatedUser = Guid.NewGuid().ToString(),
                    UpdatedDate = DateTime.Now,
                    UpdatedUser = Guid.NewGuid().ToString()
                }
            });

            anonymousDbContextMock.SaveChangesAsync();

            #endregion

            #region Mapper settings with original profiles.

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            mapper = mappingConfig.CreateMapper();

            #endregion

            countryCommandText = new CountryCommandText();

            countryRepository = new CountryRepository(configuration, countryCommandText, anonymousDbContextMock);
            stateRepository = new StateRepository(configuration, anonymousDbContextMock);
            logger = new LoggerManager();
            

            masterController = new MasterController(mapper, countryRepository, stateRepository,districtRepository, logger, loggerNew, memoryCache);
        }

        #region ImplementIDisposableCorrectly
        /** https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1063?view=vs-2019 */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerFixture()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                anonymousDbContextMock.Dispose();
                anonymousDbContextMock = null;

                //userService = null;
                mapper = null;
                countryRepository = null;
                stateRepository = null;
                logger = null;
                loggerNew = null;
                masterController = null;
            }
        }
        #endregion
    }
}
