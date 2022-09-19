using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainLayer.DomainModels;
using DomainLayer.DTOs;

namespace ApplicationCoreLayer.ApplicationServices.User;

public class UserService : IUserService
{
    private DomainLayer.DomainModels.User userLogged;

    public Boolean CheckUserLogged(string refreshToken)
    {
        if ((!refreshToken.Equals(userLogged.RefreshToken)) || (userLogged.TokenExpires < DateTime.Now)) return false;
        else return true;
    }

    public void CreateLoggedUser(UserRequest userName)
    {
        this.userLogged = new DomainLayer.DomainModels.User(userName.UserName);
    }

    public void SetLoggedUser(RefreshToken refreshToken)
    {
        userLogged.RefreshToken = refreshToken.Token;
        userLogged.TokenCreated = refreshToken.Created;
        userLogged.TokenExpires = refreshToken.Expires;

    }

    public string GetUserName()
    {
        if (userLogged is null) return "";
        else return userLogged.UserName;
    }
}
