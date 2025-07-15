using System.Globalization;
using WebApiMedWaytoR.Domain;
using WebApiMedWaytoR.DTOs.Models;

namespace WebApiMedWaytoR.Application;

public static class DtoMappers
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto(
            category.Id.ToString(),
            category.Name,
            category.Description,
            category.NumberOfProducts.ToString(),
            category.CreatedAt.ToString(CultureInfo.InvariantCulture),
            category.UpdatedAt?.ToString(CultureInfo.InvariantCulture) ?? string.Empty
        );
    }

    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Id.ToString(),
            product.Name,
            product.Category,
            product.Description,
            product.Price.ToString("C", CultureInfo.InvariantCulture),
            product.CreatedAt.ToString(CultureInfo.InvariantCulture),
            product.UpdatedAt?.ToString(CultureInfo.InvariantCulture) ?? string.Empty
        );
    }
}