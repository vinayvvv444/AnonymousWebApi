using AnonymousWebApi.Data.Contracts.Master;
using AnonymousWebApi.Data.DomainModel.Master;
using Dapper;
using Microsoft.EntityFrameworkCore;
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
        public CountryRepository(IConfiguration configuration,
            ICountryCommandText countryCommandText,
            AnonymousDBContext context) : base(configuration, context)
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
                    new { CreatedUser = entity.CreatedUser, CreatedDate = entity.CreatedDate, UpdatedUser = entity.UpdatedUser, UpdatedDate = entity.UpdatedDate, Name = entity.Name, CountryCode = entity.CountryCode }).ConfigureAwait(false);
            }).ConfigureAwait(false);

        }


        public IEnumerable<Country> GetAllCountriesUsingFromSqlRaw()
        {
            return _context.MasterCountry.FromSqlRaw("select * from MasterCountry").ToList();
        }

        

        public IEnumerable<Country> GetAllCountriesUsingFromSqlRawSP()
        {
            return _context.MasterCountry.FromSqlRaw<Country>("GetAllCountrySP").ToList();
        }

        public async Task AddCountryUsingTransaction(Country entity)
        {
            using(var transaction = _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.MasterCountry.AddAsync(entity).ConfigureAwait(false);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    await transaction.Result.CommitAsync().ConfigureAwait(false);
                }
                catch(Exception Ex)
                {
                    await transaction.Result.RollbackAsync().ConfigureAwait(false);
                }
            }
        }

    }
}
