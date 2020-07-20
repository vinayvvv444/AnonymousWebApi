using AnonymousWebApi.Data.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.DomainModelConfiguration
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddressInfo");
            builder.Property(p => p.District).IsRequired().HasColumnType("nvarchar(150)").HasColumnName("DistrictOrCity");

            

            
        }
    }
}
