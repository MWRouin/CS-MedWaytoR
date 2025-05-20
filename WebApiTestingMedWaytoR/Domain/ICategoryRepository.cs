using WebApiMedWaytor.Domain.Abstraction;

namespace WebApiMedWaytor.Domain;

public interface ICategoryRepository : IRepository<Category>
{
    public Task<Category?> GetByName(string name);
}