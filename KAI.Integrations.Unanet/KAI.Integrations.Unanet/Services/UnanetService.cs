using KAI.Integrations.Unanet.Configuration;
using KAI.Integrations.Unanet.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices;

namespace KAI.Integrations.Unanet.Services;

public class UnanetService : IUnanetService
{
    private readonly HttpClient _httpClient;

    private readonly IntegrationSettings _settings;

    public UnanetService(HttpClient httpClient, IOptions<IntegrationSettings> options)
    {
        _httpClient = httpClient;

        _settings = options.Value;
    }

    public async Task<UnanetAuthResponse> AuthenticateAsync(CancellationToken cancellationToken = default)
    {
        ValidateConfiguration();

        var request = new UnanetAuthRequest
        {
            ApiKey = _settings.UnanetApiKey,
            Database = _settings.UnanetDatabase
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/platform/authenticate",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var auth =
            await response.Content
                .ReadFromJsonAsync<UnanetAuthResponse>(
                    cancellationToken);
        if (auth is null || string.IsNullOrWhiteSpace(auth.Token))
        {
            throw new InvalidOperationException("Unanet authentication returned no token");

        }

        return auth;
    }

    public async Task<UnanetProject?> GetProjectAsync(long projectId, CancellationToken cancellationToken = default)
    {
        var auth =
            await AuthenticateAsync(cancellationToken);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/platform/projects/{projectId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
        using HttpResponseMessage response = await _httpClient.SendAsync(request,
                cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<UnanetProject>(cancellationToken);
    }

    private void ValidateConfiguration()
    {
        if (string.IsNullOrWhiteSpace(_settings.UnanetApiKey))
        {
            throw new InvalidOperationException("IntegrationSettings:UnanetApiKey is required.");
        }

        if (string.IsNullOrWhiteSpace(_settings.UnanetDatabase)) {
            throw new InvalidOperationException("IntegrationSettings:UnanetDatabase is required.");
        }
        
    }
}