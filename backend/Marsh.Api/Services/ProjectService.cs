using Marsh.Api.Data;
using Marsh.Api.DTOs.Projects;
using Marsh.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marsh.Api.Services;

public class ProjectService(MarshDbContext context, UserService userService) : ControllerBase
{
    private readonly MarshDbContext _context = context;
    private readonly UserService _userService = userService;

    public async Task<Project> CreateProjectFromUserAsync(string firebaseUid, CreateProjectDto projectDto)
    {
        var user = await _userService.GetByFirebaseUidAsync(firebaseUid);
        
        if (user == null)
            throw new InvalidOperationException("User not found");

        var newProject = new Project
        {
            Title = projectDto.Title,
            Description = projectDto.Description,
            OwnerId = user.Id,
            Owner = user,
            Visibility = projectDto.Visibility ?? "public",
            CreatedAt = DateTime.UtcNow,
        };
        
        _context.Projects.Add(newProject);
        await _context.SaveChangesAsync();
        
        return newProject;
    }

    public async Task<List<ProjectDto>?> GetUserProjectsAsync(int userId)
    {
        var projects = await _context.Projects
            .Where(project => project.Id == userId)
            .Select(
            project => new ProjectDto
                (
                    project.Id,
                    project.Title,
                    project.Description,
                    project.Visibility,
                    project.CreatedAt,
                    project.OwnerId
                )
            ).ToListAsync();
        
        return projects;
    }
}