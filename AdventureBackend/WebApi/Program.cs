using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using ApplicationCoreLayer.ApplicationServices.Token;
using ApplicationCoreLayer.ApplicationServices.User;
using DomainLayer.Abstractions;
using DomainLayer.DomainModels;
using InfraestructureLayer.DataServices.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer auth with jwt",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        {
            new OpenApiSecurityScheme 
            {
                Reference = new OpenApiReference 
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, 
            new List<string>()
        }
    });
});
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program)); 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddRouting();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
ConfigureExtensions.AddDependencies(builder.Services, connectionString);

var app = builder.Build();
{
    app.UseCors(options => {
        // options.WithOrigins("http://localhost:3000/")
        //     .AllowCredentials()
        //     .AllowAnyHeader()
        //     .AllowAnyMethod();
        options.WithOrigins("http://localhost:3000", 
            "http://localhost:3000/customers",
            "http://localhost:3000/customers/add",
            "http://localhost:3000/customers/edit",
            "http://localhost:3000/products")
            .AllowCredentials()
            .AllowAnyMethod().AllowAnyHeader();
        options.AllowAnyHeader();
        options.AllowAnyMethod();
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    #region AuthEndpoint
    app.MapPost("/auth", (ITokenService _tokenService, IUserService _userService, HttpContext context, [FromBody] UserRequest user) => {

        if (context.Request.Cookies["refreshToken"] is not null) 
            return Results.Unauthorized();
        
        _userService.CreateLoggedUser(user);
        var jwt = _tokenService.GenerateJwt(user.UserName);

        if (!jwt.Equals("")) 
        {
            var refreshToken = _tokenService.GenerateRefreshToken(jwt);
            context.Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions()
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            });
            return Results.Ok(jwt);
        } else 
        {
            return Results.Problem("Error creating jwt");
        }
    });

    app.MapGet("/auth/user", (HttpContext _context, IUserService _userService) => 
    {
        var oldRefreshToken = _context.Request.Cookies["refreshToken"];
        if (oldRefreshToken is null){
            Console.WriteLine("refreshNull");
            return Results.NoContent();
        }
        
        var name = _userService.GetUserName();
        return name.Length > 0 
            ? Results.Ok(_userService.GetUserName())
            : Results.NoContent();
    });

    app.MapPost("/auth/refresh", (ITokenService _tokenService, IUserService _userService, HttpContext _context) => 
    {
        var oldRefreshToken = _context.Request.Cookies["refreshToken"];

        if (oldRefreshToken is null)
            return Results.Unauthorized();

        var response = _userService.CheckUserLogged(oldRefreshToken);
        if (!response) return Results.Unauthorized();

        var jwt = _tokenService.GenerateJwt(_userService.GetUserName());
        if (!jwt.Equals("")) 
        {
            var refreshToken = _tokenService.GenerateRefreshToken(jwt);
            _context.Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions()
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            });
            return Results.Ok(jwt);
        } else 
        {
            return Results.Problem("Error creating jwt");
        }

    });
    
    app.MapPost("/auth/logout", (HttpContext _context, IUserService _userService) => 
    {
        var oldRefreshToken = _context.Request.Cookies["refreshToken"];

        if(oldRefreshToken is null)  
            return Results.Unauthorized();

        var response = _userService.CheckUserLogged(oldRefreshToken);
        if (!response) return Results.Unauthorized();     

        _context.Response.Cookies.Delete("refreshToken");   
        return Results.NoContent();
    });

    #endregion

    #region CustomerEndpoints

        app.MapGet("/customer", 
        [Authorize]
        async (ICustomerService _customerService) =>
        {
            // Call to the service and receive the response
            var response = await _customerService.GetCustomers();
            return response.Success ? 
                Results.Ok(response) :
                Results.NotFound(response.Message);
        });

        app.MapGet("/customer/{id:int}", 
        [Authorize]
        async (ICustomerService _customerService, int id) =>
        {
            // Call to the service and receive the response
            var response = await _customerService.GetCustomer(id);
            return response.Success ? 
                Results.Ok(response) :
                Results.NotFound(response.Message);
        });

        app.MapPost("/customer", 
        [Authorize]
        async (ICustomerService _customerService, [FromBody]Customer customer) =>
        {
            // Call to the service and receive the response
            var response = await _customerService.CreateCustomer(customer);
            return response.Success ? 
                Results.Created($"/customer/{response?.Data?[0].CustomerId}", response) :
                Results.Problem(response.Message);
        });

        app.MapPut("/customer/{id:int}", 
        [Authorize]
        async (ICustomerService _customerService, [FromBody]Customer customer, int id) =>
        {
            // Call to the service and receive the response
            // if (id != customer.CustomerId) return Results.BadRequest();

            var response = await _customerService.UpdateCustomer(customer, id);
            return response.Success ? 
                Results.Ok(response) :
                Results.NotFound(response.Message);

        });

        app.MapDelete("/customer/{id:int}", 
        [Authorize]
        async (ICustomerService _customerService, int id) =>
        {
            // Call to the service and receive the response
            var response = await _customerService.DeleteCustomer(id);
            return response.Success ? 
                Results.NoContent() :
                Results.NotFound(response.Message);
        });

    #endregion

    #region ProductEndpoints

        app.MapGet("/product", 
        [Authorize]
        async (IProductService _productService) =>
        {
            // Call to the service and receive the response
            var response = await _productService.GetProducts();
            return response.Success ? 
                Results.Ok(response) :
                Results.NotFound(response.Message);
        });

        app.MapGet("/product/{id:int}", 
        [Authorize]
        async (IProductService _productService, int id) =>
        {
            // Call to the service and receive the response
            var response = await _productService.GetProduct(id);
            return response.Success ? 
                Results.Ok(response) :
                Results.NotFound(response.Message);
        });

        app.MapPost("/product", 
        [Authorize]
        async (IProductService _productService, [FromBody]Product product) =>
        {
            // Call to the service and receive the response
            var response = await _productService.CreateProduct(product);
            return response.Success ? 
                Results.Created($"/product/{response?.Data[0]?.ProductId}", response) :
                Results.Problem(response.Message);
        });

        app.MapPut("/product/{id:int}", 
        [Authorize]
        async (IProductService _productService, [FromBody]Product product, int id) =>
        {

            var response = await _productService.UpdateProduct(product, id);
            return response.Success ? 
                Results.Ok(response) :
                Results.NotFound(response.Message);

        });

        app.MapDelete("/product/{id:int}", 
        [Authorize]
        async (IProductService _productService, int id) =>
        {
            // Call to the service and receive the response
            var response = await _productService.DeleteProduct(id);
            return response.Success ? 
                Results.NoContent() :
                Results.NotFound(response.Message);
        });

    #endregion

    #region AddressEndpoints

        app.MapGet("/address", 
        [Authorize]
        async (IAddressService _addressService) =>
        {
            // Call to the service and receive the response
            var response = await _addressService.GetAddresses();
            return response.Success ? 
                Results.Ok(response) :
                Results.NotFound(response.Message);
        });

        app.MapGet("/address/{id:int}", 
        [Authorize]
        async (IAddressService _addressService, int id) =>
        {
            // Call to the service and receive the response
            var response = await _addressService.GetAddress(id);
            return response.Success ? 
                Results.Ok(response) :
                Results.NotFound(response.Message);
        });

        app.MapPost("/address", 
        [Authorize]
        async (IAddressService _addressService, [FromBody]Address address) =>
        {
            // Call to the service and receive the response
            var response = await _addressService.CreateAddress(address);
            return response.Success ? 
                Results.Created($"/address/{response?.Data[0]?.AddressId}", response) :
                Results.Problem(response.Message);
        });

        app.MapPut("/address/{id:int}", 
        [Authorize]
        async (IAddressService _addressService, [FromBody]Address address, int id) =>
        {
            var response = await _addressService.UpdateAddress(address, id);
            return response.Success ? 
                Results.Ok(response) :
                Results.NotFound(response.Message);

        });

        app.MapDelete("/address/{id:int}", 
        [Authorize]
        async (IAddressService _addressService, int id) =>
        {
            // Call to the service and receive the response
            var response = await _addressService.DeleteAddress(id);
            return response.Success ? 
                Results.NoContent() :
                Results.NotFound(response.Message);
        });
    #endregion

    app.Run();
}
