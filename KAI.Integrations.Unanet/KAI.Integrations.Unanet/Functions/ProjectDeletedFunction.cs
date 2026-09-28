using KAI.Integrations.Unanet.Services;
using System;
using System.Collections.Generic;
using System.Text;
using KAI.Integrations.Unanet.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace KAI.Integrations.Unanet.Functions
{
    internal class ProjectDeletedFunction
    {
        private readonly IUnanetEventIngressService _ingress;

        public ProjectDeletedFunction(IUnanetEventIngressService ingress)
        {
            ingress = ingress;
        }

        [Function("ProjectDeleted")]
        public Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "unanet/projects/deleted")] HttpRequestData request,
            CancellationToken cancellationToken)
        {
            return FunctionResponseFactory.CreateAsync(
                request,
                _ingress,
                UnanetRouteEventType.Deleted,
                cancellationToken);
        }
    }
}
