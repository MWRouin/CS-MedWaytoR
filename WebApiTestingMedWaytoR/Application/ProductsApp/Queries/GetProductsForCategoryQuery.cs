using WebApiMedWaytoR.Application.Abstract;
using WebApiMedWaytoR.Domain;
using WebApiMedWaytoR.DTOs.Models;

namespace WebApiMedWaytoR.Application.ProductsApp.Queries;

public record GetProductsForCategoryQuery(string Category) : IQuery<ProductDto[]>;

public class GetProductsForCategoryQueryHandler(IProductRepository productRepository)
    : IQueryHandler<GetProductsForCategoryQuery, ProductDto[]>
{
    public async Task<ProductDto[]> Handle(GetProductsForCategoryQuery request, CancellationToken ct)
    {
        var products = (await productRepository.GetAll())
            .Where(p => string.Equals(p.Category, request.Category, StringComparison.InvariantCultureIgnoreCase));

        return products.Select(DtoMappers.ToDto).ToArray();
    }
}