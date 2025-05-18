using Api.Interfaces;
using Api.Services;
using DataBase;
using DataBase.Repositories;
using Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VKR_backend.Extantions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("isAdmin", "True"));
    options.AddPolicy("NoRevoked", policy =>
        policy.RequireClaim("isRevoked", DateTime.MinValue.ToString()));
});

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AtonDBContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(AtonDBContext)));
    });

var configuration = builder.Configuration;
var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddAPIAuthentication(jwtOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
