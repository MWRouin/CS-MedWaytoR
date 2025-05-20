using MWR.MedWaytoR.RequestResponse;
using WebApiMedWaytor.Application.Abstract;
using WebApiMedWaytor.Domain;
using WebApiMedWaytor.DTOs;

namespace WebApiMedWaytor.Application.CategoriesApp.Queries;

public record GetAllCategoriesQuery : IQuery<CategoryDto[]>;

public class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetAllCategoriesQuery, CategoryDto[]>
{
    public async Task<CategoryDto[]> Handle(GetAllCategoriesQuery request, CancellationToken ct)
    {
        var categories = await categoryRepository.GetAll();

        return categories.Select(DtoMappers.ToDto).ToArray();
    }
}