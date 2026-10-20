using System.Text.Json.Serialization;

namespace Entertainments.Models;

public class WatchProviderResponse
{
    [JsonPropertyName("results")]
    public Dictionary<string, WatchProviderRegion> Results { get; set; } = [];
}

public class WatchProviderRegion
{
    [JsonPropertyName("link")]
    public string? Link { get; set; }

    [JsonPropertyName("free")]
    public List<WatchProvider>? Free { get; set; }

    [JsonPropertyName("ads")]
    public List<WatchProvider>? Ads { get; set; }

    [JsonPropertyName("flatrate")]
    public List<WatchProvider>? Flatrate { get; set; }

    [JsonPropertyName("rent")]
    public List<WatchProvider>? Rent { get; set; }

    [JsonPropertyName("buy")]
    public List<WatchProvider>? Buy { get; set; }
}

public class WatchProvider
{
    [JsonPropertyName("provider_name")]
    public string ProviderName { get; set; } = "";

    [JsonPropertyName("logo_path")]
    public string? LogoPath { get; set; }
}