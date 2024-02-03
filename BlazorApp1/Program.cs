using BlazorApp1;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiServer = builder.Configuration["ApiServer"] ?? "http://0.0.0.0";

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
    options.ProviderOptions.DefaultScopes.Add(apiServer);
});

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiServer) });

builder.Services.AddHttpClient("WebAPI",
        client => client.BaseAddress = new Uri(apiServer))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

await builder.Build().RunAsync();
