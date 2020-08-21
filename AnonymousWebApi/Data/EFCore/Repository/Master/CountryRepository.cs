using AnonymousWebApi.Data.Contracts.Master;
using AnonymousWebApi.Data.DomainModel.Master;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.EFCore.Repository.Master
{
    public class CountryRepository : EfCoreRepository<Country, AnonymousDBContext>
    {
        private readonly AnonymousDBContext _context;
        private readonly ICountryCommandText _countryCommandText;
        public CountryRepository(IConfiguration configuration, ICountryCommandText countryCommandText, AnonymousDBContext context) : base(configuration,context)
        {
            _context = context;
            _countryCommandText = countryCommandText;
        }

        public async Task<IEnumerable<Country>> GetAllCountries()
        {

            return await WithConnection(async conn =>
            {
                var query = await conn.QueryAsync<Country>(_countryCommandText.GetAllCountry).ConfigureAwait(false);
                return query;
            }).ConfigureAwait(false);

        }

        public async Task AddCountryUsingDapper(Country entity)
        {
            await WithConnection(async conn =>
            {
                await conn.ExecuteAsync(_countryCommandText.AddCountry,
                    new { CreatedUser = entity.CreatedUser, CreatedDate = entity.CreatedDate, UpdatedUser = entity.UpdatedUser, UpdatedDate = entity.UpdatedDate, Name = entity.Name}).ConfigureAwait(false);
            }).ConfigureAwait(false);

        }
    }
}
