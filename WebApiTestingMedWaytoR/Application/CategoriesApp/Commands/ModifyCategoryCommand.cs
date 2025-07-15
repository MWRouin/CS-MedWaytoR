using WebApiMedWaytoR.Application.Abstract;
using WebApiMedWaytoR.Domain;

namespace WebApiMedWaytoR.Application.CategoriesApp.Commands;

public record ModifyCategoryCommand(Guid Id, string Description) : ICommand<string>;

public class ModifyCategoryCommandHandler(ICategoryRepository categoryRepository)
    : ICommandHandler<ModifyCategoryCommand, string>
{
    public async Task<string> Handle(ModifyCategoryCommand command, CancellationToken ct)
    {
        var category = await categoryRepository.GetById(command.Id);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {command.Id} not found.");
        }

        category.UpdateDescription(command.Description, DateTime.Now);
        //categoryRepository.Update(category)
        return category.Description;
    }
}