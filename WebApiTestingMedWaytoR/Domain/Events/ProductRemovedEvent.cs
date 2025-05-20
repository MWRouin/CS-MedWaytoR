using WebApiMedWaytor.Domain.Abstraction;

namespace WebApiMedWaytor.Domain.Events;

public record ProductRemovedEvent(Product Product) : IDomainEvent;