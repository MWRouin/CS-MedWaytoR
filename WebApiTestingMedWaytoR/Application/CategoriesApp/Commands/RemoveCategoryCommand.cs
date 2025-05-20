using WebApiMedWaytor.Application.Abstract;
using WebApiMedWaytor.Domain;
using WebApiMedWaytor.DTOs;

namespace WebApiMedWaytor.Application.CategoriesApp.Commands;

public record RemoveCategoryCommand(Guid CategoryId) : ICommand<CategoryDto>;

public class RemoveCategoryCommandHandler(ICategoryRepository categoryRepository)
    : ICommandHandler<RemoveCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(RemoveCategoryCommand command, CancellationToken ct)
    {
        var category = await categoryRepository.GetById(command.CategoryId);
        if (category is null)
            throw new KeyNotFoundException($"Category with id {command.CategoryId} not found");

        categoryRepository.Delete(category);

        return category.ToDto();
    }
}