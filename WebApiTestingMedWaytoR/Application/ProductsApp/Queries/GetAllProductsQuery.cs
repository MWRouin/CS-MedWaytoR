using WebApiMedWaytor.Application.Abstract;
using WebApiMedWaytor.Domain;
using WebApiMedWaytor.DTOs;

namespace WebApiMedWaytor.Application.ProductsApp.Queries;

public record GetAllProductsQuery : IQuery<ProductDto[]>;

public class GetAllProductsQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetAllProductsQuery, ProductDto[]>
{
    public async Task<ProductDto[]> Handle(GetAllProductsQuery request, CancellationToken ct)
    {
        var products = await productRepository.GetAll();

        return products.Select(DtoMappers.ToDto).ToArray();
    }
}