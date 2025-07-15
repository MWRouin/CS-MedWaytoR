using WebApiMedWaytoR.Domain.Abstraction;

namespace WebApiMedWaytoR.Domain.Events;

public record ProductModifiedPrice(
    decimal OldPrice,
    decimal NewPrice,
    Product Product) 
    : IDomainEvent;