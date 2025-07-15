using WebApiMedWaytoR.Domain.Abstraction;

namespace WebApiMedWaytoR.Domain.Events;

public record ProductModifiedCategoryEvent(
    string OldCategory,
    string NewCategory,
    Product Product) 
    : IDomainEvent;