using WebApiMedWaytor.Application.Abstract;
using WebApiMedWaytor.Domain;
using WebApiMedWaytor.DTOs;

namespace WebApiMedWaytor.Application.CategoriesApp.Queries;

public record GetCategoryByNameQuery(string Name) : IQuery<CategoryDto?>;

public class GetCategoryByNameQueryHandler(ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoryByNameQuery, CategoryDto?>
{
    public async Task<CategoryDto?> Handle(GetCategoryByNameQuery request, CancellationToken ct)
    {
        var category = await categoryRepository.GetByName(request.Name);

        return category?.ToDto();
    }
}