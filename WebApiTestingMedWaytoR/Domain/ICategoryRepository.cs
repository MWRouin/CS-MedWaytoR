using WebApiMedWaytoR.Domain.Abstraction;

namespace WebApiMedWaytoR.Domain;

public interface ICategoryRepository : IRepository<Category>
{
    public Task<Category?> GetByName(string name);
}