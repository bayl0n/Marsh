using Marsh.Api.Data;
using Marsh.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Marsh.Api.Services;

public class UserService(MarshDbContext context)
{
    private readonly MarshDbContext _context = context;

    public async Task<User?> GetByFirebaseUidAsync(string firebaseUid)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid);
    }

    public async Task<User> SyncFirebaseUserAsync(string firebaseUid, string? email, string? username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid);

        if (user != null)
        {
            return user;
        }

        var newUser = new User
        {
            FirebaseUid = firebaseUid,
            Email = email,
            Username = username ?? $"user_{Guid.NewGuid().ToString()[..8]}",
        };
        
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        
        return newUser;
    }
}