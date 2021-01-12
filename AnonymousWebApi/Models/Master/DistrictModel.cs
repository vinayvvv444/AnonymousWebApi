using AnonymousWebApi.Data.DomainModel.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnonymousWebApi.Models.Master
{
    public class DistrictModel : GeneralModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        [JsonIgnore]
        public Country CountryModel { get; set; }

        [JsonIgnore]
        public State StateModel { get; set; }
    }
}
