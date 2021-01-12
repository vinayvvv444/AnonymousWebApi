using Microsoft.AspNetCore.Mvc;
using SampleApp.Controllers;
using SampleApp.Models;
using SampleApp.Test.Fixture;
using SampleApp.Test.Theory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleApp.Test
{
    public class ProductControllerFixtureTest : IClassFixture<ControllerFixture>
    {
       ProductsController productsController;
        IProductsRepository productsRepository;
        public ProductControllerFixtureTest(ControllerFixture fixture)
        {
            productsController = fixture.productsController;
            productsRepository = fixture.productsRepository;
        }

        [Fact]
        public async Task GetProduct_ReturnProductList_OKResult()
        {
            var result = await productsController.GetProduct();
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Theory]
        [InlineData(1000)]
        public async Task GetProductById_WhenCalledWithId_ReturnProduct(int id)
        {
            var x = productsRepository.GetAsync(id);
            var result = await productsController.GetProduct(id);
            Assert.IsType<OkObjectResult>(result.Result);
        }


        [Theory]
        [ClassData(typeof(ProductTheoryData))]
        public async Task PostProduct_WhenCalledPassModel_ReturnProduct(Product model)
        {
            var result = await productsController.PostProduct(model);
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}
