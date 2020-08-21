using AnonymousWebApi.Data.DomainModel;
using AnonymousWebApi.Data.DomainModel.Log;
using AnonymousWebApi.Data.DomainModel.Master;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace AnonymousWebApi.Data.EFCore
{
    public class AnonymousDBContext : IdentityDbContext
    {
        public AnonymousDBContext(DbContextOptions<AnonymousDBContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<UserDetails> UsersDetails { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Country> MasterCountry { get; set; }

        public DbSet<State> MasterState { get; set; }

        public DbSet<NLogs> NLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>()
                        .ToTable("Users", "dbo");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
