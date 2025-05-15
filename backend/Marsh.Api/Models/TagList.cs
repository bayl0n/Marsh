using Newtonsoft.Json;

namespace Marsh.Api.Models;

public class TagList
{
    public int TicketId { get; set; }
    
    [JsonIgnore]
    public Ticket Ticket { get; set; } = null!;
    
    public int TagId { get; set; }
    
    [JsonIgnore]
    public Tag Tag { get; set; } = null!;
}