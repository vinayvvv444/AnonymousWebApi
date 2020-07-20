using AnonymousWebApi.Data.DomainModel;
using AnonymousWebApi.Models;
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
        }
    }
}
