using System.Data;
using WebApiMedWaytor.Domain;
using WebApiMedWaytor.Domain.Abstraction;
using WebApiMedWaytor.Extentions;

namespace WebApiMedWaytor.Infrastructure;

public class InMemoryDbContext
{
    private Dictionary<Type, Dictionary<Guid, IEntity>> _tables = InMemoryDb.GetCopy();

    public IEntity[] ModifiedEntities() => _entities.Values.ToArray();

    private Dictionary<Guid, IEntity> _entities = [];

    public void CreateTable<T>() where T : class, IEntity
    {
        _tables.Add(typeof(T), new Dictionary<Guid, IEntity>());
    }

    public void RemoveTable<T>() where T : class, IEntity
    {
        if (!_tables.Remove(typeof(T)))
        {
            throw new InvalidOperationException($"Table for type {typeof(T).Name} does not exist.");
        }
    }

    public Dictionary<Guid, IEntity> GetTable<T>() where T : class, IEntity
    {
        if (!_tables.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException($"Table for type {typeof(T).Name} does not exist.");
        }

        return _tables[typeof(T)];
    }

    public T? Get<T>(Guid id) where T : class, IEntity
    {
        var table = GetTable<T>();

        if (!table.TryGetValue(id, out var value)) return null;

        _entities.TryAdd(value.Id, value);

        return (T)value;
    }

    public IEnumerable<T> GetAll<T>() where T : class, IEntity
    {
        var table = GetTable<T>();
        var records = table.Values.Cast<T>().ToArray();
        foreach (var record in records)
            _entities.TryAdd(record.Id, record);
        return records;
    }

    public void Insert<T>(T item) where T : class, IEntity
    {
        var table = GetTable<T>();
        var id = item.Id;
        if (!table.TryAdd(id, item))
        {
            // Uncomment the following line if you want to throw an exception when a duplicate ID is added
            throw new DuplicateNameException($"Item with ID {id} already exists in table {typeof(T).Name}.");
            // Alternatively, you can log a warning or handle it in another way
            // Console.WriteLine($"Warning: Item with ID {id} already exists in table {typeof(T).Name}.")
        }

        _entities.TryAdd(item.Id, item);
    }

    public void Update<T>(T item) where T : class, IEntity
    {
        var table = GetTable<T>();

        if (table.ContainsKey(item.Id))
        {
            table[item.Id] = item;

            _entities.TryAdd(item.Id, item);
        }
        else
        {
            throw new KeyNotFoundException($"Item with ID {item.Id} not found in table {typeof(T).Name}.");
        }
    }

    public void Delete<T>(T item) where T : class, IEntity
    {
        var table = GetTable<T>();

        if (!table.Remove(item.Id))
        {
            throw new KeyNotFoundException($"Item with ID {item.Id} not found in table {typeof(T).Name}.");
        }

        _entities.TryAdd(item.Id, item);
    }

    public async Task SaveChanges()
    {
        await Task.Delay(99);
        InMemoryDb.Commit(_tables);
        _tables = InMemoryDb.GetCopy();
        _entities = [];
    }
}

file static class InMemoryDb
{
    private static Dictionary<Type, Dictionary<Guid, IEntity>> _tables = new()
    {
        { typeof(Category), [] },
        { typeof(Product), [] },
    };

    public static void Commit(Dictionary<Type, Dictionary<Guid, IEntity>> tables)
    {
        _tables = tables;
    }

    public static Dictionary<Type, Dictionary<Guid, IEntity>> GetCopy()
    {
        Dictionary<Type, Dictionary<Guid, IEntity>> copy = [];

        foreach (var (tableType, dictionary) in _tables)
        {
            var tableCopy = new Dictionary<Guid, IEntity>();

            foreach (var (guid, entity) in dictionary)
            {
                var guidCopy = Copier.Copy(guid);
                var entityCopy = (IEntity)Copier.Copy(entity);

                tableCopy.Add(guidCopy, entityCopy);
            }

            copy.Add(tableType, tableCopy);
        }

        return copy;
    }
}