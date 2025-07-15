using WebApiMedWaytoR.Domain.Abstraction;
using WebApiMedWaytoR.Domain.Events;
using WebApiMedWaytoR.Extentions;

namespace WebApiMedWaytoR.Domain;

public class Product : IEntity, ICopy<Product>
{
    private Product(
        Guid id,
        string name,
        string category,
        string description,
        decimal price,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        Name = name;
        Category = category;
        Description = description;
        Price = price;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Category { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }
    public List<IDomainEvent> DomainEvents { get; } = [];

    public void UpdateDescription(string description, DateTime updatedAt)
    {
        Description = description;
        UpdatedAt = updatedAt;
    }

    public void UpdatePrice(decimal price, DateTime updatedAt)
    {
        var oldPrice = Copier.Copy(Price);
        Price = price;
        UpdatedAt = updatedAt;
        DomainEvents.Add(new ProductModifiedPrice(oldPrice, price, this));
    }

    public void UpdateCategory(string category, DateTime updatedAt)
    {
        var oldCategory = Copier.Copy(Category);
        Category = category;
        UpdatedAt = updatedAt;
        DomainEvents.Add(new ProductModifiedCategoryEvent(oldCategory, category, this));
    }

    public void Remove()
    {
        DomainEvents.Add(new ProductRemovedEvent(this));
    }

    public Product Copy()
    {
        return new Product(
            Copier.Copy(Id),
            Copier.Copy(Name),
            Copier.Copy(Category),
            Copier.Copy(Description),
            Copier.Copy(Price),
            Copier.Copy(CreatedAt),
            UpdatedAt.HasValue ? Copier.Copy(UpdatedAt.Value) : null);
    }

    public static Product Create(
        string name,
        string category,
        string description,
        decimal price,
        DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be null or empty.", nameof(category));

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));

        var product = new Product(
            Guid.NewGuid(),
            name,
            category,
            description,
            price,
            createdAt,
            null);

        product.DomainEvents.Add(new ProductCreatedEvent(product));

        return product;
    }
}