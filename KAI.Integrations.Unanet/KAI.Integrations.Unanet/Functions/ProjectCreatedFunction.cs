using KAI.Integrations.Unanet.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using KAI.Integrations.Unanet.Models;

namespace KAI.Integrations.Unanet.Functions;

public class ProjectCreatedFunction
{
    private readonly IUnanetEventIngressService _ingress;

    private readonly ILogger<ProjectCreatedFunction> _logger;

    public ProjectCreatedFunction(IUnanetEventIngressService ingress)
    {
        ingress = ingress;
    }

    [Function("ProjectCreated")]
    public Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "unanet/projects/created")] HttpRequestData request, 
        CancellationToken cancellationToken)
    {
        return FunctionResponseFactory.CreateAsync(
            request,
            _ingress,
            UnanetRouteEventType.Created,
            cancellationToken);
    }
}