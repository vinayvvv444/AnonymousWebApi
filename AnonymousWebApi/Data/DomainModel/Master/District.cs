using AnonymousWebApi.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.DomainModel.Master
{
    public class District : General
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        
        public virtual State StateModel { get; set; }

       
        public virtual Country Country { get; set; }
    }
}
