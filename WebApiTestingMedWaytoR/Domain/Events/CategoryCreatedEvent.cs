using WebApiMedWaytoR.Domain.Abstraction;

namespace WebApiMedWaytoR.Domain.Events;

public record CategoryCreatedEvent(Category Category) : IDomainEvent;