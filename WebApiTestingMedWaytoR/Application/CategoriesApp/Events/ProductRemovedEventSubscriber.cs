using MWR.MedWaytoR.PubSub;
using WebApiMedWaytoR.Domain;
using WebApiMedWaytoR.Domain.Events;

namespace WebApiMedWaytoR.Application.CategoriesApp.Events;

public class ProductRemovedEventSubscriber(ICategoryRepository repository)
    : IEventSubscriber<ProductRemovedEvent>
{
    public async Task Handle(ProductRemovedEvent @event, CancellationToken ct)
    {
        var category = await repository.GetByName(@event.Product.Category);

        category?.UpdateNumberOfProducts(category.NumberOfProducts - 1, DateTime.Now);
    }
}