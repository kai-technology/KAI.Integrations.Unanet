using System;
using System.Collections.Generic;
using System.Text;
using KAI.Integrations.Unanet.Models;

namespace KAI.Integrations.Unanet.Services;

public interface IUnanetService
{
    Task<Models.UnanetAuthResponse> AuthenticateAsync(
        CancellationToken cancellationToken = default);

    Task<UnanetProject?> GetProjectAsync(
        long projectId,
        CancellationToken cancellationToken = default);
}