using System.Security.Claims;
using AutoMapper;
using Marsh.Api.DTOs.Projects;
using Marsh.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marsh.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProjectsController(ProjectService projectService, UserService userService, IMapper mapper) : ControllerBase
{
    private readonly ProjectService _projectService = projectService;
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;
    
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
        
        var projectDto = _mapper.Map<ProjectDto>(newProject);
        
        return Ok(projectDto);
    }

    [HttpPut("{projectId:int}")]
    public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectDto dto, int projectId)
    {
        var firebaseUid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (firebaseUid == null)
            throw new InvalidOperationException("Firebase uid not found.");
        
        var user = await _userService.GetByFirebaseUidAsync(firebaseUid);
        
        if (user == null)
            throw new InvalidOperationException("Marsh user not found.");
        
        var updatedProject = await _projectService.UpdateUserProjectAsync(projectId, user.Id, dto);
        
        var responseDto = _mapper.Map<ProjectDto>(updatedProject);
        
        return Ok(responseDto);
    }
}

