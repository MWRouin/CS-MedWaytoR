using Microsoft.AspNetCore.Mvc;
using MWR.MedWaytoR.RequestResponse;
using WebApiMedWaytoR.Application.ProductsApp.Commands;
using WebApiMedWaytoR.Application.ProductsApp.Queries;
using WebApiMedWaytoR.DTOs.Commands;

namespace WebApiMedWaytoR.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IRequestDispatcher requestDispatcher) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var result = await requestDispatcher.DispatchAsync(new GetAllProductsQuery());
        return Ok(result);
    }

    [HttpGet("category/{categoryName}")]
    public async Task<IActionResult> GetProductsForCategory(string categoryName)
    {
        var result = await requestDispatcher.DispatchAsync(new GetProductsForCategoryQuery(categoryName));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var result = await requestDispatcher.DispatchAsync(new GetProductByIdQuery(id));

        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto command)
    {
        if (!decimal.TryParse(command.Price, out var price))
            return BadRequest();

        var result = await requestDispatcher.DispatchAsync(new CreateProductCommand(
            command.Name,
            command.Category,
            command.Description,
            price));

        return CreatedAtAction(nameof(GetProductById), new { id = result }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ModifyProductDto command)
    {
        decimal? price = null;
        if (!string.IsNullOrWhiteSpace(command.Price))
            if (!decimal.TryParse(command.Price, out var parsedPrice))
                return BadRequest();
            else
                price = parsedPrice;

        await requestDispatcher.DispatchAsync(new ModifyProductCommand(
            id,
            command.Category,
            command.Description,
            price));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await requestDispatcher.DispatchAsync(new RemoveProductCommand(id));
        return NoContent();
    }
}