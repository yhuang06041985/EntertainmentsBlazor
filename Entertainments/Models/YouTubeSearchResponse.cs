
using System.Text.Json.Serialization;

namespace Entertainments.Models
{
    public class YouTubeSearchResponse
    {
        public string Kind { get; set; } = "";
        public string Etag { get; set; } = "";
        public string NextPageToken { get; set; } = "";
        public string RegionCode { get; set; } = "";

        public YouTubePageInfo PageInfo { get; set; } = new();

        public List<YouTubeVideo> Items { get; set; } = new();
    }

    public class YouTubePageInfo
    {
        public int TotalResults { get; set; }
        public int ResultsPerPage { get; set; }
    }

    public class YouTubeVideo
    {
        public string Kind { get; set; } = "";
        public string Etag { get; set; } = "";

        public YouTubeVideoId Id { get; set; } = new();
        public YouTubeSnippet Snippet { get; set; } = new();

        [JsonIgnore]
        public string VideoUrl =>
            $"https://www.youtube.com/watch?v={Id.VideoId}";

        [JsonIgnore]
        public string EmbedUrl =>
            $"https://www.youtube.com/embed/{Id.VideoId}";
    }

    public class YouTubeVideoId
    {
        public string Kind { get; set; } = "";
        public string VideoId { get; set; } = "";
    }

    public class YouTubeSnippet
    {
        public DateTime PublishedAt { get; set; }

        public string ChannelId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

        public YouTubeThumbnails Thumbnails { get; set; } = new();

        public string ChannelTitle { get; set; } = "";
        public string LiveBroadcastContent { get; set; } = "";

        public DateTime PublishTime { get; set; }
    }

    public class YouTubeThumbnails
    {
        [JsonPropertyName("default")]
        public YouTubeThumbnail? Default { get; set; }

        public YouTubeThumbnail? Medium { get; set; }
        public YouTubeThumbnail? High { get; set; }
        public YouTubeThumbnail? Standard { get; set; }
        public YouTubeThumbnail? Maxres { get; set; }
    }

    public class YouTubeThumbnail
    {
        public string Url { get; set; } = "";
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
