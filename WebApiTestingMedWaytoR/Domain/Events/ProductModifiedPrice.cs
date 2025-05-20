using WebApiMedWaytor.Domain.Abstraction;

namespace WebApiMedWaytor.Domain.Events;

public record ProductModifiedPrice(
    decimal OldPrice,
    decimal NewPrice,
    Product Product) 
    : IDomainEvent;