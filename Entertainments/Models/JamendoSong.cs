using System.Text.Json.Serialization;

namespace Entertainments.Models
{
    public class JamendoSong
    {
        public string Id { get; set; } = "";

        public string Name { get; set; } = "";

        [JsonPropertyName("artist_name")]
        public string ArtistName { get; set; } = "";

        public int Duration { get; set; }

        public string Audio { get; set; } = "";

        [JsonPropertyName("audiodownload")]
        public string AudioDownload { get; set; } = "";

        [JsonPropertyName("audiodownload_allowed")]
        public bool DownloadAllowed { get; set; }

        public string Image { get; set; } = "";
    }

    public class JamendoResponse
    {
        public List<JamendoSong> Results { get; set; } = new();
    }
}