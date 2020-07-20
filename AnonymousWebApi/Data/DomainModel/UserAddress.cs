using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.DomainModel
{
    public class UserAddress
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [Required]
        public string AddressType { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PinCode { get; set; }
        public string Locality { get; set; }
        public string District { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
    }
}
