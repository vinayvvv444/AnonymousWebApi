using AnonymousWebApi.Data.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.DomainModelConfiguration 
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasOne(p => p.Grade)
                .WithMany(p => p.Students)
                .HasForeignKey(p => p.GradeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
