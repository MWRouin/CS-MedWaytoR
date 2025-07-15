using MWR.MedWaytoR.PubSub;
using WebApiMedWaytoR.Domain;
using WebApiMedWaytoR.Domain.Events;

namespace WebApiMedWaytoR.Application.CategoriesApp.Events;

public class ProductModifiedCategoryEventSubscriber(ICategoryRepository repository) 
    : IEventSubscriber<ProductModifiedCategoryEvent>
{
    public async Task Handle(ProductModifiedCategoryEvent @event, CancellationToken ct)
    {
        var oldCategory = await repository.GetByName(@event.OldCategory);

        oldCategory?.UpdateNumberOfProducts(oldCategory.NumberOfProducts - 1, DateTime.Now);

        var newCategory = await repository.GetByName(@event.NewCategory);

        if (newCategory == null)
        {
            newCategory = Category.Create(@event.NewCategory, string.Empty, DateTime.Now);
            repository.Add(newCategory);
        }

        newCategory.UpdateNumberOfProducts(newCategory.NumberOfProducts + 1, DateTime.Now);
    }
}