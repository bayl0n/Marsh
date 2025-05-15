using Marsh.Api.Data;
using Marsh.Api.Middleware;
using Marsh.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Marsh API", Version = "v1" });

    // Add JWT bearer security definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Paste your Firebase token here: Bearer <token>",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add security requirement to all endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

// Firebase Auth
builder.Services.AddSingleton<FirebaseAuthService>();
builder.Services.AddScoped<UserService>();

// Add db context
builder.Services.AddDbContext<MarshDbContext>(
        opt => opt.UseSqlite("Data Source=marsh.db")
    );

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMarshAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowMarshAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/v1"),
    appBuilder => appBuilder.UseMiddleware<FirebaseAuthenticationMiddleware>());


app.MapControllers();

app.MapGet("api/v1", () => "pong");

app.Run();