using WebApiMedWaytor.Application.Abstract;
using WebApiMedWaytor.Domain;
using WebApiMedWaytor.DTOs;

namespace WebApiMedWaytor.Application.ProductsApp.Queries;

public record GetProductByIdQuery(Guid ProductId) : IQuery<ProductDto?>;

public class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await productRepository.GetById(request.ProductId);

        return product?.ToDto();
    }
}