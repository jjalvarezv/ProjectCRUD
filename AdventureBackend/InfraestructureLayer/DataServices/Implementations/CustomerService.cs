
using AutoMapper;
using DomainLayer.Abstractions;
using DomainLayer.DomainModels;
using DomainLayer.DTOs;
using InfraestructureLayer.DataModels;
using Microsoft.Extensions.Configuration;

namespace ApplicationLayer.Services;

public class CustomerService : ICustomerService
{
    private readonly IDataBaseRepository _databaseRepository;
    private readonly IMapper _mapper;

    public CustomerService(IDataBaseRepository dataBaseRepository, IMapper mapper)
    {
        _databaseRepository = dataBaseRepository; 
        _mapper = mapper;
    }

    public async Task<CustomerResponse> GetCustomer(int id)
    {
        var dataCustomer = await _databaseRepository.DbGet<InfraestructureLayer.DataModels.Customer>(id, "customers");

        if (dataCustomer != null) return new CustomerResponse(_mapper.Map<CustomerDto>(dataCustomer), 200);
        else return new CustomerResponse("Not possible to GET");

    }

    public async Task<CustomerResponse> CreateCustomer(DomainLayer.DomainModels.Customer customer)
    {
        var dataCustomer = _mapper.Map<InfraestructureLayer.DataModels.Customer>(customer);

        var response = await _databaseRepository.DbCreate(dataCustomer);

        if (response) return new CustomerResponse(_mapper.Map<CustomerDto>(dataCustomer), 201);
        else return new CustomerResponse("Not possible to CREATE");
    }

    public async Task<CustomerResponse> UpdateCustomer(DomainLayer.DomainModels.Customer customer, int id)
    {
        var dataCustomer = _mapper.Map<InfraestructureLayer.DataModels.Customer>(customer);

        var response = await _databaseRepository.DbUpdate(dataCustomer, id);

        if (response) return new CustomerResponse(_mapper.Map<CustomerDto>(dataCustomer), 200);
        else return new CustomerResponse("Not possible to UPDATE");
    }

    public async Task<CustomerResponse> DeleteCustomer(int id)
    {
        var dataResponse = await _databaseRepository.DbDelete(id, "customers");

        if (dataResponse) return new CustomerResponse(new CustomerDto(), 204);
        else return new CustomerResponse("Not possible to DELETE");
    }

    public async Task<CustomerResponse> GetCustomers()
    {
        var dataCustomer = await _databaseRepository.DbGetAll<InfraestructureLayer.DataModels.Customer>("customers");

        if (dataCustomer != null)
        {
            var dataList = new List<CustomerDto>();
            foreach(InfraestructureLayer.DataModels.Customer data in dataCustomer) {
                dataList.Add(_mapper.Map<CustomerDto>(data));
            }
            return new CustomerResponse(dataList, 200);
        } 
        else return new CustomerResponse("Not possible to GET");
    }
}