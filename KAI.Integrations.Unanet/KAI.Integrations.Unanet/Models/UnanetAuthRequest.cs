using System.Text.Json.Serialization;

namespace KAI.Integrations.Unanet.Models;

public sealed class UnanetAuthRequest
{
    [JsonPropertyName("apiKey")]
    public string? ApiKey { get; set; }

    [JsonPropertyName("database")]
    public string? Database { get; set; }
}