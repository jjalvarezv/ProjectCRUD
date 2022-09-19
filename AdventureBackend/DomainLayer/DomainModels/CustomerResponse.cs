using System.Reflection.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainLayer.DTOs;

namespace DomainLayer.DomainModels;

public class CustomerResponse
{
    public int Status { get; set; }   
    public string Message { get; set; }       
    public Boolean Success { get; set; }
    public List<CustomerDto>? Data { get; set; }

    public CustomerResponse(CustomerDto data, int statusCode)
    {
        Status = statusCode;
        Message = string.Empty;
        Success = true;
        Data = new List<CustomerDto>() {data};
    }

    public CustomerResponse(List<CustomerDto> data, int statusCode)
    {
        Status = statusCode;
        Message = string.Empty;
        Success = true;
        Data = data;
    }

    public CustomerResponse(string msg)
    {
        Status = 500;
        Message = msg;
        Success = false;
    }

}