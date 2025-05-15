using System.ComponentModel.DataAnnotations;

namespace Marsh.Api.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(128)]
    public string FirebaseUid { get; set; } = null!;
    
    [MaxLength(64)]
    public string? Username { get; set; }
    
    [MaxLength(128)]
    public string? Email { get; set; }
    
    [MaxLength(64)]
    public string? FirstName { get; set; }
    
    [MaxLength(64)]
    public string? LastName { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Project> OwnedProjects { get; set; } = [];
    public ICollection<ProjectMember> ProjectMemberships { get; set; } = [];
}