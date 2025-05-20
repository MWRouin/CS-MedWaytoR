namespace WebApiMedWaytor.DTOs;

public record CategoryDto(
    string Id,
    string Name,
    string Description,
    string NumberOfProducts,
    string CreatedAt,
    string UpdatedAt);