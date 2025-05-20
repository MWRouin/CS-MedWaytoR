using WebApiMedWaytor.Domain.Abstraction;

namespace WebApiMedWaytor.Infrastructure;

public class GenericRepository<T>(InMemoryDbContext dbContext)
    : IRepository<T> where T : class, IEntity
{
    protected readonly InMemoryDbContext DbContext = dbContext;

    public async Task<T?> GetById(Guid id)
    {
        await Task.Delay(33);
        return DbContext.Get<T>(id);
    }

    public async Task<T[]> GetAll()
    {
        await Task.Delay(66);
        return DbContext.GetAll<T>().ToArray();
    }

    public void Add(T entity)
    {
        DbContext.Insert(entity);
    }

    public void Update(T entity)
    {
        DbContext.Update(entity);
    }

    public void Delete(T entity)
    {
        entity.Remove();
        DbContext.Delete(entity);
    }
}