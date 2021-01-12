using AnonymousWebApi.Models.Master;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AnonymousWebApiTest.Theory
{
    public class CountryTheoryData : TheoryData<CountryModel>
    {
        public CountryTheoryData()
        {
            Add(new CountryModel()
            {
                Id = 123456,
                Name = "Sample Country",
                CountryCode = "Samp",
                CreatedDate = DateTime.Now,
                CreatedUser = "dfdfdf",
                UpdatedDate = DateTime.Now,
                UpdatedUser = "dffffffffffffffffffffdf"
            });
        }
    }
}
