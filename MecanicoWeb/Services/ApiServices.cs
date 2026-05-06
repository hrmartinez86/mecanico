using System.Net.Http.Json;
using MecanicoWeb.Models;

namespace MecanicoWeb.Services;

public class ClienteService
{
    private readonly HttpClient _http;
    public ClienteService(HttpClient http) => _http = http;

    public Task<List<ClienteResponse>?> GetAllAsync(string? search = null)
    {
        var url = string.IsNullOrWhiteSpace(search)
            ? "/api/clientes"
            : $"/api/clientes?search={Uri.EscapeDataString(search)}";
        return _http.GetFromJsonAsync<List<ClienteResponse>>(url);
    }

    public Task<ClienteResponse?> GetByIdAsync(int id) =>
        _http.GetFromJsonAsync<ClienteResponse>($"/api/clientes/{id}");

    public async Task<(ClienteResponse? data, string? error)> CreateAsync(ClienteForm form)
    {
        var res = await _http.PostAsJsonAsync("/api/clientes", form);
        if (res.IsSuccessStatusCode)
            return (await res.Content.ReadFromJsonAsync<ClienteResponse>(), null);
        var err = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        return (null, err?.Message ?? "Error al crear cliente");
    }

    public async Task<(ClienteResponse? data, string? error)> UpdateAsync(int id, ClienteForm form)
    {
        var res = await _http.PutAsJsonAsync($"/api/clientes/{id}", form);
        if (res.IsSuccessStatusCode)
            return (await res.Content.ReadFromJsonAsync<ClienteResponse>(), null);
        var err = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        return (null, err?.Message ?? "Error al actualizar cliente");
    }

    public Task<HttpResponseMessage> DeleteAsync(int id) =>
        _http.DeleteAsync($"/api/clientes/{id}");
}

public class AutoService
{
    private readonly HttpClient _http;
    public AutoService(HttpClient http) => _http = http;

    public Task<List<AutoResponse>?> GetAllAsync(int? clienteId = null, string? search = null)
    {
        var qs = new List<string>();
        if (clienteId.HasValue) qs.Add($"clienteId={clienteId}");
        if (!string.IsNullOrWhiteSpace(search)) qs.Add($"search={Uri.EscapeDataString(search)}");
        var url = qs.Count > 0 ? $"/api/autos?{string.Join("&", qs)}" : "/api/autos";
        return _http.GetFromJsonAsync<List<AutoResponse>>(url);
    }

    public Task<AutoResponse?> GetByIdAsync(int id) =>
        _http.GetFromJsonAsync<AutoResponse>($"/api/autos/{id}");

    public async Task<(AutoResponse? data, string? error)> CreateAsync(AutoForm form)
    {
        var res = await _http.PostAsJsonAsync("/api/autos", form);
        if (res.IsSuccessStatusCode)
            return (await res.Content.ReadFromJsonAsync<AutoResponse>(), null);
        var err = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        return (null, err?.Message ?? "Error al crear auto");
    }

    public async Task<(AutoResponse? data, string? error)> UpdateAsync(int id, AutoForm form)
    {
        var res = await _http.PutAsJsonAsync($"/api/autos/{id}", form);
        if (res.IsSuccessStatusCode)
            return (await res.Content.ReadFromJsonAsync<AutoResponse>(), null);
        var err = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        return (null, err?.Message ?? "Error al actualizar auto");
    }

    public Task<HttpResponseMessage> DeleteAsync(int id) =>
        _http.DeleteAsync($"/api/autos/{id}");
}

public class ServicioService
{
    private readonly HttpClient _http;
    public ServicioService(HttpClient http) => _http = http;

    public Task<List<ServicioResponse>?> GetHistorialAsync(int autoId) =>
        _http.GetFromJsonAsync<List<ServicioResponse>>($"/api/servicios/auto/{autoId}/historial");

    public Task<ServicioResponse?> GetByIdAsync(int id) =>
        _http.GetFromJsonAsync<ServicioResponse>($"/api/servicios/{id}");

    public async Task<(ServicioResponse? data, string? error)> CreateAsync(ServicioForm form)
    {
        var payload = new
        {
            form.AutoId,
            form.Tipo,
            form.Titulo,
            form.Descripcion,
            form.Diagnostico,
            form.TrabajoRealizado,
            form.CostoManoObra,
            form.CostoPiezas,
            form.CostoTotal,
            Fecha = form.Fecha,
            FechaTermino = form.FechaTermino,
            form.Mecanico,
            form.Estado,
            form.KilometrajeEntrada,
            form.Observaciones
        };
        var res = await _http.PostAsJsonAsync("/api/servicios", payload);
        if (res.IsSuccessStatusCode)
            return (await res.Content.ReadFromJsonAsync<ServicioResponse>(), null);
        var err = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        return (null, err?.Message ?? "Error al guardar servicio");
    }

    public async Task<(ServicioResponse? data, string? error)> UpdateAsync(int id, ServicioForm form)
    {
        var payload = new
        {
            form.Tipo,
            form.Titulo,
            form.Descripcion,
            form.Diagnostico,
            form.TrabajoRealizado,
            form.CostoManoObra,
            form.CostoPiezas,
            form.CostoTotal,
            Fecha = form.Fecha,
            FechaTermino = form.FechaTermino,
            form.Mecanico,
            form.Estado,
            form.KilometrajeEntrada,
            form.Observaciones
        };
        var res = await _http.PutAsJsonAsync($"/api/servicios/{id}", payload);
        if (res.IsSuccessStatusCode)
            return (await res.Content.ReadFromJsonAsync<ServicioResponse>(), null);
        var err = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        return (null, err?.Message ?? "Error al actualizar servicio");
    }

    public Task<HttpResponseMessage> DeleteAsync(int id) =>
        _http.DeleteAsync($"/api/servicios/{id}");
}

// Auxiliar para deserializar errores de la API
file record ErrorResponse(string? Message);
