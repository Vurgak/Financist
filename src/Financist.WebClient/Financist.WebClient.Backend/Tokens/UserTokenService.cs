using System.Security.Claims;
using Financist.WebClient.Backend.Configuration;
using Financist.WebClient.Backend.Contracts;
using Financist.WebClient.Backend.Session;
using Financist.WebClient.Backend.System;

namespace Financist.WebClient.Backend.Tokens;

public class UserTokenService : IUserTokenService
{
    private readonly IUserSessionStore _userSessionStore;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;
    private readonly string _refreshTokenUrl;
    private readonly string _revokeTokenUrl;

    public UserTokenService(
        IUserSessionStore userSessionStore,
        IHttpClientFactory httpClientFactory,
        IDateTimeOffsetProvider dateTimeOffsetProvider,
        IConfiguration configuration)
    {
        _userSessionStore = userSessionStore;
        _httpClientFactory = httpClientFactory;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;

        var authenticationConfiguration = new AuthenticationConfiguration();
        configuration.GetSection("Authentication")
            .Bind(authenticationConfiguration);
        _refreshTokenUrl = $"{authenticationConfiguration.ServerUrl}/{authenticationConfiguration.RefreshTokenEndpoint}";
        _revokeTokenUrl = $"{authenticationConfiguration.ServerUrl}/{authenticationConfiguration.RevokeTokenEndpoint}";
    }
    
    public async Task<string?> GetAccessTokenAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var sessionId = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        if (sessionId is null)
            return null;

        var session = await _userSessionStore.GetSessionDataAsync(sessionId);
        if (session is null)
            return null;

        if (_dateTimeOffsetProvider.Now < session.AccessTokenExpiration)
            return session.AccessToken;

        var tokens = await RefreshAccessTokenAsync(session.RefreshToken, cancellationToken);
        if (tokens is null)
            return null;

        var newSession = UserSession.FromTokensResponse(tokens, _dateTimeOffsetProvider.Now);
        await _userSessionStore.SetSessionDataAsync(sessionId, newSession);
        return tokens.AccessToken;
    }

    public async Task RevokeRefreshToken(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var sessionId = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        if (sessionId is null)
            return;

        var session = await _userSessionStore.GetSessionDataAsync(sessionId);
        if (session is null)
            return;
        
        var httpClient = _httpClientFactory.CreateClient();
        var request = new RevokeTokenRequest(session.RefreshToken);
        await httpClient.PostAsJsonAsync(_refreshTokenUrl, request, cancellationToken);
    }

    private async Task<TokensResponse?> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var request = new RefreshTokenRequest(refreshToken);
        var response = await httpClient.PostAsJsonAsync(_refreshTokenUrl, request, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;

        var tokens = await response.Content.ReadFromJsonAsync<TokensResponse>(cancellationToken); 
        return tokens;
    }
}
