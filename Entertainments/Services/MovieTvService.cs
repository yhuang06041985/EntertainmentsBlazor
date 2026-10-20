using Entertainments.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Entertainments.Services;

public class MovieTvService
{
    private readonly HttpClient _httpClient;

    public MovieTvService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;

        var baseUrl = configuration["TMDB:BaseUrl"]
            ?? "https://api.themoviedb.org/3/";

        var token = configuration["TMDB:ReadAccessToken"]
            ?? throw new InvalidOperationException(
                "TMDB ReadAccessToken is missing.");

        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<MovieSearchResponse> SearchMoviesAsync(string query)
    {
        var url = $"search/movie?query={Uri.EscapeDataString(query)}";

        var json = await _httpClient.GetStringAsync(url);

        return JsonSerializer.Deserialize<MovieSearchResponse>(json)
            ?? new MovieSearchResponse();
    }

    public async Task<TvSearchResponse> SearchTvShowsAsync(string query)
    {
        var url = $"search/tv?query={Uri.EscapeDataString(query)}";

        var json = await _httpClient.GetStringAsync(url);

        return JsonSerializer.Deserialize<TvSearchResponse>(json)
            ?? new TvSearchResponse();
    }

    public async Task<MediaDetails> GetMediaDetailsAsync(
    string mediaType,
    int id)
    {
        if (mediaType != "movie" && mediaType != "tv")
        {
            throw new ArgumentException(
                "Media type must be 'movie' or 'tv'.",
                nameof(mediaType));
        }

        var url = $"{mediaType}/{id}";

        var json = await _httpClient.GetStringAsync(url);

        return JsonSerializer.Deserialize<MediaDetails>(json)
            ?? throw new InvalidOperationException(
                "TMDB returned an empty response.");
    }

    public async Task<WatchProviderResponse> GetWatchProvidersAsync(
    string mediaType,
    int id)
    {
        if (mediaType != "movie" && mediaType != "tv")
        {
            throw new ArgumentException(
                "Media type must be 'movie' or 'tv'.",
                nameof(mediaType));
        }

        var url = $"{mediaType}/{id}/watch/providers";

        var json = await _httpClient.GetStringAsync(url);

        return JsonSerializer.Deserialize<WatchProviderResponse>(json)
            ?? new WatchProviderResponse();
    }
}