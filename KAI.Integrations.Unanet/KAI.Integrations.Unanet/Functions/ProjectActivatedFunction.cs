using KAI.Integrations.Unanet.Models;
using KAI.Integrations.Unanet.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace KAI.Integrations.Unanet.Functions
{
    public class ProjectActivatedFunction
    {
        private readonly IUnanetEventIngressService _ingress;
        private readonly ILogger<ProjectCreatedFunction> _logger;

        public ProjectActivatedFunction(IUnanetEventIngressService ingress)
        {
            ingress = ingress;
        }

        [Function("ProjectActivated")]
        public Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "unanet/projects/activated")] HttpRequestData request,
            CancellationToken cancellationToken)
        {
            return FunctionResponseFactory.CreateAsync(
                request,
                _ingress,
                UnanetRouteEventType.Activated,
                cancellationToken);
        }
    }
}
