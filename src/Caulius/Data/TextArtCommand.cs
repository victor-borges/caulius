using System.Text.Json.Serialization;

namespace Caulius.Data
{
    public record TextArtCommand
    {
        [JsonPropertyName("command")]
        public string Command { get; init; }

        [JsonPropertyName("summary")]
        public string Summary { get; init; }

        [JsonPropertyName("text_art")]
        public string TextArt { get; init; }
    }
}
