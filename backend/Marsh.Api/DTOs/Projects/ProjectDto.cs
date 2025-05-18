namespace Marsh.Api.DTOs.Projects;

public record ProjectDto
(
    int Id,
    string Title,
    string? Description,
    string? Visibility,
    DateTime CreatedAt,
    int OwnerId
);