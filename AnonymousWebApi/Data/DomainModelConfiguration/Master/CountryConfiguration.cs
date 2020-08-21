using AnonymousWebApi.Data.DomainModel.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.DomainModelConfiguration.Master
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("MasterCountry");

            builder.Property(p => p.CreatedUser).IsRequired().HasColumnType("nvarchar(450)");


        }
    }
}
