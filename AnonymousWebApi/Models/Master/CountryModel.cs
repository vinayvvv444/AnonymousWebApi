using AnonymousWebApi.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Models.Master
{
    public class CountryModel : GeneralModel
    {
       
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }
    }
}
