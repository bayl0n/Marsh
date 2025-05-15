using System.ComponentModel.DataAnnotations;

namespace Marsh.Api.Models;

public class Project
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; } = null!;
    [MaxLength(2000)]
    public string Description { get; set; } = null!;
    [MaxLength(32)]
    public string Visibility { get; set; } = "public";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    
    public ICollection<ProjectMember> Members { get; set; } = [];
    public ICollection<TicketList> TicketLists { get; set; } = [];
}