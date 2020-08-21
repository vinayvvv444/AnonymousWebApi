using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.Contracts.Master
{
    public interface ICountryCommandText
    {
        string GetAllCountry { get; }
        string GetProductById { get; }
        string AddCountry { get; }
        string UpdateProduct { get; }
        string RemoveProduct { get; }
    }
}
