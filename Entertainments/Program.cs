using Entertainments.Components;
using Entertainments.Datas;
using Entertainments.Repository;
using Entertainments.Services;
using Entertainments.Utilities;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<YouTubeService>();
builder.Services.AddHttpClient<SongDownloadService>();
builder.Services.AddHttpClient<JamendoService>();
builder.Services.AddHttpClient<MovieTvService>();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<SongRepostitory>();
builder.Services.AddScoped<JamendoService>();
builder.Services.AddTransient<Utility>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

// Download all MP3 files as one ZIP.
app.MapGet("/download-songs", (IWebHostEnvironment env) =>
{
    var folder = Path.Combine(env.WebRootPath, "songs");

    if (!Directory.Exists(folder))
        return Results.NotFound("Songs folder not found.");

    using var memory = new MemoryStream();

    using (var zip = new ZipArchive(
        memory, ZipArchiveMode.Create, true))
    {
        foreach (var file in Directory.GetFiles(folder, "*.mp3"))
        {
            zip.CreateEntryFromFile(
                file,
                Path.GetFileName(file),
                CompressionLevel.Fastest);
        }
    }

    return Results.File(
        memory.ToArray(),
        "application/zip",
        "MyMusic.zip");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();