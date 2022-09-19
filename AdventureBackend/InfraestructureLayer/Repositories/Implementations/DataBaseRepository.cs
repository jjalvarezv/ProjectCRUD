using System.Collections.Immutable;
using DomainLayer.Abstractions;
using InfraestructureLayer.Data;
using InfraestructureLayer.DataModels;
using Microsoft.EntityFrameworkCore;

namespace InfraestructureLayer.Repositories;

public class DatabaseRepository : IDataBaseRepository
{
    private readonly AdventureWorksLT2019Context _dbContext;

    public DatabaseRepository(AdventureWorksLT2019Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> DbGet<T>(int id, string table) where T : class , new()
    {
        switch (table)
        {
            case "customers":
                return await _dbContext.Customers.FindAsync(id)
                is T customer
                ? customer
                : null;
            case "products":
                return await _dbContext.Products.FindAsync(id)
                is T product
                ? product
                : null;
            case "addresses":
                return await _dbContext.Addresses.FindAsync(id)
                is T addresss
                ? addresss
                : null;
            default:
                return null;
        }       
    }

    public async Task<Boolean> DbCreate(Customer customer)
    {
        try
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch(DbUpdateConcurrencyException) { return false; }
        catch(OperationCanceledException) { return false; }
    }

    public async Task<Boolean> DbCreate(Product product)
    {
        try
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch(DbUpdateConcurrencyException) { return false; }
        catch(OperationCanceledException) { return false; }
    }

    public async Task<Boolean> DbCreate(Address address)
    {
        try
        {
            _dbContext.Addresses.Add(address);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch(DbUpdateConcurrencyException) { return false; }
        catch(OperationCanceledException) { return false; }
    }

    public async Task<Boolean> DbUpdate(Customer customer, int id)
    {
        var selectedCustomer = await _dbContext.Customers.FindAsync(id);

        if (selectedCustomer != null) 
        {
            selectedCustomer.FirstName = customer.FirstName;
            selectedCustomer.LastName = customer.LastName;
            selectedCustomer.CompanyName = customer.CompanyName;
            selectedCustomer.Phone = customer.Phone;
            selectedCustomer.PasswordHash = customer.PasswordHash;
            selectedCustomer.PasswordSalt = customer.PasswordSalt;
            selectedCustomer.ModifiedDate = customer.ModifiedDate;
            await _dbContext.SaveChangesAsync();
            return true;
        } else 
            return false;
    }

    public async Task<Boolean> DbUpdate(Product product, int id)
    {
        var selectedProduct = await _dbContext.Products.FindAsync(id);

        if (selectedProduct != null) 
        {
            selectedProduct.Name = product.Name;
            selectedProduct.ProductNumber = product.ProductNumber;
            selectedProduct.StandardCost = product.StandardCost;
            selectedProduct.ListPrice = product.ListPrice;
            selectedProduct.Size = product.Size;
            selectedProduct.Weight = product.Weight;
            selectedProduct.SellStartDate = product.SellStartDate;
            selectedProduct.ModifiedDate = product.ModifiedDate;
            await _dbContext.SaveChangesAsync();
            return true;
        } else 
            return false;
    }

    public async Task<Boolean> DbUpdate(Address address, int id)
    {
        var selectedAddress = await _dbContext.Addresses.FindAsync(id);

        if (selectedAddress != null) 
        {
            selectedAddress.AddressLine1 = address.AddressLine1;
            selectedAddress.AddressLine2 = address.AddressLine2;
            selectedAddress.City = address.City;
            selectedAddress.StateProvince = address.StateProvince;
            selectedAddress.CountryRegion = address.CountryRegion;
            selectedAddress.PostalCode = address.PostalCode;
            selectedAddress.ModifiedDate = address.ModifiedDate;
            await _dbContext.SaveChangesAsync();
            return true;
        } else 
            return false;
    }
    
    public async Task<Boolean> DbDelete(int id, string table)
    {
        switch (table)
        {
            case "customers":
                var customer = await _dbContext.Customers.FindAsync(id);
                if (customer is null) return false;
                else 
                {
                    _dbContext.Customers.Remove(customer);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            case "products":
                var product = await _dbContext.Products.FindAsync(id);
                if (product is null) return false;
                else 
                {
                    _dbContext.Products.Remove(product);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            case "addresses":
                var address = await _dbContext.Addresses.FindAsync(id);
                if (address is null) return false;
                else 
                {
                    _dbContext.Addresses.Remove(address);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            default:
                return true;
        }
    }

    public async Task<ICollection<T>?> DbGetAll<T>(string table) where T : class , new()
    {
        switch (table)
        {
            case "customers":
                if (typeof(T).Equals(typeof(Customer))){
                    return (ICollection<T>) await _dbContext.Customers.OrderByDescending<Customer, DateTime>(k => k.ModifiedDate).Take(15).ToListAsync(); 
                } else return new List<T>() {};
            case "products":
                if (typeof(T).Equals(typeof(Product))){
                    return (ICollection<T>) await _dbContext.Products.OrderByDescending<Product, DateTime>(k => k.ModifiedDate).Take(15).ToListAsync(); 
                } else return new List<T>() {};
            case "addresses":
                if (typeof(T).Equals(typeof(Address))){
                    return (ICollection<T>) await _dbContext.Addresses.OrderByDescending<Address, DateTime>(k => k.ModifiedDate).Take(15).ToListAsync(); 
                } else return new List<T>() {};
            default:
                return new List<T>() {};
        }  
    }
}  