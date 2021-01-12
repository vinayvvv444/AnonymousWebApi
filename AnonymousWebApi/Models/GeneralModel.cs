using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Models
{
    public class GeneralModel
    {
        [Required]
        public string CreatedUser { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string UpdatedUser { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }
    }
}
