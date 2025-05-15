using Marsh.Api.Data;
using Marsh.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marsh.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService;

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var firebaseUid = HttpContext.Items["FirebaseUserId"] as string;

        if (string.IsNullOrEmpty(firebaseUid))
        {
            return Unauthorized("Missing Firebase UID");
        }
        
        var user = await _userService.GetByFirebaseUidAsync(firebaseUid);

        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(new
        {
            user.Id,
            user.Username,
            user.Email,
            user.FirstName,
            user.LastName,
            user.CreatedAt
        });
    }
}