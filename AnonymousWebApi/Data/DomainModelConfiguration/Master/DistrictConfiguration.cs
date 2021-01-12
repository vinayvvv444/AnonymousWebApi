using AnonymousWebApi.Data.DomainModel.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.DomainModelConfiguration.Master
{
    public class DistrictConfiguration : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("MasterDistrict");

            builder.Property(x => x.Name).HasColumnName("DistrictName").IsRequired().HasColumnType("nvarchar(450)");

            builder.Property(x => x.CountryId).HasColumnName("CountryId").IsRequired().HasColumnType("int");

            builder.Property(x => x.StateId).HasColumnName("StateId").IsRequired().HasColumnType("int");
        }
    }
}
