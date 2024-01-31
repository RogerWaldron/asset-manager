using Microsoft.JSInterop;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;

namespace BlazorApp.Providers;

public class SbSessionHandler : IGotrueSessionPersistence<Session>
{
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<SbSessionHandler> _logger;
    private const string SessionKey = "SUPABASE_SESSION";


    public SbSessionHandler(ILocalStorageService localStorage, ILogger<SbSessionHandler> logger)
    {
        _localStorage = localStorage;
        _logger = logger;
    }

    public void SaveSession(Session session)
    {
        _logger.LogInformation("Session Save");
        _localStorage.SetItem(SessionKey, session);
    }

    public void DestroySession()
    {
        _logger.LogInformation("Session Destroy");
        _localStorage.RemoveItem(SessionKey);
    }

    public Session? LoadSession()
    {
        _logger.LogInformation("Session Load");
        var session = _localStorage.GetItem<Session>(SessionKey);

        return session?.ExpiresAt() <= DateTime.Now ? null : session;
    }
}