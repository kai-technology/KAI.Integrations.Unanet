using KAI.Integrations.Unanet.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;

namespace KAI.Integrations.Unanet.Services
{
    public sealed class UnanetEventIngressService : IUnanetEventIngressService
    {
        private static readonly JsonSerializerOptions JsonOptions = new () { PropertyNameCaseInsensitive = true };
        private readonly IUnanetService _unanetService;
        private readonly IUnanetEventDeduplicator _deduplicator;
        private readonly ILogger < UnanetEventIngressService > _logger;

        public UnanetEventIngressService(IUnanetService unanetService,
            IUnanetEventDeduplicator deduplicator,
            ILogger<UnanetEventIngressService> logger)
        {
            _unanetService = unanetService;
            _deduplicator = deduplicator;
            _logger = logger;
        }

        public async Task<UnanetIngressResult> ReceiveAsync(
            HttpRequestData request,
            UnanetRouteEventType routeEventType,
            CancellationToken cancellationToken = default)
        {
            string body;

            using (var reader = new StreamReader(request.Body))
            {
                body = await reader.ReadToEndAsync(
                    cancellationToken);
            }

            UnanetEvent? unanetEvent;

            try
            {
                unanetEvent = JsonSerializer.Deserialize<UnanetEvent>(body, JsonOptions);
            }
            catch (JsonException exception)
            {
                _logger.LogWarning(exception, "Invalid Unanet payload received on route {RouteEventType}.", routeEventType);
            }

            return new UnanetIngressResult
            {
                Accepted = false,
                Message = "Invalid JSON payload."
            };

            if (unanetEvent is null || unanetEvent.KeyId <= 0 || string.IsNullOrWhiteSpace(unanetEvent.Database))
            {
                return new UnanetIngressResult
                {
                    Accepted = false,
                    Event = unanetEvent,
                    Message = "Payload requires key_id and database."
                };
            }

            _logger.LogInformation(
                "Unanet route event {RouteEventType} received. " +
                "ReportedEvent={ReportedEvent}; " +
                "ProjectId={ProjectId}; " + "Database={Database};"  +
            "Username={Username}",
            routeEventType,
            unanetEvent.EventName,
            unanetEvent.KeyId,
            unanetEvent.Database,
            unanetEvent.Username);

            if (!_deduplicator.TryAccept(
                    routeEventType,
                    unanetEvent))
            {
                _logger.LogInformation(
                    "Duplicate Unanet event suppressed. " +
                    "RouteEvent={RouteEventType}; " +
                    "ProjectId={ProjectId}",
                    routeEventType,
                    unanetEvent.KeyId);

                return new UnanetIngressResult
                {
                    Accepted = true,
                    Duplicate = true,
                    Event = unanetEvent,
                    Message = "Duplicate event suppressed."
                };
            }

            //A deleted project may no longer be retrievable.
            //Do not make successful deletion-event receipt depend on GET succeeding.

            if (routeEventType ==
                UnanetRouteEventType.Deleted)
            {
                return new UnanetIngressResult
                {
                    Accepted = true,
                    Duplicate = false,
                    Event = unanetEvent,
                    RecordClassification = "Deleted",
                    Message = "Project deletion event accepted."
                };
            }

            UnanetProject? project =
                await _unanetService.GetProjectAsync(
                    unanetEvent.KeyId,
                    cancellationToken);
 
            if (project is null)
            {
                _logger.LogWarning(
                    "Unanet project {ProjectId} was not found after " +
                    "receiving route event {RouteEveetType}.",
                    unanetEvent.KeyId,
                    routeEventType);
 
                return new UnanetIngressResult
                {
                    Accepted = true,
                    Event = unanetEvent,
                    Message =
                        "Event accepted, but project lookup returned no record."
                };
            }
 
            string classification = project.IsOpportunity ? "Opportunity": "Project";
 
            _logger.LogInformation(
                "Unanet record classified. " +
                "ProjectId={ProjectId}; " +
                "Code={Code}; " +
                "ChargeType={ChargeType}; " +
                "Classification={Classification}; " + "Active={Active}",
                project.Id,
                project.Code,
                project.ChangeTypeName,
                classification,
                project.Active);
            return new UnanetIngressResult
            {
                Accepted = true,
                Duplicate = false,
                Event = unanetEvent,
                Project = project,
                RecordClassification = classification,
                Message = $"{classification} event accepted"
            };
        }
    }
}
