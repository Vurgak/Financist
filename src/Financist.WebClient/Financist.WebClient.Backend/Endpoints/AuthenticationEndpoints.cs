using System.Security.Claims;
using System.Security.Cryptography;
using Financist.WebClient.Backend.Contracts;
using Financist.WebClient.Backend.Session;
using Financist.WebClient.Backend.System;
using Financist.WebClient.Backend.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Financist.WebClient.Backend.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("sign-up", SignUpAsync);
        builder.MapPost("sign-in", SignInAsync);
        builder.MapPost("sign-out", SignOutAsync);
        builder.MapGet("session", (Delegate)GetSessionInformationAsync);
    }

    private static async Task<IResult> SignUpAsync(
        [FromBody] SignUpRequest request,
        [FromServices] IHttpClientFactory httpClientFactory,
        [FromServices] IConfiguration configuration,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var apiClient = httpClientFactory.CreateClient();
        var requestUrl = $"{configuration["Authentication:ServerUrl"]}/{configuration["Authentication:RegisterEndpoint"]}";
        var response = await apiClient.PostAsJsonAsync(requestUrl, request, cancellationToken);
        return response.IsSuccessStatusCode ? Results.Ok() : Results.BadRequest();
    }

    private static async Task<IResult> SignInAsync(
        [FromBody] SignInRequest request,
        [FromServices] IHttpClientFactory httpClientFactory,
        [FromServices] IUserSessionStore userSessionStore,
        [FromServices] IConfiguration configuration,
        [FromServices] IDateTimeOffsetProvider dateTimeOffsetProvider,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var apiClient = httpClientFactory.CreateClient();
        var requestUrl = $"{configuration["Authentication:ServerUrl"]}/{configuration["Authentication:AuthenticateEndpoint"]}";
        var response = await apiClient.PostAsJsonAsync(requestUrl, request, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return Results.Unauthorized(); 

        var tokens = await response.Content.ReadFromJsonAsync<TokensResponse>(cancellationToken);
        var sessionData = UserSession.FromTokensResponse(tokens!, dateTimeOffsetProvider.Now);
        var sessionId = RandomNumberGenerator.GetHexString(16);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, sessionId),
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        await userSessionStore.SetSessionDataAsync(sessionId, sessionData);
        return Results.Ok();
    }

    private static async Task SignOutAsync(
        [FromServices] IUserTokenService userTokenService,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await userTokenService.RevokeRefreshToken(context.User, cancellationToken);
    }

    private static async Task<IResult> GetSessionInformationAsync(
        [FromServices] IUserSessionStore userSessionStore,
        HttpContext context)
    {
        var authenticationResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!authenticationResult.Succeeded)
            return Results.Unauthorized();

        var sessionId = context.User.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;
        var userSession = await userSessionStore.GetSessionDataAsync(sessionId);

        var subjectId = GetSubjectIdFromAccessToken(userSession!.AccessToken);
        var sessionInformation = new SessionInformation
        {
            SubjectId = subjectId,
            SessionId = sessionId,
            SessionExpiresIn = (int)authenticationResult.Properties.ExpiresUtc!.Value.Subtract(DateTimeOffset.Now).TotalSeconds,
        };
        return Results.Ok(sessionInformation);
    }

    private static string GetSubjectIdFromAccessToken(string accessToken)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var decodedToken = (JsonWebToken)tokenHandler.ReadToken(accessToken);
        return decodedToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
    }
}
