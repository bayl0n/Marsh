namespace Marsh.Api.DTOs.Users;

public record UpdateUserDto
(
    string? Username,
    string? FirstName,
    string? LastName
);