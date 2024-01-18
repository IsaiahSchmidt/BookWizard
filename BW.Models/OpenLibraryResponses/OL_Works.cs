using System.Text.Json.Serialization;

namespace BW.Models.OpenLibraryResponses
{
    public class OL_Works
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("subjects")]
        public List<string> Subjects { get; set; } = null;

    }
}