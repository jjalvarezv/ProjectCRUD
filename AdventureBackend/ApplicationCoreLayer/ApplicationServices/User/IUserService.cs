using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainLayer.DTOs;
using DomainLayer.DomainModels;

namespace ApplicationCoreLayer.ApplicationServices.User;

public interface IUserService
{
    void CreateLoggedUser(UserRequest userName);
    void SetLoggedUser(RefreshToken refreshToken);
    Boolean CheckUserLogged(string refreshToken);
    string GetUserName();
}
