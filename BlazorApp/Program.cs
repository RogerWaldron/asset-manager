using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp;
using Radzen;
using BlazorApp.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddLocalStorageServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddSupabaseServices();
// Radzen services
builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

// Init Supabase
// var supabase = app.Services.GetRequiredService<Client>();
// await supabase.InitializeAsync();
// supabase.Auth.LoadSession();

await app.RunAsync();