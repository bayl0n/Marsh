using System.Security.Claims;
using AutoMapper;
using Marsh.Api.DTOs.Users;
using Marsh.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marsh.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(UserService userService, IMapper mapper) : ControllerBase
{
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;
    
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var firebaseUid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(firebaseUid))
            return Unauthorized("Missing Firebase UID");

        var email = User.FindFirstValue(ClaimTypes.Email);
        var user = await _userService.GetOrCreateByFirebaseUidAsync(firebaseUid, email);

        var dto = _mapper.Map<UserDto>(user);
        return Ok(dto);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null)
            return NotFound("User not found");
        
        var userDto = _mapper.Map<UserDto>(user);

        return Ok(userDto);
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserDto dto)
    {
        var firebaseUid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(firebaseUid))
            return Unauthorized("Missing Firebase UID");
        
        var user = await _userService.GetByFirebaseUidAsync(firebaseUid);
        
        if (user == null)
            return NotFound("User not found");

        user.UpdatedAt = DateTime.UtcNow;
        var newUser = await _userService.UpdateUserAsync(user.Id, dto);
        
        var newUserDto = _mapper.Map<UserDto>(newUser);
        
        return Ok(newUserDto);
    }
}