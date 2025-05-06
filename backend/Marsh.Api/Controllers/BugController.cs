using Marsh.Api.Data;
using Marsh.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marsh.Api.Controllers;

[ApiController]
[Route("api/bugs")]
public class BugController(MarshDbContext context) : ControllerBase
{
    private readonly MarshDbContext _context = context;

    [HttpGet]
    public IActionResult GetBugs()
    {
        return Ok(_context.Bugs);
    }

    [HttpPost]
    public async Task<IActionResult> PostBug([FromBody] Bug bug)
    {
        _context.Bugs.Add(bug);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetBugs), new { id = bug.Id }, bug);
    }
}