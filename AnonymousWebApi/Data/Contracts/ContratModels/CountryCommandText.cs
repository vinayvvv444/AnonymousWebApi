using AnonymousWebApi.Data.Contracts.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.Contracts.ContratModels
{
    public class CountryCommandText : ICountryCommandText
    {
        public string GetAllCountry => "Select * From MasterCountry";
        public string GetProductById => "Select * From MasterCountry Where Id= @Id";
        public string AddCountry => "Insert Into  MasterCountry ([CreatedUser],[CreatedDate],[UpdatedUser],[UpdatedDate],[Name]) Values (@CreatedUser, @CreatedDate, @UpdatedUser, @UpdatedDate, @Name)";
        public string UpdateProduct => "Update Product set Name = @Name, Cost = @Cost, CreatedDate = GETDATE() Where Id =@Id";
        public string RemoveProduct => "Delete From Product Where Id= @Id";
    }
}
