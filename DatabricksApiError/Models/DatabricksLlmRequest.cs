using System.Text.Json.Serialization;

namespace DatabricksApiError.Models;
public class DatabricksLlmRequest
{
    [JsonPropertyName("messages")]
    public List<DatabricksLlmRequestMessage> Messages { get; set; } = new List<DatabricksLlmRequestMessage>();
    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }
}