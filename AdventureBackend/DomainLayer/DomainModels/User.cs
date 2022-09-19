using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainLayer.DomainModels;
public class User
{
    public string UserName { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }

    public User(string username) 
    {   
        this.UserName = username;
    }
}
