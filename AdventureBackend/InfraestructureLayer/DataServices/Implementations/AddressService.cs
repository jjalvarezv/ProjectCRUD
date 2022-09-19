using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Abstractions;
using DomainLayer.DomainModels;
using DomainLayer.DTOs;
using InfraestructureLayer.DataServices.Abstractions;

namespace InfraestructureLayer.DataServices.Implementations;

public class AddressService : IAddressService
{
    private readonly IDataBaseRepository _databaseRepository;
    private readonly IMapper _mapper;

    public AddressService(IDataBaseRepository dataBaseRepository, IMapper mapper)
    {
        _databaseRepository = dataBaseRepository; 
        _mapper = mapper;
    }

    public async Task<AddressResponse> CreateAddress(Address address)
    {
        var dataAddress = _mapper.Map<InfraestructureLayer.DataModels.Address>(address);

        var response = await _databaseRepository.DbCreate(dataAddress);

        if (response) return new AddressResponse(_mapper.Map<AddressDto>(dataAddress), 201);
        else return new AddressResponse("Not possible to CREATE");
    }

    public async Task<AddressResponse> DeleteAddress(int id)
    {
        var dataAddress = await _databaseRepository.DbDelete(id, "addresses");

        if (dataAddress) return new AddressResponse(new AddressDto(), 204);
        else return new AddressResponse("Not possible to DELETE");
    }

    public async Task<AddressResponse> GetAddress(int id)
    {
        var dataAddress = await _databaseRepository.DbGet<InfraestructureLayer.DataModels.Address>(id, "addresses");

        if (dataAddress != null) return new AddressResponse(_mapper.Map<AddressDto>(dataAddress), 200);
        else return new AddressResponse("Not possible to GET");
    }

    public async Task<AddressResponse> UpdateAddress(Address address, int id)
    {
        var dataAddress = _mapper.Map<InfraestructureLayer.DataModels.Address>(address);

        var response = await _databaseRepository.DbUpdate(dataAddress, id);

        if (response) return new AddressResponse(_mapper.Map<AddressDto>(dataAddress), 200);
        else return new AddressResponse("Not possible to UPDATE");
    }

    public async Task<AddressResponse> GetAddresses()
    {
        var dataAddress = await _databaseRepository.DbGetAll<InfraestructureLayer.DataModels.Address>("addresses");

        if (dataAddress != null)
        {
            var dataList = new List<AddressDto>();
            foreach(InfraestructureLayer.DataModels.Address data in dataAddress) {
                dataList.Add(_mapper.Map<AddressDto>(data));
            }
            return new AddressResponse(dataList, 200);
        } 
        else return new AddressResponse("Not possible to GET");
    }
}
