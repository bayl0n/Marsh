using Microsoft.EntityFrameworkCore;

namespace Marsh.Api.Models;

public class MarshDbContext(DbContextOptions<MarshDbContext> options) : DbContext(options)
{
    public DbSet<Bug> Bugs { get; set; } = null!;
}