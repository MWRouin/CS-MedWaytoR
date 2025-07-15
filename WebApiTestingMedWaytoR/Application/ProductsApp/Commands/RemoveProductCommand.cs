using WebApiMedWaytoR.Application.Abstract;
using WebApiMedWaytoR.Domain;

namespace WebApiMedWaytoR.Application.ProductsApp.Commands;

public record RemoveProductCommand(Guid ProductId) : ICommand<Guid>;

public class RemoveProductCommandHandler(IProductRepository productRepository)
    : ICommandHandler<RemoveProductCommand, Guid>
{
    public async Task<Guid> Handle(RemoveProductCommand command, CancellationToken ct)
    {
        var product = await productRepository.GetById(command.ProductId);

        if (product is null)
            throw new KeyNotFoundException($"Product with ID {command.ProductId} not found.");

        productRepository.Delete(product);

        return product.Id;
    }
}