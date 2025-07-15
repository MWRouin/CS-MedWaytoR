using WebApiMedWaytoR.Application.Abstract;
using WebApiMedWaytoR.Domain;
using WebApiMedWaytoR.DTOs.Models;

namespace WebApiMedWaytoR.Application.ProductsApp.Queries;

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