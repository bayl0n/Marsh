namespace Marsh.Api.DTOs.Projects;

public record UpdateProjectDto
(
    string? Title,
    string? Description,
    string? Visibility
);