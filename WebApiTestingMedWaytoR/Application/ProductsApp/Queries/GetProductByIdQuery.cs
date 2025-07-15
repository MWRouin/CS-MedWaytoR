using WebApiMedWaytoR.Application.Abstract;
using WebApiMedWaytoR.Domain;
using WebApiMedWaytoR.DTOs.Models;

namespace WebApiMedWaytoR.Application.ProductsApp.Queries;

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