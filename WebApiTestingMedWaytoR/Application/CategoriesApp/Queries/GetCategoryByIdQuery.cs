using WebApiMedWaytor.Application.Abstract;
using WebApiMedWaytor.Domain;
using WebApiMedWaytor.DTOs;

namespace WebApiMedWaytor.Application.CategoriesApp.Queries;

public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<CategoryDto?>;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoryByIdQuery, CategoryDto?>
{
    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        var category = await categoryRepository.GetById(request.CategoryId);

        return category?.ToDto();
    }
}