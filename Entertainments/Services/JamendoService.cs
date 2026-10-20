
using Entertainments.Models;
using Entertainments.Repository;
using System.Net.Http.Json;

namespace Entertainments.Services
{
    public class JamendoService
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly SongRepostitory _repository;

        public JamendoService(
            HttpClient httpClient,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            SongRepostitory repository)
        {
            _httpClient = httpClient;
            _environment = environment;
            _configuration = configuration;
            _repository = repository;
        }

        public async Task<List<JamendoSong>> SearchSongsAsync(string songName)
        {
            if (string.IsNullOrWhiteSpace(songName))
                return new List<JamendoSong>();

            string clientId =
                _configuration["Jamendo:ClientId"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(clientId))
                throw new InvalidOperationException(
                    "Jamendo:ClientId is missing from appsettings.json.");

            async Task<List<JamendoSong>> SearchApi(string parameter)
            {
                string url =
                    "https://api.jamendo.com/v3.0/tracks/" +
                    $"?client_id={Uri.EscapeDataString(clientId)}" +
                    "&format=json" +
                    "&limit=50" +
                    "&audioformat=mp32" +
                    "&audiodlformat=mp32" +
                    $"&{parameter}={Uri.EscapeDataString(songName.Trim())}";

                var response =
                    await _httpClient.GetFromJsonAsync<JamendoResponse>(url);

                return response?.Results ?? new List<JamendoSong>();
            }

            var results = await SearchApi("namesearch");

            if (results.Count == 0)
                results = await SearchApi("search");

            return results;
        }





        public async Task<string> DownloadSongAsync(JamendoSong song)
        {
            if (song == null)
                throw new ArgumentNullException(nameof(song));

            if (!song.DownloadAllowed ||
                string.IsNullOrWhiteSpace(song.AudioDownload))
            {
                throw new InvalidOperationException(
                    "This song is not available for download.");
            }

            if (!Uri.TryCreate(
                    song.AudioDownload,
                    UriKind.Absolute,
                    out var uri) ||
                uri.Scheme != Uri.UriSchemeHttps ||
                uri.Host != "prod-1.storage.jamendo.com")
            {
                throw new InvalidOperationException(
                    "Invalid Jamendo download URL.");
            }

            string folder = Path.Combine(
                _environment.WebRootPath,
                "songs");

            Directory.CreateDirectory(folder);

            string fileName = $"{Guid.NewGuid():N}.mp3";
            string filePath = Path.Combine(folder, fileName);

            using var response = await _httpClient.GetAsync(
                uri,
                HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            await using (var source =
                await response.Content.ReadAsStreamAsync())
            await using (var destination = File.Create(filePath))
            {
                await source.CopyToAsync(destination);
            }

            try
            {
                var songAdd = new Song
                {
                    Name = song.Name,
                    Artist = song.ArtistName,
                    FilePath = $"/songs/{fileName}"
                };

                await _repository.AddSongAsync(songAdd);
            }
            catch
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                throw;
            }

            return $"/songs/{fileName}";
        }

        public async Task DeleteSongAsync(Song song)
        {
            if (song == null)
                throw new ArgumentNullException(nameof(song));

            string relativePath = song.FilePath
                .Replace('\\', '/')
                .TrimStart('/');

            if (!relativePath.StartsWith(
                    "songs/",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Invalid song file path.");
            }

            string fileName = relativePath["songs/".Length..];

            if (string.IsNullOrWhiteSpace(fileName) ||
                fileName != Path.GetFileName(fileName))
            {
                throw new InvalidOperationException(
                    "Invalid song file name.");
            }

            string filePath = Path.Combine(
                _environment.WebRootPath,
                "songs",
                fileName);

            await _repository.DeleteSongAsync(song.Id);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
