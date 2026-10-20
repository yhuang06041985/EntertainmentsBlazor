
using System.Net.Http.Json;
using Entertainments.Models;

namespace Entertainments.Services
{
    public class YouTubeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public YouTubeService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;

            _apiKey = configuration["YouTube:ApiKey"]
                ?? throw new InvalidOperationException(
                    "YouTube API key is missing.");
        }

        public async Task<List<YouTubeVideo>> SearchVideoAsync(
            string songName)
        {
            if (string.IsNullOrWhiteSpace(songName))
                return new();

            string url =
                "https://www.googleapis.com/youtube/v3/search" +
                "?part=snippet" +
                "&type=video" +
                "&videoCategoryId=10" +
                "&videoEmbeddable=true" +
                "&order=viewCount" +
                "&maxResults=5" +
                $"&q={Uri.EscapeDataString(songName + " official audio")}" +
                $"&key={_apiKey}";

            var response = await _httpClient
                .GetFromJsonAsync<YouTubeSearchResponse>(url);

            return response?.Items ?? new();
        }
    }
}
