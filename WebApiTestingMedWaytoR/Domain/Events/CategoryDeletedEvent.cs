using WebApiMedWaytor.Domain.Abstraction;

namespace WebApiMedWaytor.Domain.Events;

public record CategoryDeletedEvent(Category Category) : IDomainEvent;