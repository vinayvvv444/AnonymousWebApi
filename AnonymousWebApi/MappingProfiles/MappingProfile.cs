using AnonymousWebApi.Data.DomainModel;
using AnonymousWebApi.Data.DomainModel.Master;
using AnonymousWebApi.Helpers.ExtensionMethods;
using AnonymousWebApi.Models;
using AnonymousWebApi.Models.Master;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<UserAddress, UserAddressModel>();
            CreateMap<UserAddressModel, UserAddress>();
            

            CreateMap<ApplicationUser, ApplicationUserModel>();
            CreateMap<ApplicationUserModel, ApplicationUser>();

            //CreateMap<Country, CountryModel>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Country, CountryModel>().IgnoreNoMap();
            CreateMap<CountryModel, Country>();

            CreateMap<State, StateModel>().IgnoreNoMap().AfterMap((b, r) =>
            {
                r.CountryModel = b.Country;
            });
            CreateMap<StateModel, State>();

            CreateMap<District, DistrictModel>().IgnoreNoMap();
            CreateMap<DistrictModel, District>();
        }
    }
}
