using System.Net;
using KAI.Integrations.Unanet.Models;
using KAI.Integrations.Unanet.Services;
using Microsoft.Azure.Functions.Worker.Http;

namespace KAI.Integrations.Unanet.Functions;

internal static class FunctionResponseFactory
{
    public static async Task<HttpResponseData> CreateAsync(
        HttpRequestData request,
        IUnanetEventIngressService ingress,
        UnanetRouteEventType routeEventType,
        CancellationToken cancellationToken)
    {
        UnanetIngressResult result =
            await ingress.ReceiveAsync(
                request,
                routeEventType,
                cancellationToken);

        HttpStatusCode statusCode =
            result.Accepted
                ? HttpStatusCode.OK
                : HttpStatusCode.BadRequest;

        HttpResponseData response =
            request.CreateResponse(statusCode);

        await response.WriteAsJsonAsync(
            new
            {
                accepted = result.Accepted,
                duplicate = result.Duplicate,
                routeEvent =
                    routeEventType.ToString(),
                reportedEvent =
                    result.Event?.EventName,
                projectId =
                    result.Event?.KeyId,
                classification =
                    result.RecordClassification,
                message =
                    result.Message
            },
            cancellationToken);

        return response;
    }
}