using System.Text.Json.Serialization;

namespace BW.Models.OpenLibraryResponses
{
    public class OL_Docs
    {
        [JsonPropertyName("key")]
        public string Key {get; set;} 
    }
}