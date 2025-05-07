using System.ComponentModel.DataAnnotations;

namespace Marsh.Api.Models;
public class Ticket
{
    public int Id { get; set; }
    
    [MaxLength(255)]
    public string? Title { get; set; }
    
    [MaxLength(2000)]
    public string? Description { get; set; }
    
    public bool IsResolved { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
