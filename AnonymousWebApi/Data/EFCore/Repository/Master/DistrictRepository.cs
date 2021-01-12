using AnonymousWebApi.Data.DomainModel.Master;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.EFCore.Repository.Master
{
    public class DistrictRepository : EfCoreRepository<District, AnonymousDBContext>
    {
        private readonly AnonymousDBContext _context;
        public DistrictRepository(IConfiguration configuration, AnonymousDBContext context) : base(configuration, context)
        {
            _context = context;
        }
    }
}
