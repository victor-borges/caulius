using System.Text.Json.Serialization;

namespace Caulius.Data.Entities
{
    public class TextArtCommand : Entity
    {
        [JsonPropertyName("command")]
        public string Command { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("text_art")]
        public string TextArt { get; set; }
    }
}
