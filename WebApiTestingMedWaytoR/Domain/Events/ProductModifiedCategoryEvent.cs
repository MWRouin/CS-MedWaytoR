using WebApiMedWaytor.Domain.Abstraction;

namespace WebApiMedWaytor.Domain.Events;

public record ProductModifiedCategoryEvent(
    string OldCategory,
    string NewCategory,
    Product Product) 
    : IDomainEvent;