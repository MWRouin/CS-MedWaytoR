using WebApiMedWaytor.Application.Abstract;
using WebApiMedWaytor.Domain;

namespace WebApiMedWaytor.Application.ProductsApp.Commands;

public record ModifyProductCommand(
    Guid ProductId,
    string? Category,
    string? Description,
    decimal? Price)
    : ICommand<Guid>;

public class ModifyProductCommandHandler(IProductRepository productRepository)
    : ICommandHandler<ModifyProductCommand, Guid>
{
    public async Task<Guid> Handle(ModifyProductCommand command, CancellationToken ct)
    {
        var product = await productRepository.GetById(command.ProductId);

        if (product == null)
            throw new KeyNotFoundException($"Product with ID {command.ProductId} not found.");

        if (!string.IsNullOrWhiteSpace(command.Category))
            product.UpdateCategory(command.Category, DateTime.Now);

        if (command.Description is not null)
            product.UpdateDescription(command.Description, DateTime.Now);

        if (command.Price.HasValue)
            product.UpdatePrice(command.Price.Value, DateTime.Now);

        //productRepository.Update(product)

        return product.Id;
    }
}