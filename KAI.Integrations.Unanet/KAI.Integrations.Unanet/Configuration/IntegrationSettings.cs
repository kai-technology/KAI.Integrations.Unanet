using System;
using System.Collections.Generic;
using System.Text;

namespace KAI.Integrations.Unanet.Configuration
{
    public class IntegrationSettings
    {
        public string? CogramBaseUrl { get; set; }

        public string? CogramApiKey { get; set; }
        /// <summary>
        /// The base username for the account for FreshService. Their API does not use an API key.
        /// </summary>
        public string? FreshserviceApiKey { get; set; }

        public string? FreshserviceBaseUrl { get; set; }

        public string? UnanetBaseUrl { get; set; }

        public string? UnanetApiKey { get; set; }

        public string? UnanetDatabase { get; set; }
    }
}
