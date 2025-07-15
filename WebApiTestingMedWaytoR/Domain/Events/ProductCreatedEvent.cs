using WebApiMedWaytoR.Domain.Abstraction;

namespace WebApiMedWaytoR.Domain.Events;

public record ProductCreatedEvent(Product Product) : IDomainEvent;