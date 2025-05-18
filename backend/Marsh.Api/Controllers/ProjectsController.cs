using System.Security.Claims;
using Marsh.Api.DTOs.Projects;
using Marsh.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marsh.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProjectsController(ProjectService projectService, UserService userService) : ControllerBase
{
    private readonly ProjectService _projectService = projectService;
    private readonly UserService _userService = userService;
    
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var firebaseUid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (firebaseUid == null)
            throw new InvalidOperationException("Firebase user uid not found.");
        
        var user = await _userService.GetByFirebaseUidAsync(firebaseUid);
        
        if (user == null)
            throw new InvalidOperationException("Marsh user not found.");

        var projects = await _projectService.GetUserProjectsAsync(user.Id);
        
        return Ok(projects);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
    {
        var firebaseUid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (firebaseUid == null)
            throw new InvalidOperationException("Firebase user uid not found.");
        
        var newProject = await _projectService.CreateProjectFromUserAsync(firebaseUid, dto);

        var projectDto = new ProjectDto
        (
            newProject.Id,
            newProject.Title,
            newProject.Description,
            newProject.Visibility,
            newProject.CreatedAt,
            newProject.OwnerId
        );
        
        return Ok(projectDto);
    }
}

