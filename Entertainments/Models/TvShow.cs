using System.Text.Json.Serialization;

namespace Entertainments.Models;

public class TvShow
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("overview")]
    public string Overview { get; set; } = "";

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("first_air_date")]
    public string? FirstAirDate { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }
}