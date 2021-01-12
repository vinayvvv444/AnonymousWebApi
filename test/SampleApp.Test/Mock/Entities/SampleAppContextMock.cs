using Microsoft.EntityFrameworkCore;
using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.Test.Mock.Entities
{
    public partial class SampleAppContextMock : SampleAppContext
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
