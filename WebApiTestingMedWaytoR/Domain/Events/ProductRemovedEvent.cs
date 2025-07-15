using WebApiMedWaytoR.Domain.Abstraction;

namespace WebApiMedWaytoR.Domain.Events;

public record ProductRemovedEvent(Product Product) : IDomainEvent;