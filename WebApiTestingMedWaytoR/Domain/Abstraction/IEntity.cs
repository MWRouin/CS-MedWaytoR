using WebApiMedWaytoR.Extentions;

namespace WebApiMedWaytoR.Domain.Abstraction;

public interface IEntity : ICopy
{
    Guid Id { get; }

    List<IDomainEvent> DomainEvents { get; }

    public void Remove();
}