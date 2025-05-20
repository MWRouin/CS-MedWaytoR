using MWR.MedWaytoR.PubSub;
using WebApiMedWaytor.Domain;
using WebApiMedWaytor.Domain.Events;

namespace WebApiMedWaytor.Application.CategoriesApp.Events;

public class ProductCreatedEventSubscriber(ICategoryRepository repository)
    : IEventSubscriber<ProductCreatedEvent>
{
    public async Task Handle(ProductCreatedEvent @event, CancellationToken ct)
    {
        var categoryName = @event.Product.Category;

        var category = await repository.GetByName(categoryName);

        if (category == null)
        {
            category = Category.Create(categoryName, string.Empty, DateTime.Now);
            repository.Add(category);
        }

        category.UpdateNumberOfProducts(1, DateTime.Now);
    }
}