

using InfraestructureLayer.DataModels;

namespace DomainLayer.Abstractions;

public interface IDataBaseRepository
{
    Task<ICollection<T>?> DbGetAll<T>(string table) where T : class , new();
    Task<T?> DbGet<T>(int id, string table) where T : class , new();
    Task<Boolean> DbCreate(Customer customer);
    Task<Boolean> DbCreate(Product product);
    Task<Boolean> DbCreate(Address address);

    Task<Boolean> DbUpdate(Customer customer, int id);
    Task<Boolean> DbUpdate(Product product, int id);
    Task<Boolean> DbUpdate(Address address, int id);

    Task<Boolean> DbDelete(int id, string table);
}