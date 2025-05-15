using System.ComponentModel.DataAnnotations;

namespace Marsh.Api.Models;

public class TicketList
{
    public int Id { get; set; }
    
    [MaxLength(255)]
    public string Title { get; set; } = null!;
    
    [MaxLength(2000)]
    public string Description { get; set; } = null!;
    
    public Boolean Archived { get; set; } = false;
    
    public double Position { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    
    public ICollection<Ticket> Tickets { get; set; } = [];
}