using AnonymousWebApi.Controllers;
using AnonymousWebApi.Models.Master;
using AnonymousWebApiTest.Fixture;
using AnonymousWebApiTest.Theory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AnonymousWebApiTest
{
    public class MasterControllerTest : IClassFixture<ControllerFixture>
    {
        MasterController masterController;

        public MasterControllerTest(ControllerFixture fixture)
        {
            masterController = fixture.masterController;
        }

        [Theory]
        [ClassData(typeof(CountryTheoryData))]
        public void AddCountry_ValidModelWithTestDataPassed_ReturnsOKResult(CountryModel model)
        {
            masterController.ModelState.Clear();
            var result = masterController.AddCountry(model).Result as OkObjectResult;

            Assert.Equal(200,result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(result.Value), JsonConvert.SerializeObject(model));
        }

        [Theory]
        [ClassData(typeof(CountryTheoryData))]
        public void AddCountry_InValidModelNameMissing_ReturnBadRequest(CountryModel model)
        {
            model.Name = null;
            masterController.ModelState.AddModelError("Name", "Required");

            var result = masterController.AddCountry(model) as Task<IActionResult>;
            var badRequest = result.Result as BadRequestObjectResult;

             Assert.Equal(400, badRequest.StatusCode);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCountryUsingEFCore_WhenCalled_ReturnsOkResult()
        {
            var result = masterController.GetAllCountryUsingEFCore().Result as OkObjectResult;

            Assert.Equal(200, result.StatusCode);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<CountryModel>>(result.Value);
        }
    }
}
