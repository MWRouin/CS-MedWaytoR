using WebApiMedWaytor.Domain.Abstraction;

namespace WebApiMedWaytor.Domain.Events;

public record ProductCreatedEvent(Product Product) : IDomainEvent;