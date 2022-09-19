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

public class ProductService : IProductService
{

    private readonly IDataBaseRepository _databaseRepository;
    private readonly IMapper _mapper;

    public ProductService(IDataBaseRepository dataBaseRepository, IMapper mapper)
    {
        _databaseRepository = dataBaseRepository; 
        _mapper = mapper;
    }

    public async Task<ProductResponse> CreateProduct(Product product)
    {
        var dataProduct = _mapper.Map<InfraestructureLayer.DataModels.Product>(product);

        var response = await _databaseRepository.DbCreate(dataProduct);

        if (response) return new ProductResponse(_mapper.Map<ProductDto>(dataProduct), 201);
        else return new ProductResponse("Not possible to CREATE");
    }

    public async Task<ProductResponse> DeleteProduct(int id)
    {
        var dataProduct = await _databaseRepository.DbDelete(id, "products");

        if (dataProduct) return new ProductResponse(new ProductDto(), 204);
        else return new ProductResponse("Not possible to DELETE");
    }

    public async Task<ProductResponse> GetProduct(int id)
    {
        var dataProduct = await _databaseRepository.DbGet<InfraestructureLayer.DataModels.Product>(id, "products");

        if (dataProduct != null) return new ProductResponse(_mapper.Map<ProductDto>(dataProduct), 200);
        else return new ProductResponse("Not possible to GET");
    }

    public async Task<ProductResponse> UpdateProduct(Product product, int id)
    {
        var dataProduct = _mapper.Map<InfraestructureLayer.DataModels.Product>(product);

        var response = await _databaseRepository.DbUpdate(dataProduct, id);

        if (response) return new ProductResponse(_mapper.Map<ProductDto>(dataProduct), 200);
        else return new ProductResponse("Not possible to UPDATE");
    }

    public async Task<ProductResponse> GetProducts()
    {
        var dataProduct = await _databaseRepository.DbGetAll<InfraestructureLayer.DataModels.Product>("products");

        if (dataProduct != null)
        {
            var dataList = new List<ProductDto>();
            foreach(InfraestructureLayer.DataModels.Product data in dataProduct) {
                dataList.Add(_mapper.Map<ProductDto>(data));
            }
            return new ProductResponse(dataList, 200);
        } 
        else return new ProductResponse("Not possible to GET");
    }
}