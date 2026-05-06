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

// ─── Swagger / OpenAPI ───────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Mecánico API",
        Version = "v1",
        Description = "API REST para gestión de clientes, autos e historial de servicios/reparaciones"
    });
});

var app = builder.Build();

// ─── Auto-crear/migrar BD al iniciar ────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// ─── Middleware ───────────────────────────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mecánico API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
