using Application.Interfaces;
using Application.Services;
using Infrastructure.ExternalServices.MusicApi;
using Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MusicApiSettings>(
    builder.Configuration.GetSection("MusicApi"));

builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddHttpClient<IMusicApiClient, MusicApiClient>(client =>
{
    client.BaseAddress = new Uri("https://ws.audioscrobbler.com/");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();