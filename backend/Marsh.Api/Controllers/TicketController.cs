using Marsh.Api.Data;
using Marsh.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marsh.Api.Controllers;

[ApiController]
[Route("api/v1/bugs")]
public class TaskController(MarshDbContext context) : ControllerBase
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

    [HttpPut]
    public async Task<IActionResult> PutBug([FromBody] Bug bug)
    {
        _context.Bugs.Update(bug);
        await _context.SaveChangesAsync();
        
        return Ok(bug);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBug(int id)
    {
        var bug = await _context.Bugs.FindAsync(id);
        
        if (bug == null)
        {
            return NotFound();
        }
        
        _context.Bugs.Remove(bug);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}