using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Supabase;

namespace BlazorApp.Providers;

public class SbAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<SbAuthStateProvider> _logger;
    private readonly Client _client;

    public SbAuthStateProvider(ILocalStorageService localStorage, ILogger<SbAuthStateProvider> logger, Supabase.Client client)
    {
        _localStorage = localStorage;
        _logger = logger;
        _client = client;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogInformation("GetAuthenticationState");

        await _client.InitializeAsync();

        var identity = new ClaimsIdentity();

        if (!string.IsNullOrEmpty(_client?.Auth?.CurrentSession?.AccessToken))
        {
            identity = new ClaimsIdentity(ParseClaimsFromJWT(_client.Auth.CurrentSession.AccessToken), "jwt");
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        
        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private static IEnumerable<Claim>? ParseClaimsFromJWT(string jwt)
    {
        var payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        var kvPairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return kvPairs.Select(kv => new Claim(kv.Key, kv.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: 
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }
}