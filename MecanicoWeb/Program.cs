using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MecanicoWeb;
using MecanicoWeb.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ─── HTTP Client apuntando a la REST API ─────────────────────────────────────
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("http://localhost:5050") });

// ─── Servicios de dominio ─────────────────────────────────────────────────────
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<AutoService>();
builder.Services.AddScoped<ServicioService>();

await builder.Build().RunAsync();
