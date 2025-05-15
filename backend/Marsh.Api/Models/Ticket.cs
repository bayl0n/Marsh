using System.ComponentModel.DataAnnotations;

namespace Marsh.Api.Models;
public class Ticket
{
    [Key]
    public int Id { get; set; }

    [MaxLength(255)] public string Title { get; set; } = null!;
    
    [MaxLength(2000)]
    public string Description { get; set; } = null!;

    public bool IsResolved { get; set; }
    public bool IsArchived { get; set; }

    [MaxLength(16)]
    public string Priority { get; set; } = null!;
    
    [MaxLength(32)]
    public string Status { get; set; } = null!;

    public double Position { get; set; } = 0;
    
    public DateTime? DueDate { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public int TicketListId { get; set; }
    public TicketList TicketList { get; set; } = null!;
    
    public int? AssignedToUserId { get; set; }
    public User AssignedToUser { get; set; } = null!;
    
    public ICollection<TagList> Tags { get; set; } = [];
}
