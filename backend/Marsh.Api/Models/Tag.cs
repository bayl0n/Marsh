using System.ComponentModel.DataAnnotations;

namespace Marsh.Api.Models;

public class Tag
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(255)]
    public string Title { get; set; } = null!;
    
    [MaxLength(8)]
    public string Color { get; set; } = "#AAAAFF";
    
    public ICollection<TagList> Tickets { get; set; } = [];
}