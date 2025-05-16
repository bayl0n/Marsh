using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Marsh.Api.Data;
using Marsh.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1) Firebase project settings
const string firebaseProjectId = "marsh-70540";
var firebaseAuthority        = $"https://securetoken.google.com/{firebaseProjectId}";

// 2) Authentication + JWT-Bearer config
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // tell ASP.NET Core how to validate the token
        options.Authority = firebaseAuthority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer   = true,
            ValidIssuer      = firebaseAuthority,
            ValidateAudience = true,
            ValidAudience    = firebaseProjectId,
            ValidateLifetime = true,
            ClockSkew        = TimeSpan.FromMinutes(2),
        };

        // once Microsoft’s checks pass, do your own Firebase→DB sync
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async ctx =>
            {
                // pull the raw JWT so you can re-decode extra claims
                var jwtToken = ctx.SecurityToken as JwtSecurityToken;
                var rawJwt    = jwtToken?.RawData;
                if (rawJwt is null) return;

                // verify & decode via your service
                var firebaseAuth = ctx.HttpContext.RequestServices
                                           .GetRequiredService<FirebaseAuthService>();
                var decoded = await firebaseAuth.VerifyIdTokenAsync(rawJwt);

                // sync into your Users table
                var userSvc = ctx.HttpContext.RequestServices
                                     .GetRequiredService<UserService>();
                var dbUser = await userSvc.SyncFirebaseUserAsync(
                    decoded.Uid,
                    decoded.Claims.TryGetValue("email", out var e)      ? e?.ToString()      : null,
                    decoded.Claims.TryGetValue("username", out var u) ? u?.ToString()       : null
                );

                // add your own DbUserId claim
                if (ctx.Principal?.Identity is ClaimsIdentity id)
                {
                    id.AddClaim(new Claim("DbUserId", dbUser.Id.ToString()));
                }
            }
        };
    });

// 3) Standard Authorize support
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// 4) Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Marsh API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In          = ParameterLocation.Header,
        Name        = "Authorization",
        Type        = SecuritySchemeType.ApiKey,
        Scheme      = "Bearer",
        Description = "Enter: Bearer <your Firebase ID token>"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme, 
                    Id   = "Bearer" 
                }
            },
            []
        }
    });
});

// 5) Your services & EF Core
builder.Services.AddSingleton<FirebaseAuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddDbContext<MarshDbContext>(opt =>
    opt.UseSqlite("Data Source=marsh.db")
);

// 6) CORS
builder.Services.AddCors(o => o.AddPolicy("AllowMarshAngular", p =>
    p.WithOrigins("http://localhost:4200")
     .AllowAnyHeader()
     .AllowAnyMethod()
     .AllowCredentials()
));

var app = builder.Build();

// 7) Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowMarshAngular");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// a public ping:
app.MapGet("/api/v1", () => "pong")
   .AllowAnonymous();

app.Run();
