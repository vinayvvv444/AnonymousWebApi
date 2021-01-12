using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.DomainModel.Log
{
    public class NLogs
    {
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Level { get; set; }
       
        [Column(TypeName = "nvarchar(250)")]
        public string Logger { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Application { get; set; }
      
        public string CallSite { get; set; }
       
        public string Exception { get; set; }
        [Required]
        public DateTime Logged { get; set; }
    }
}
