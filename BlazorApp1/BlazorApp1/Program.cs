using BlazorApp1.Client.Pages;
using BlazorApp1.Components;
using BlazorApp1.Components.Account;
using BlazorApp1.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Razor Components + interactivitate WASM
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

// Auth helper services
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();

// API controllers
builder.Services.AddControllers();

// CORS – permisiv în dev
builder.Services.AddCors(o =>
{
     o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// HttpClient (server-side)
builder.Services.AddHttpClient();

// Identity (dacă folosești)
builder.Services.AddAuthentication(options =>
{
     options.DefaultScheme = IdentityConstants.ApplicationScheme;
     options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

builder.Services.AddAuthorization();

// DbContext + connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
     app.UseWebAssemblyDebugging();
     app.UseMigrationsEndPoint();
}
else
{
     app.UseExceptionHandler("/Error", createScopeForErrors: true);
     app.UseHsts();
}

app.UseHttpsRedirection();

// CORS
app.UseCors();

// Antiforgery (API-ul nostru îl ignoră explicit)
app.UseAntiforgery();

// API
app.MapControllers();

// UI Blazor
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp1.Client._Imports).Assembly);

// Endpoint-uri suplimentare Identity
app.MapAdditionalIdentityEndpoints();

app.Run();
