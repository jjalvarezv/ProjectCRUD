using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainLayer.DomainModels;

namespace InfraestructureLayer.DataServices.Abstractions;

public interface IAddressService
{
    Task<AddressResponse> GetAddresses();

    Task<AddressResponse> GetAddress(int id);

    Task<AddressResponse> UpdateAddress(Address product, int id);

    Task<AddressResponse> DeleteAddress(int id);

    Task<AddressResponse> CreateAddress(Address product);        
}
