namespace WebApiMedWaytoR.DTOs.Models;

public record CategoryDto(
    string Id,
    string Name,
    string Description,
    string NumberOfProducts,
    string CreatedAt,
    string UpdatedAt);