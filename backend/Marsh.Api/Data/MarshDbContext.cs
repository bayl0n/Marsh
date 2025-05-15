using Marsh.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Marsh.Api.Data;

public class MarshDbContext(DbContextOptions<MarshDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ProjectMember> ProjectMembers { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;
    public DbSet<TicketList> TicketLists { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<TagList> TagLists { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // composite keys
        modelBuilder.Entity<ProjectMember>()
            .HasKey(pm => new { pm.UserId, pm.ProjectId });

        modelBuilder.Entity<TagList>()
            .HasKey(tl => new { tl.TagId, tl.TicketId });
        
        // unique index on username
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        
        // unique index on firebase uid
        modelBuilder.Entity<User>()
            .HasIndex(u => u.FirebaseUid)
            .IsUnique();
        
        // unique index on user email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        // relationships
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.OwnedProjects)
            .HasForeignKey(fk => fk.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.AssignedToUser)
            .WithMany()
            .HasForeignKey(fk => fk.AssignedToUserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.TicketList)
            .WithMany(tl => tl.Tickets)
            .HasForeignKey(fk => fk.TicketListId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<TicketList>()
            .HasOne(tl => tl.Project)
            .WithMany(p => p.TicketLists)
            .HasForeignKey(fk => fk.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}