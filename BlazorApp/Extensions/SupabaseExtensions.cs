using BlazorApp.Interfaces;
using BlazorApp.Services;
using Supabase;

namespace BlazorApp.Extensions;
public static class SupabaseExtensions {

  public static void AddSupabaseServices(this IServiceCollection services) {

  // Register Supabase

    try {
      var url = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? "";
      var key = Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? "";
    
      var options = new SupabaseOptions {
        AutoRefreshToken = true,
        AutoConnectRealtime = true,
        // SessionHandler = new SupabaseSessionProvider(),
      }; 
      services.AddSingleton(provider => new Supabase.Client(url, key, options));
    }
    catch (Exception)
    {
      throw new Exception("Failed to read Supabase environment variables");
    }

    // prov => new SbAuthStateProvider(
    //     prov.GetRequiredService<ILogger<SbAuthStateProvider>>(),
    //     prov.GetRequiredService<Supabase.Client>()
    //     ));  
    services.AddScoped<SbAuthService>();
    services.AddScoped<SbStorageService>();
    services.AddScoped<IAppStateService>(p => new AppStateService(p.GetRequiredService<Supabase.Client>()));
  // Register postgrest cache provider, comes with Supabase
  
  // services.AddScoped<IPostgrestCacheProvider, PostgrestCacheProvider>();

  }

} 