using API.Middleware;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services;
using Infrastructure.ExternalServices.MusicApi;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MusicTrackerDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MusicApiSettings>(
    builder.Configuration.GetSection("MusicApi"));

builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<ISyncService, SyncService>();
builder.Services.AddScoped<IListeningHistoryRepository, ListeningHistoryRepository>();
builder.Services.AddHttpClient<IMusicApiClient, MusicApiClient>(client =>
{
    client.BaseAddress = new Uri("https://ws.audioscrobbler.com/");
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();