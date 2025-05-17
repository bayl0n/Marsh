using Marsh.Api.Data;
using Marsh.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marsh.Api.Services;

public class ProjectService(MarshDbContext context, UserService userService) : ControllerBase
{
    private readonly MarshDbContext _context = context;
    private readonly UserService _userService = userService;

    // public async Task<Project> CreateProjectFromUser(string firebaseUid)
    // {
    //     var newProject = new Project();
    // }

    public async Task<List<Project>?> GetUserProjects(int userId)
    {
        var projects = await _context.Projects.Where(project => project.Id == userId).ToListAsync();
        
        return projects;
    }
}