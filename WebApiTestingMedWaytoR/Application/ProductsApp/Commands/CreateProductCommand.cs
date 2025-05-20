using WebApiMedWaytor.Application.Abstract;
using WebApiMedWaytor.Domain;

namespace WebApiMedWaytor.Application.ProductsApp.Commands;

public record CreateProductCommand(
    string Name,
    string Category,
    string Description,
    decimal Price)
    : ICommand<Guid>;

public class CreateProductCommandHandler(IProductRepository productRepository)
    : ICommandHandler<CreateProductCommand, Guid>
{
    public Task<Guid> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var product = Product.Create(
            command.Name,
            command.Category,
            command.Description,
            command.Price,
            DateTime.Now);

        productRepository.Add(product);

        return Task.FromResult(product.Id);
    }
}