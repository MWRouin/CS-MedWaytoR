using MWR.MedWaytoR.PubSub;
using WebApiMedWaytoR.Domain;
using WebApiMedWaytoR.Domain.Events;

namespace WebApiMedWaytoR.Application.ProductsApp.Events;

public class CategoryDeletedEventSubscriber(IProductRepository repository)
    : IEventSubscriber<CategoryDeletedEvent>
{
    public async Task Handle(CategoryDeletedEvent @event, CancellationToken ct)
    {
        var products = await repository.GetForCategory(@event.Category.Name);

        foreach (var product in products)
        {
            repository.Delete(product);
        }
    }
}