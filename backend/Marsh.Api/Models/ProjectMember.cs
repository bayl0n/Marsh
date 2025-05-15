using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Marsh.Api.Models;

public class ProjectMember
{
    public int ProjectId { get; set; }
    [JsonIgnore]
    public Project Project { get; set; } = null!;
    
    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; } = null!;
    
    [MaxLength(64)]
    public string Role { get; set; } = "member";
}