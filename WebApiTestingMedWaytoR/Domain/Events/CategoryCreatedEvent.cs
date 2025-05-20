using WebApiMedWaytor.Domain.Abstraction;

namespace WebApiMedWaytor.Domain.Events;

public record CategoryCreatedEvent(Category Category) : IDomainEvent;