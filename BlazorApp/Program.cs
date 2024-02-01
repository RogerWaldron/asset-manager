using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp;
using BlazorApp.Providers;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddLocalStorageServices();

builder.Services.AddScoped<AuthenticationStateProvider, SbAuthStateProvider>(
    prov => new SbAuthStateProvider(
        prov.GetRequiredService<ILocalStorageService>(),
        prov.GetRequiredService<ILogger<SbAuthStateProvider>>(),
        prov.GetRequiredService<Supabase.Client>()
        ));
builder.Services.AddAuthorizationCore();

var url = "https://oudgzfjtwfrawcfyzhcx.supabase.co";
var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im91ZGd6Zmp0d2ZyYXdjZnl6aGN4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDY1Njk3MzksImV4cCI6MjAyMjE0NTczOX0.YhMY8_UfUfG8ArhP7GzwMUeY4i6zj4GJn-R5g6RYtHk";
    
builder.Services.AddScoped<Supabase.Client>(
    provider => new Supabase.Client(
        url,
        key,
        new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true,
            SessionHandler = new SbSessionHandler(
                provider.GetRequiredService<ILocalStorageService>(),
                provider.GetRequiredService<ILogger<SbSessionHandler>>()
            )
        }
    )
);

builder.Services.AddScoped<SbAuthService>();
builder.Services.AddScoped<SbStorageService>();
builder.Services.AddScoped<NotificationService>();

await builder.Build().RunAsync();