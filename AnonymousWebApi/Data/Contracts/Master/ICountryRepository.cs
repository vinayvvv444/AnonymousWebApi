using AnonymousWebApi.Data.DomainModel.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.Contracts.Master
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllCountries();

        Task AddCountryUsingDapper(Country entity);
    }
}
