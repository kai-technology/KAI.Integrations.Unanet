using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KAI.Integrations.Unanet.Models
{
    public sealed class UnanetEvent
    {
        [JsonPropertyName("event_name")]
        public string? EventName { get; set; }
        [JsonPropertyName("username")]
        public string? Username { get; set; }
        [JsonPropertyName("key_id")]
        public long KeyId { get; set; }
        [JsonPropertyName("data")]
        public object? Data { get; set; }
        [JsonPropertyName("database")]
        public string? Database { get; set; }
    }
}
