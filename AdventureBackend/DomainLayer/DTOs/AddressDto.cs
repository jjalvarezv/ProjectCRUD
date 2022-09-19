using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainLayer.DTOs;

public class AddressDto
{
    public int AddressId { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = null!;
    public string StateProvince { get; set; } = null!;
    public string CountryRegion { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    
}