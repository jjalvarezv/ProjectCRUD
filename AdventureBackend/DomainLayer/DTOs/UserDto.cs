using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainLayer.DTOs;
public class UserDto
{
    public string UserName { get; set; } = "";
    // public string Password { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public string Jwt { get; set; } = "";
}