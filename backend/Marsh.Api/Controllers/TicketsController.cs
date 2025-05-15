using Marsh.Api.Data;
using Marsh.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marsh.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TicketsController(MarshDbContext context) : ControllerBase
{
    private readonly MarshDbContext _context = context;

    [HttpGet]
    public IActionResult GetTickets()
    {
        return Ok(_context.Tickets);
    }

    [HttpPost]
    public async Task<IActionResult> PostTicket([FromBody] Ticket ticket)
    {
        // TODO: Create a CreateTicketDto to handle this better
        ticket.Id = 0;
        ticket.IsResolved = false;
        ticket.IsArchived = false;
        ticket.CreatedAt = DateTime.UtcNow;

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetTickets), new { id = ticket.Id }, ticket);
    }

    [HttpPut]
    public async Task<IActionResult> PutTicket([FromBody] Ticket ticket)
    {
        ticket.UpdatedAt = DateTime.UtcNow;
        
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
        
        return Ok(ticket);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        
        if (ticket == null)
        {
            return NotFound();
        }
        
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}