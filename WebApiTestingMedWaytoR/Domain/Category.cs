using WebApiMedWaytor.Domain.Abstraction;
using WebApiMedWaytor.Domain.Events;
using WebApiMedWaytor.Extentions;

namespace WebApiMedWaytor.Domain;

public class Category : IEntity, ICopy<Category>
{
    private Category(
        Guid id,
        string name,
        string description,
        int numberOfProducts,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        Name = name;
        Description = description;
        NumberOfProducts = numberOfProducts;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; private set; }
    public int NumberOfProducts { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }
    public List<IDomainEvent> DomainEvents { get; } = [];

    public void UpdateDescription(string description, DateTime updatedAt)
    {
        Description = description;
        UpdatedAt = updatedAt;
    }

    public void UpdateNumberOfProducts(int numberOfProducts, DateTime updatedAt)
    {
        if (numberOfProducts < 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfProducts), "Number of products cannot be negative.");
        NumberOfProducts = numberOfProducts;
        UpdatedAt = updatedAt;
    }

    public void Remove()
    {
        DomainEvents.Add(new CategoryDeletedEvent(this));
    }

    public Category Copy()
    {
        return new Category(
            Copier.Copy(Id),
            Copier.Copy(Name),
            Copier.Copy(Description),
            Copier.Copy(NumberOfProducts),
            Copier.Copy(CreatedAt),
            UpdatedAt.HasValue ? Copier.Copy(UpdatedAt.Value) : null);
    }

    public static Category Create(string name, string description, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        var category = new Category(
            Guid.NewGuid(),
            name,
            description,
            0,
            createdAt,
            null);

        category.DomainEvents.Add(new CategoryCreatedEvent(category));

        return category;
    }
}