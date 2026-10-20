namespace Entertainments.Services;

public class SongDownloadService
{
    private readonly HttpClient _httpClient;
    private readonly IWebHostEnvironment _environment;

    public SongDownloadService(
        HttpClient httpClient,
        IWebHostEnvironment environment)
    {
        _httpClient = httpClient;
        _environment = environment;
    }

    public async Task<string> DownloadSongAsync(
        string audioUrl, string songName)
    {
        string folder = Path.Combine(
            _environment.WebRootPath, "songs");
        

        Directory.CreateDirectory(folder);

        string fileName = $"{Guid.NewGuid():N}.mp3";
        string filePath = Path.Combine(folder, fileName);

        using var response = await _httpClient.GetAsync(
            audioUrl, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        var contentType = response.Content.Headers.ContentType?.MediaType;

        if (contentType != "audio/mpeg")
            throw new InvalidOperationException(
                "The URL does not return an MP3 audio file.");

        await using var source =
            await response.Content.ReadAsStreamAsync();

        await using var destination = File.Create(filePath);

        await source.CopyToAsync(destination);

        

        return $"/songs/{fileName}";
    }
}