using Microsoft.AspNetCore.Mvc;
using Moq;
using SampleApp.Controllers;
using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class ProductControllerMoqTest
    {
        [Fact]
        public async Task GetProduct_WhenCalled_ReturnsOKResult()
        {
            // arrange
            var productsRepositoryMock = new Mock<IProductsRepository>();
            var expectedProducts = new List<Product>();
            productsRepositoryMock.Setup(p => p.GetAllAsync()).ReturnsAsync(expectedProducts);
            var productsController = new ProductsController(productsRepositoryMock.Object);
            // act
            var productsRetrieved = await productsController.GetProduct();
            // assert
            Assert.Equal(expectedProducts, ((OkObjectResult)productsRetrieved.Result).Value);
        }
    }
}
