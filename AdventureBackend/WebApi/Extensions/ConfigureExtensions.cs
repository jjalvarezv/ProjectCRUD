using ApplicationCoreLayer.ApplicationServices.Token;
using ApplicationCoreLayer.ApplicationServices.User;
using ApplicationLayer.Services;
using DomainLayer.Abstractions;
using InfraestructureLayer.Data;
using InfraestructureLayer.DataServices.Abstractions;
using InfraestructureLayer.DataServices.Implementations;
using InfraestructureLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Extensions;
public class ConfigureExtensions
{
    public static void AddDependencies(IServiceCollection services,string connectionString)
    {
        services.AddDbContext<AdventureWorksLT2019Context>(options => options.UseSqlServer(connectionString));
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IDataBaseRepository, DatabaseRepository>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IUserService, UserService>();
    }
}

