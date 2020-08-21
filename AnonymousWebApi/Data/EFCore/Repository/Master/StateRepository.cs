using AnonymousWebApi.Data.DomainModel.Master;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.EFCore.Repository.Master
{
    public class StateRepository : EfCoreRepository<State, AnonymousDBContext>
    {
        private readonly AnonymousDBContext _context;

        public StateRepository(IConfiguration configuration, AnonymousDBContext context) : base(configuration, context)
        {
            _context = context;
        }
    }
}
