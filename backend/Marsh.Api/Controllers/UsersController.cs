using System.Security.Claims;
using Marsh.Api.DTOs.Users;
using Marsh.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marsh.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var firebaseUid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(firebaseUid))
            return Unauthorized("Missing Firebase UID");

        var user = await userService.GetByFirebaseUidAsync(firebaseUid);
        if (user == null)
            return NotFound("User not found");

        var userDto = new UserDto(
            user.Id,
            user.Username,
            user.Email,
            user.FirstName,
            user.LastName,
            user.CreatedAt,
            user.UpdatedAt
        );

        return Ok(userDto);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
            return NotFound("User not found");

        return Ok(user);
    }
}