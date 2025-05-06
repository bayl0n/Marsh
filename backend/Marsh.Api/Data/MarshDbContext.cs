using Marsh.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Marsh.Api.Data;

public class MarshDbContext(DbContextOptions<MarshDbContext> options) : DbContext(options)
{
    public DbSet<Bug> Bugs { get; set; } = null!;
}