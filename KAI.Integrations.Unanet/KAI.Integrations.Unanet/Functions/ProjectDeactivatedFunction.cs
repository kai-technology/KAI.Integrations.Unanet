using KAI.Integrations.Unanet.Services;
using System;
using System.Collections.Generic;
using System.Text;
using KAI.Integrations.Unanet.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace KAI.Integrations.Unanet.Functions
{
    public class ProjectDeactivatedFunction
    {
        private readonly IUnanetEventIngressService _ingress;

        public ProjectDeactivatedFunction(IUnanetEventIngressService ingress)
        {
            ingress = ingress;
        }

        [Function("ProjectDeactivated")]
        public Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "unanet/projects/deactivated")] HttpRequestData request,
            CancellationToken cancellationToken)
        {
            return FunctionResponseFactory.CreateAsync(
                request,
                _ingress,
                UnanetRouteEventType.Deactivated,
                cancellationToken);
        }
    }
}
