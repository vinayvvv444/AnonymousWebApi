using AnonymousWebApi.Data.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.EFCore.Repository
{
    public class UserAddressRepository : EfCoreRepository<UserAddress, AnonymousDBContext>
    {
        public UserAddressRepository(AnonymousDBContext context) : base(context)
        {

        }
    }
}
