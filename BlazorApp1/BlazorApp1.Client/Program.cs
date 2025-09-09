using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// HttpClient folosește baza aplicației (ex: https://localhost:7138/)
builder.Services.AddScoped(sp => new HttpClient
{
     BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

await builder.Build().RunAsync();
