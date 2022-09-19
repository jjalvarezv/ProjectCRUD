using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainLayer.DTOs;

namespace DomainLayer.DomainModels
{
    public class ProductResponse
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public Boolean Success { get; set; }

        public List<ProductDto>? Data { get; set; }

        public ProductResponse(ProductDto data, int statusCode)
        {
            Status = statusCode;
            Message = string.Empty;
            Success = true;
            Data = new List<ProductDto>() { data };
        }

        public ProductResponse(List<ProductDto> data, int statusCode)
        {
            Status = statusCode;
            Message = string.Empty;
            Success = true;
            Data = data;
        }

        public ProductResponse(string msg)
        {
            Status = 500;
            Message = msg;
            Success = false;
        }
    }
}
