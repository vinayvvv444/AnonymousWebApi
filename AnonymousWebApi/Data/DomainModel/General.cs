using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.DomainModel
{
    public class General
    {
        [Required]
        [Column(TypeName = "nvarchar(450)")]
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(450)")]
        public string UpdatedUser { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
