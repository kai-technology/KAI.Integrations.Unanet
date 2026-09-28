using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace KAI.Integrations.Unanet.Functions;

public class ProjectCreatedFunction
{
    private readonly ILogger<ProjectCreatedFunction> _logger;

    public ProjectCreatedFunction(ILogger<ProjectCreatedFunction> logger)
    {
        _logger = logger;
    }

    [Function("ProjectCreated")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "projectcreated")] HttpRequestData req)
    {
        string body =
            await new StreamReader(req.Body).ReadToEndAsync();

        _logger.LogInformation("Unanet Project Created Event Received");

        _logger.LogInformation(body);

        var response = req.CreateResponse(HttpStatusCode.OK);

        await response.WriteStringAsync("Project event received.");

        return response;
    }
}