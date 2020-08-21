using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Models
{
    public class GeneralModel
    {
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }

       
        public string UpdatedUser { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
