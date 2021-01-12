using Microsoft.AspNetCore.Mvc;
using Moq;
using SampleApp.Controllers;
using SampleApp.Models;
using SampleApp.Test.Theory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Net;

namespace SampleApp.Test
{
    public class ProductControllerMoqTest
    {
        //private Mock<IProductsRepository> productsRepositoryMock; 
        //private ProductsController productsController;
        //public ProductControllerMoqTest()
        //{
        //    productsRepositoryMock = new Mock<IProductsRepository>();
        //    productsController = new ProductsController(productsRepositoryMock.Object);
        //}

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

        [Theory]
        [InlineData(5)]
        public async Task GetProductById_WhenCalledWithId_ReturnsProduct(int id)
        {
            // arrange
            // int id = 5;
            var productsRepositoryMock = new Mock<IProductsRepository>();
            var expectedResult = new Product();
            productsRepositoryMock.Setup(p => p.GetAsync(id)).ReturnsAsync(expectedResult);
            var productsController = new ProductsController(productsRepositoryMock.Object);
            // act
            var productRetrived = await productsController.GetProduct(id);
            // assert
            Assert.Equal(expectedResult, productRetrived.Value);
        }

        [Theory]
        [InlineData(0)]
        public async Task GetProductById_WhenCalledWithId_ReturnsNotFound(int id)
        {
            // arrange
            // int id = 5;
            var productsRepositoryMock = new Mock<IProductsRepository>();
            var expectedResult = new Product();
            expectedResult = null;
            productsRepositoryMock.Setup(p => p.GetAsync(id)).ReturnsAsync(expectedResult);
            var productsController = new ProductsController(productsRepositoryMock.Object);
            // act
            var productRetrived = await productsController.GetProduct(id);
            // assert
            Assert.IsType<NotFoundResult>((NotFoundResult)productRetrived.Result);
        }

        [Theory]
        [ClassData(typeof(ProductTheoryData))]
        public async Task PostProduct_ShouldReturnProduct_CreatedAtAction(Product model)
        {
            // arrange
            var productsRepositoryMock = new Mock<IProductsRepository>();
            
            productsRepositoryMock.Setup(p => p.CreateAsync(model)).ReturnsAsync(true);
            var productsController = new ProductsController(productsRepositoryMock.Object);

            // act
            var productRetrived = await productsController.PostProduct(model).ConfigureAwait(true);
            // assert
            Assert.Equal(model, ((CreatedAtActionResult)productRetrived.Result).Value);
            Assert.IsType<Product>(((CreatedAtActionResult)productRetrived.Result).Value);
            Assert.IsType<CreatedAtActionResult>(productRetrived.Result);
            //Assert.IsType<NotFoundResult>((NotFoundResult)productRetrived.Result);
        }

        [Theory]
        [ClassData(typeof(ProductTheoryData))]
        public async Task PostProduct_PriceLessThanZero_ReturnBadRequest(Product model)
        {
            // arrange
            model.Price = -1;
            var productsRepositoryMock = new Mock<IProductsRepository>();

            productsRepositoryMock.Setup(p => p.CreateAsync(model)).ReturnsAsync(true);
            var productsController = new ProductsController(productsRepositoryMock.Object);

            // act
            var productRetrived = await productsController.PostProduct(model).ConfigureAwait(true);

            // assert
            Assert.IsType<BadRequestResult>(productRetrived.Result);
        }

        [Theory]
        [ClassData(typeof(ProductTheoryData))]
        public async Task PostProduct_InsertFails_ReturnBadRequest(Product model)
        {
            // arrange
            var productsRepositoryMock = new Mock<IProductsRepository>();

            productsRepositoryMock.Setup(p => p.CreateAsync(model)).ReturnsAsync(false);
            var productsController = new ProductsController(productsRepositoryMock.Object);

            // act
            var productRetrived = await productsController.PostProduct(model).ConfigureAwait(true);

            // assert
            Assert.IsType<BadRequestResult>(productRetrived.Result);
        }

        [Theory]
        [ClassData(typeof(ProductTheoryData))]
        public async Task PostProduct_InsertFailsIfModelIsEmpty_ReturnBadRequest(Product model)
        {
            // arrange
            model = null;
            var productsRepositoryMock = new Mock<IProductsRepository>();

            productsRepositoryMock.Setup(p => p.CreateAsync(model)).ReturnsAsync(false);
            var productsController = new ProductsController(productsRepositoryMock.Object);

            // act
            var productRetrived = await productsController.PostProduct(model).ConfigureAwait(true);

            // assert
            Assert.IsType<BadRequestResult>(productRetrived.Result);
        }

        [Theory]
        [ClassData(typeof(ProductTheoryData))]
        public async Task PutProduct_ReturnNoContent_OnUpdate(Product model)
        {
            // arrange
            var productsRepositoryMock = new Mock<IProductsRepository>();

            productsRepositoryMock.Setup(p => p.UpdateAsync(model)).ReturnsAsync(true);
            var productsController = new ProductsController(productsRepositoryMock.Object);

            // act
            var productRetrived = await productsController.PutProduct(model.Id,model).ConfigureAwait(true);

            // assert
            Assert.IsType<NoContentResult>(productRetrived);
            ((NoContentResult)productRetrived).StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(ProductTheoryData))]
        public async Task PutProduct_ReturnNotFoundIfUpdateFails_OnUpdate(Product model)
        {
            // arrange
            var productsRepositoryMock = new Mock<IProductsRepository>();

            productsRepositoryMock.Setup(p => p.UpdateAsync(model)).ReturnsAsync(false);
            var productsController = new ProductsController(productsRepositoryMock.Object);

            // act
            var productRetrived = await productsController.PutProduct(model.Id, model).ConfigureAwait(true);

            // assert
            Assert.IsType<NotFoundResult>(productRetrived);
            ((NotFoundResult)productRetrived).StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(25)]
        public async Task DeleteProduct_WhenCalledAndPassId_ReturnNoContent(int id)
        {
            // arrange
            var productsRepositoryMock = new Mock<IProductsRepository>();

            productsRepositoryMock.Setup(p => p.DeleteAsync(id)).ReturnsAsync(true);
            var productsController = new ProductsController(productsRepositoryMock.Object);

            // act
            var productRetrived = await productsController.DeleteProduct(id).ConfigureAwait(true);

            // assert
            Assert.IsType<NoContentResult>(productRetrived);
            ((NoContentResult)productRetrived).StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }
    }
}
