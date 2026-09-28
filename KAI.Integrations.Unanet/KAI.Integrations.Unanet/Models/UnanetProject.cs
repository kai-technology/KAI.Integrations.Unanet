using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using System.Text.Json;

namespace KAI.Integrations.Unanet.Models
{
    public class UnanetProject
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("longName")]
        public string? LongName { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /*
         * JsonElement is intentional here.
         *
         * Keep this flexible until you confirm whether your tenant returns:
         *
         * "chargeType": "Opportunity"
         *
         * or:
         *
         * "chargeType": {
         *     "id": 123,
         *     "name": "Opportunity"
         * }
         */
        [JsonPropertyName("chargeType")]
        public JsonElement ChargeType { get; set; }

        [JsonPropertyName("createBy")]
        public string? CreatedBy { get; set; }

        [JsonPropertyName("createDate")]
        public DateTimeOffset? CreatedDate { get; set; }

        [JsonPropertyName("modifyBy")]
        public string? ModifiedBy { get; set; }

        [JsonPropertyName("modifyDate")]
        public DateTimeOffset? ModifiedDate { get; set; }

        [JsonIgnore]
        public string? ChangeTypeName => ChargeType.ValueKind switch
        {
            JsonValueKind.String =>
                ChargeType.GetString(),

            JsonValueKind.Object
                when ChargeType.TryGetProperty(
                    "name",
                    out JsonElement name) =>
                name.GetString(),

            JsonValueKind.Null =>
                null,

            JsonValueKind.Undefined =>
                null,

            _ =>
                ChargeType.ToString()
        };

        [JsonIgnore]
        public bool IsOpportunity => string.Equals(ChangeTypeName, "Opportunity", StringComparison.OrdinalIgnoreCase);
        
    }
}