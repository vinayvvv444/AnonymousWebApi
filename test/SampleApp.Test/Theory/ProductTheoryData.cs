using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleApp.Test.Theory
{
    class ProductTheoryData : TheoryData<Product>
    {
        public ProductTheoryData()
        {
            Add(new Product()
            {
                Id = 5525,
                Name = "Shoe",
                Price = 500
            });
        }
    }
}
