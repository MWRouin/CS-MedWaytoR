using WebApiMedWaytoR.Domain.Abstraction;

namespace WebApiMedWaytoR.Domain;

public interface IProductRepository : IRepository<Product>
{
    public Task<Product[]> GetForCategory(string category);
}