using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.DTOs;
using InfraestructureLayer.DataModels;

namespace WebApi.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
       CreateMap<Customer, CustomerDto>();
       CreateMap<DomainLayer.DomainModels.Customer, InfraestructureLayer.DataModels.Customer>();
       CreateMap<Product, ProductDto>();
       CreateMap<DomainLayer.DomainModels.Product, InfraestructureLayer.DataModels.Product>();
       CreateMap<Address, AddressDto>();
       CreateMap<DomainLayer.DomainModels.Address, InfraestructureLayer.DataModels.Address>();
    }
}