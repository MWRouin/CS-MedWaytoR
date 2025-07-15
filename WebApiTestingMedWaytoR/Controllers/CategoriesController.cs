using Microsoft.AspNetCore.Mvc;
using MWR.MedWaytoR.RequestResponse;
using WebApiMedWaytoR.Application.CategoriesApp.Commands;
using WebApiMedWaytoR.Application.CategoriesApp.Queries;
using WebApiMedWaytoR.DTOs.Commands;

namespace WebApiMedWaytoR.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(IRequestDispatcher requestDispatcher) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await requestDispatcher.DispatchAsync(new GetAllCategoriesQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var result = await requestDispatcher.DispatchAsync(new GetCategoryByIdQuery(id));
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetCategoryById(string name)
    {
        var result = await requestDispatcher.DispatchAsync(new GetCategoryByNameQuery(name));
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto command)
    {
        var result =
            await requestDispatcher.DispatchAsync(new CreateCategoryCommand(command.Name, command.Description));
        return CreatedAtAction(nameof(GetCategoryById), new { id = result }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] ModifyCategoryDto command)
    {
        await requestDispatcher.DispatchAsync(new ModifyCategoryCommand(id, command.Description));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        await requestDispatcher.DispatchAsync(new RemoveCategoryCommand(id));
        return NoContent();
    }
}