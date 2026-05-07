using Microsoft.EntityFrameworkCore;
using MecanicoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ─── SQLite con EF Core ─────────────────────────────────────────────────────
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "..", "mecanico.sqlite");
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite($"Data Source={dbPath}"));

// ─── CORS (para frontend React/Vite u otras clientes) ──────────────────────
builder.Services.AddCors(opt =>
    opt.AddDefaultPolicy(p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// ─── Controladores + JSON enums como strings ────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// ─── OpenAPI nativo (.NET 9/10) ───────────────────────────────────────────────
builder.Services.AddOpenApi();

var app = builder.Build();

// ─── Auto-crear/migrar BD al iniciar ────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// ─── Middleware ───────────────────────────────────────────────────────────────
// OpenAPI nativo: sirve el JSON en /openapi/v1.json
app.MapOpenApi();

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
