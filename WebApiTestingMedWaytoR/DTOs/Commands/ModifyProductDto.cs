namespace WebApiMedWaytoR.DTOs.Commands;

public record ModifyProductDto(
    string? Category,
    string? Price,
    string? Description);