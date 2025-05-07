using Marsh.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add db context
builder.Services.AddDbContext<MarshDbContext>(
        opt => opt.UseSqlite("Data Source=marsh.db")
    );

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMarshAngular",
        policy =>
        {
            policy.WithOrigins("http://10.0.0.66:4200")
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

app.MapControllers();

app.MapGet("api/v1", () => "pong");

app.Run();