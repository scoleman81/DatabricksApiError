using System.Text.Json.Serialization;

namespace DatabricksApiError.Models;
public class DatabricksLlmRequestMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
}