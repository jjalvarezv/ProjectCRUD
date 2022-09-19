using System.Net;
using System.Security.Cryptography;
using System.Collections.Immutable;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using DomainLayer.DomainModels;
using ApplicationCoreLayer.ApplicationServices.User;

namespace ApplicationCoreLayer.ApplicationServices.Token;
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    public TokenService(IConfiguration configuration, IUserService userService)
    {
        _configuration =  configuration;
        _userService = userService;
    }

    public string GenerateJwt(string name)
    {
        var claims = new[] 
        {
            new Claim(ClaimTypes.NameIdentifier, name)
        };

        var token = new JwtSecurityToken
        (
            issuer: _configuration.GetSection("Jwt:Issuer").Value,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(30),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value)), 
                SecurityAlgorithms.HmacSha256
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(string userJwt)
    {
        var refreshToken = new RefreshToken() {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(2),
            Created = DateTime.Now
        };

        _userService.SetLoggedUser(refreshToken);

        return refreshToken;
    }

}