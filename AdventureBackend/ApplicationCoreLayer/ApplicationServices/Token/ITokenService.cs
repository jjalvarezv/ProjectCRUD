using DomainLayer.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCoreLayer.ApplicationServices.Token
{
    public interface ITokenService
    {
        string GenerateJwt(string name);
        RefreshToken GenerateRefreshToken(string userJwt);
    }
}
