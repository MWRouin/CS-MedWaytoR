using WebApiMedWaytoR.Domain;

namespace WebApiMedWaytoR.Infrastructure;

public class ProductRepository(InMemoryDbContext dbContext)
    : GenericRepository<Product>(dbContext), IProductRepository
{
    public async Task<Product[]> GetForCategory(string category)
    {
        await Task.Delay(44);
        return DbContext.GetAll<Product>().Where(x => x.Category == category).ToArray();
    }
}