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
        try
        {
            var serialized = JsonConvert.SerializeObject(session);
            _localStorage.SetItem(SessionKey, session);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception - Session Save");
        }
    }

    public void DestroySession()
    {
        _logger.LogInformation("Session Destroy");
        _localStorage.RemoveItem(SessionKey);
    }

    public Session? LoadSession()
    {
        try
        {
            var session = _localStorage.GetItem<Session>(SessionKey);

            if (string.IsNullOrEmpty(session))
                return null;
            
            return session.ExpiresAt() <= DateTime.Now ? null : session;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception - Session Load");
            return null;
        }

    }
}