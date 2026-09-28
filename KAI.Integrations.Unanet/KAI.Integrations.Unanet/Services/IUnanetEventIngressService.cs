using System;
using System.Collections.Generic;
using System.Text;
using KAI.Integrations.Unanet.Models;
using Microsoft.Azure.Functions.Worker.Http;

namespace KAI.Integrations.Unanet.Services
{
    public interface IUnanetEventIngressService
    {
        Task<UnanetIngressResult> ReceiveAsync(
            HttpRequestData request,
        UnanetRouteEventType routeEventType,
        CancellationToken cancellationToken = default);
    }
}
