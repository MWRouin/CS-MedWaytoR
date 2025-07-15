namespace WebApiMedWaytoR.Domain.Abstraction;

public interface IRepository<T> where T : class, IEntity
{
    public Task<T?> GetById(Guid id);

    public Task<T[]> GetAll();

    public void Add(T entity);

    public void Update(T entity);

    public void Delete(T entity);
}