using AnonymousWebApi.Data.DomainModel.Master;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<State> GetAllStatesByCountryIdUsingFromSqlRaw(int countryId)
        {
            //_context.MasterState.Where(x => x.CountryId == countryId).Select(x => x).ToList();
            //var data = (from c in _context.MasterState where c.CountryId == countryId select c).ToList();
            return _context.MasterState.FromSqlRaw("select * from MasterState where CountryId=" + countryId).ToList();
        }
    }
}
