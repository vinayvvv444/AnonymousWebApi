using AnonymousWebApi.Data.DomainModel.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.DomainModelConfiguration.Master
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ToTable("MasterState");

            builder.Property(x => x.Name).HasColumnName("StateName").IsRequired().HasColumnType("nvarchar(450)");

            builder.Property(x => x.CountryId).HasColumnName("CountryId").IsRequired().HasColumnType("int");

            
        }
    }
}
