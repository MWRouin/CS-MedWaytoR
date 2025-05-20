using WebApiMedWaytor.Domain.Abstraction;

namespace WebApiMedWaytor.Domain;

public interface IProductRepository : IRepository<Product>
{
    public Task<Product[]> GetForCategory(string category);
}