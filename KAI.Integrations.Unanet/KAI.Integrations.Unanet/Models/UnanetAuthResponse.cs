using System.Text.Json.Serialization;

namespace KAI.Integrations.Unanet.Models;

public sealed class UnanetAuthResponse
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("expireDate")]
    public DateTimeOffset? ExpireDate { get; set; }

    [JsonPropertyName("passwordResetRequired")]
    public bool PasswordResetRequired { get; set; }
}