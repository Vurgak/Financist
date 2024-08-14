using System.Security.Claims;
using System.Security.Cryptography;
using Financist.WebClient.Backend.Contracts;
using Financist.WebClient.Backend.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Financist.WebClient.Backend.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("sign-up", SignUpAsync);
        builder.MapPost("sign-in", SignInAsync);
        builder.MapPost("sign-out", SignOutAsync);
    }

    private static async Task<IResult> SignUpAsync(
        [FromBody] SignUpRequest request,
        [FromServices] IHttpClientFactory httpClientFactory,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var apiClient = httpClientFactory.CreateClient();
        var response = await apiClient.PostAsJsonAsync("https://webapi/authenticate/register", request, cancellationToken);
        return response.IsSuccessStatusCode ? Results.Ok() : Results.BadRequest();
    }

    private static async Task<IResult> SignInAsync(
        [FromBody] SignInRequest request,
        [FromServices] IHttpClientFactory httpClientFactory,
        [FromServices] IUserSessionStore userSessionStore,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var apiClient = httpClientFactory.CreateClient();
        var response = await apiClient.PostAsJsonAsync("https://webapi/authenticate", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return Results.Unauthorized(); 

        var tokens = await response.Content.ReadFromJsonAsync<TokensResponse>(cancellationToken);
        var sessionData = new UserSession
        {
            AccessToken = tokens!.AccessToken,
            RefreshToken = tokens.RefreshToken,
        };
        
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

    private static async Task SignOutAsync(HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
