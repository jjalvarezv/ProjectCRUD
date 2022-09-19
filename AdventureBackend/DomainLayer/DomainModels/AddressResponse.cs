using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainLayer.DTOs;

namespace DomainLayer.DomainModels;
public class AddressResponse
{
    public int Status { get; set; }   
    public string Message { get; set; }       
    public Boolean Success { get; set; }
    public List<AddressDto>? Data { get; set; }

    public AddressResponse(AddressDto data, int statusCode)
    {
        Status = statusCode;
        Message = string.Empty;
        Success = true;
        Data = new List<AddressDto>() { data };
    }

    public AddressResponse(List<AddressDto> data, int statusCode)
    {
        Status = statusCode;
        Message = string.Empty;
        Success = true;
        Data = data;
    }

    public AddressResponse(string msg)
    {
        Status = 500;
        Message = msg;
        Success = false;
    }
}
