using WebApiMedWaytor.Domain;

namespace WebApiMedWaytor.Infrastructure;

public class CategoryRepository(InMemoryDbContext dbContext)
    : GenericRepository<Category>(dbContext), ICategoryRepository
{
    public async Task<Category?> GetByName(string name)
    {
        await Task.Delay(44);
        return DbContext.GetAll<Category>().FirstOrDefault(x => x.Name == name);
    }
}