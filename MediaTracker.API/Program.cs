using System;
using System.IO;
using MediaTracker.API.Repositories;
using MediaTracker.API.Repositories.Interfaces;
using MediaTracker.API.Services;
using MediaTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add repositories
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IMediaRepository, MediaRepository>();
builder.Services.AddSingleton<ICommentRepository, CommentRepository>();
builder.Services.AddSingleton<IWatchlistRepository, WatchlistRepository>();

// Add services
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IMediaService, MediaService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IReportService, ReportService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Ensure data directory exists
var dataDir = Path.Combine(app.Environment.ContentRootPath, "Data");
if (!Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
    MediaTracker.API.Utils.DataGenerator.GenerateSampleData(dataDir);
}

app.Run();