using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainLayer.DomainModels;

namespace InfraestructureLayer.DataServices.Abstractions;

public interface IProductService
{
    Task<ProductResponse> GetProducts();
    Task<ProductResponse> GetProduct(int id);

    Task<ProductResponse> UpdateProduct(Product product, int id);

    Task<ProductResponse> DeleteProduct(int id);

    Task<ProductResponse> CreateProduct(Product product);
}