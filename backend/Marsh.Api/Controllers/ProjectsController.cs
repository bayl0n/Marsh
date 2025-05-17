using Marsh.Api.Data;
using Marsh.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marsh.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProjectsController(ProjectService projectService, UserService userService) : ControllerBase
{
    
}

