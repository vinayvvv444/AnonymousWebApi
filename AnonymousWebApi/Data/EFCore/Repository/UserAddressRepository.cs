using AnonymousWebApi.Data.DomainModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.EFCore.Repository
{
    public class UserAddressRepository : EfCoreRepository<UserAddress, AnonymousDBContext>
    {
        private readonly AnonymousDBContext _context;
        public UserAddressRepository(IConfiguration configuration, AnonymousDBContext context) : base(configuration, context)
        {
            _context = context;
        }

       
    }
}
