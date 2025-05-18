namespace Marsh.Api.DTOs.Projects;

public record CreateProjectDto
(
    string Title,
    string? Description,
    string? Visibility
);