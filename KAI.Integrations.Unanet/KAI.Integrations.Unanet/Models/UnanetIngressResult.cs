using System;
using System.Collections.Generic;
using System.Text;

namespace KAI.Integrations.Unanet.Models
{
    public sealed class UnanetIngressResult
    {
        public bool Accepted { get; init; }

        public bool Duplicate { get; init; }
     
        public UnanetEvent? Event { get; init; }
        public UnanetProject? Project { get; init; }

        public string? RecordClassification { get; init; }

        public string? Message { get; init; }
    }
}
