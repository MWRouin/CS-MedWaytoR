using WebApiMedWaytor.Extentions;

namespace WebApiMedWaytor.Domain.Abstraction;

public interface IEntity : ICopy
{
    Guid Id { get; }

    List<IDomainEvent> DomainEvents { get; }

    public void Remove();
}