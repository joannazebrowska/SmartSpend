using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SmartSpend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("MyConn")));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
//app.UseStaticFiles();

app.UseAuthorization();

app.UseCors("ReactApp");

app.MapControllers();

app.Run();
