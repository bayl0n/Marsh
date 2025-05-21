namespace Marsh.Api.DTOs.Users;

public record CreateUserDto
(
    string? Email,
    string? Username,
    string? FirstName,
    string? LastName
);