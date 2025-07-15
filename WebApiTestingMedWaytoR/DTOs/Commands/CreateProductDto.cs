namespace WebApiMedWaytoR.DTOs.Commands;

public record CreateProductDto(
    string Name,
    string Category,
    string Description,
    string Price);