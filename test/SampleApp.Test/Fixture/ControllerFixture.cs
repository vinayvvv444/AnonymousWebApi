using SampleApp.Controllers;
using SampleApp.Models;
using SampleApp.Test.Mock.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.Test.Fixture
{
    public class ControllerFixture : IDisposable
    {
        public IProductsRepository productsRepository { get; set; }
        public SampleAppContextMock sampleAppContextMock { get; set; }

        public ProductsController productsController { get; set; }

        public ControllerFixture()
        {
            #region Create mock/memory database

            sampleAppContextMock = new SampleAppContextMock();

            sampleAppContextMock.Product.AddRange(new Product[]
            {
                new Product()
                {
                    Id = 1000,
                    Name = "Sample",
                    Price = 500
                },
                new Product()
                {
                    Id = 1001,
                    Name = "Sample new",
                    Price = 2000
                }
            });

            sampleAppContextMock.SaveChangesAsync();

            #endregion

            productsRepository = new ProductsRepository(sampleAppContextMock);
            productsController = new ProductsController(productsRepository);
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
                sampleAppContextMock.Dispose();
                sampleAppContextMock = null;
                productsController = null;
                productsRepository = null;
            }
        }
        #endregion

    }
}
