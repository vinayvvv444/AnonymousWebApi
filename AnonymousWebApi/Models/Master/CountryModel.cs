using AnonymousWebApi.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Models.Master
{
    public class CountryModel : GeneralModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CountryCode { get; set; }
    }
}
