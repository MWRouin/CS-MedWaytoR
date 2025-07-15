using WebApiMedWaytoR.Application.Abstract;
using WebApiMedWaytoR.Domain;

namespace WebApiMedWaytoR.Application.CategoriesApp.Commands;

public record CreateCategoryCommand(string Name, string Description) : ICommand<Guid>;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    public Task<Guid> Handle(CreateCategoryCommand command, CancellationToken ct)
    {
        var category = Category.Create(command.Name, command.Description, DateTime.Now);
        categoryRepository.Add(category);
        return Task.FromResult(category.Id);
    }
}