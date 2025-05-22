using AutoMapper;
using Marsh.Api.Data;
using Marsh.Api.DTOs.Users;
using Marsh.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Marsh.Api.Services;

public class UserService(MarshDbContext context, IMapper mapper)
{
    private readonly MarshDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<User?> GetByFirebaseUidAsync(string firebaseUid)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid);
    }
    
    public async Task<User> CreateAsync(string firebaseUid, string? email)
    {
        var user = new User {
            FirebaseUid = firebaseUid,
            Email       = email,
            Username    = $"user_{Guid.NewGuid().ToString()[..8]}",
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = DateTime.UtcNow
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task<User> GetOrCreateByFirebaseUidAsync(string firebaseUid, string? email)
    {
        var user = await GetByFirebaseUidAsync(firebaseUid);
        if (user != null) return user;
        return await CreateAsync(firebaseUid, email);
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

    public async Task<User?> GetUserAsync(int marshId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == marshId);

        return user;
    }

    public async Task<User?> UpdateUserAsync(int marshId, UpdateUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == marshId);
        
        _mapper.Map(dto, user);
        await _context.SaveChangesAsync();
        
        return user;
    }
}