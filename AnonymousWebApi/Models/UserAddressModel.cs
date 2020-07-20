using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Models
{
    public class UserAddressModel
    {
        public int Id { get; set; }
        public string AddressType { get; set; }
        public string Name { get; set; }
        public string PinCode { get; set; }
        public string Locality { get; set; }
        public string District { get; set; }

        public string UserId { get; set; }

        public ApplicationUserModel User { get; set; }
    }
}
