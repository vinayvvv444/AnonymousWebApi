using AnonymousWebApi.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.DomainModel.Master
{
    public class Country : General
    {
        //[NoMap]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(450)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(450)")]
        public string CountryCode { get; set; }

    }
}
