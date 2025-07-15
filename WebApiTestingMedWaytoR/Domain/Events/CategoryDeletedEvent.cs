using WebApiMedWaytoR.Domain.Abstraction;

namespace WebApiMedWaytoR.Domain.Events;

public record CategoryDeletedEvent(Category Category) : IDomainEvent;