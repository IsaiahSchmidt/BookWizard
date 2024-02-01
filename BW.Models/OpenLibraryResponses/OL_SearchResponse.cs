
using System.Text.Json.Serialization;

namespace BW.Models.OpenLibraryResponses
{
    public class OL_SearchResponse
    {
        [JsonPropertyName("numFound")]
        public int Found {get; set;}

        [JsonPropertyName("docs")]
        public List<OL_Docs> Docs {get; set;} = null;
    }
}