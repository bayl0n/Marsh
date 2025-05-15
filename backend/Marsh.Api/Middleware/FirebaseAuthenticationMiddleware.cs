using Marsh.Api.Services;

namespace Marsh.Api.Middleware;

public class FirebaseAuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, FirebaseAuthService firebaseAuthService)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader["Bearer ".Length..];

            try
            {
                var decodedToken = await firebaseAuthService.VerifyIdTokenAsync(token);

                var firebaseUid = decodedToken.Uid;
                var email = decodedToken.Claims.TryGetValue("email", out var emailClaim) ? emailClaim?.ToString() : null;
                var username = decodedToken.Claims.TryGetValue("username", out var usernameClaim) ? usernameClaim?.ToString() : null;
                
                var userService = context.RequestServices.GetRequiredService<UserService>();
                var dbUser = await userService.SyncFirebaseUserAsync(firebaseUid, email, username);
                
                context.Items["FirebaseUserId"] = dbUser.FirebaseUid;
                context.Items["DbUserId"] = dbUser.Id;
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid Firebase token.");
                return;
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authorization header missing.");
            return;
        }

        await next(context);
    }
}