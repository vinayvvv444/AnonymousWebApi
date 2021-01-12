using AnonymousWebApi.Controllers;
using AnonymousWebApi.Data.Contracts;
using AnonymousWebApi.Data.Contracts.Master;
using AnonymousWebApi.Data.EFCore.Repository.Master;
using AnonymousWebApi.Models.Master;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace AnonymousWebApiTest
{
    public class MasterControllerMoqTest
    {

        private readonly MasterController _masterController;
        private readonly Mock<MasterController> _masterControllerMock = new Mock<MasterController>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<CountryRepository> _countryRepositoryMock = new Mock<CountryRepository>();
        private readonly Mock<StateRepository> _stateRepositoryMock = new Mock<StateRepository>();
        private readonly Mock<DistrictRepository> _districtRepositoryMock = new Mock<DistrictRepository>();
        private readonly Mock<ILoggerManager> _loggerMock = new Mock<ILoggerManager>();
        private readonly Mock<ILogger<MasterController>> _loggerNewMock = new Mock<ILogger<MasterController>>();
        private readonly Mock<IMemoryCache> _memoryCacheMock = new Mock<IMemoryCache>();

        public MasterControllerMoqTest()
        {
            _masterController = new MasterController(_mapperMock.Object,
                _countryRepositoryMock.Object,
                _stateRepositoryMock.Object,
                _districtRepositoryMock.Object,
                _loggerMock.Object,
                _loggerNewMock.Object,
                _memoryCacheMock.Object);
        }

        [Fact]
        public async Task GetCountryById_ShoiuldReturnCountry_WhenCountryExists()
        {
            // Arrange
            int countyrId = 5;

            CountryModel model = new CountryModel
            {
                Id = countyrId,
                Name = "Sample"
            };

            OkObjectResult obj = new OkObjectResult(model);

            _masterControllerMock.Setup(x => x.GetCountryById(countyrId).Result as OkObjectResult)
                .Returns(obj);

            // Act
            var countryData = await _masterController.GetCountryById(countyrId) as Task<IActionResult>;
            var result = countryData.Result as OkObjectResult;
          //  Assert.Equal(200, result.StatusCode);
        }
    }
}
