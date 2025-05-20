namespace WebApiMedWaytor.DTOs;

public record ProductDto(
    string Id,
    string Name,
    string Category,
    string Description,
    string Price,
    string CreatedAt,
    string UpdatedAt);