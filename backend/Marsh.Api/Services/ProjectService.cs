using AutoMapper;
using Marsh.Api.Data;
using Marsh.Api.DTOs.Projects;
using Marsh.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marsh.Api.Services;

public class ProjectService(MarshDbContext context, UserService userService, IMapper mapper) : ControllerBase
{
    private readonly MarshDbContext _context = context;
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;

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
            .Where(project => project.OwnerId == userId)
            .Select(
            project => _mapper.Map<ProjectDto>(project)
            ).ToListAsync();
        
        return projects;
    }

    public async Task<Project?> UpdateUserProjectAsync(int projectId, int ownerId, UpdateProjectDto projectDto)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(project => project.Id == projectId);
        
        if (project == null || project.OwnerId != ownerId)
            throw new InvalidOperationException("Project not found or access denied");
        
        _mapper.Map(projectDto, project);
        
        project.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        
        return project;
    }
}