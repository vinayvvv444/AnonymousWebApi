using AnonymousWebApi.Data.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnonymousWebApiTest.Mock.Entities
{
    public partial class AnonymousDbContextMock : AnonymousDBContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
    }
}
