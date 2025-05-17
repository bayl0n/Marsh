using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Marsh.Api.Data;
using Marsh.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

const string firebaseProjectId = "marsh-70540";
var firebaseAuthority        = $"https://securetoken.google.com/{firebaseProjectId}";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
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

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async ctx =>
            {
                var jwtToken = ctx.SecurityToken as JwtSecurityToken;
                var rawJwt    = jwtToken?.RawData;
                if (rawJwt is null) return;

                var firebaseAuth = ctx.HttpContext.RequestServices
                                           .GetRequiredService<FirebaseAuthService>();
                var decoded = await firebaseAuth.VerifyIdTokenAsync(rawJwt);

                var userSvc = ctx.HttpContext.RequestServices
                                     .GetRequiredService<UserService>();
                var dbUser = await userSvc.SyncFirebaseUserAsync(
                    decoded.Uid,
                    decoded.Claims.TryGetValue("email", out var e)      ? e?.ToString()      : null,
                    decoded.Claims.TryGetValue("username", out var u) ? u?.ToString()       : null
                );

                if (ctx.Principal?.Identity is ClaimsIdentity id)
                {
                    id.AddClaim(new Claim("DbUserId", dbUser.Id.ToString()));
                }
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

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

builder.Services.AddSingleton<FirebaseAuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddDbContext<MarshDbContext>(opt =>
    opt.UseSqlite("Data Source=marsh.db")
);

builder.Services.AddCors(o => o.AddPolicy("AllowMarshAngular", p =>
    p.WithOrigins("http://localhost:4200")
     .AllowAnyHeader()
     .AllowAnyMethod()
     .AllowCredentials()
));

var app = builder.Build();

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

app.MapGet("/api/v1", () => "pong")
   .AllowAnonymous();

app.Run();
