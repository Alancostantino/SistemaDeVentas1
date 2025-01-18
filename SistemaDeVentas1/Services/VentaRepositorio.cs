using SistemaDeVentas1.Interfaces;
using SistemaDeVentas1.Models;
using System.Globalization;
using System.Text.Json;

namespace SistemaDeVentas1.Services
{
    public class VentaRepositorio : IVentaRepositorio
    {
        private readonly HttpClient _httpClient;

        public VentaRepositorio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Registrar una venta
        public async Task<Venta> Registrar(Venta venta)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7299/api/venta/registrar", venta);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Venta>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al registrar la venta. Detalles: {errorMessage}");
            }
        }

        // Historial de ventas filtrado por número de venta o fecha
        public async Task<List<Venta>> Historial(string numeroVenta, string fechaInicio, string fechaFin)
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(numeroVenta)) queryParams.Add($"numeroVenta={numeroVenta}");
            if (!string.IsNullOrWhiteSpace(fechaInicio)) queryParams.Add($"fechaInicio={fechaInicio}");
            if (!string.IsNullOrWhiteSpace(fechaFin)) queryParams.Add($"fechaFin={fechaFin}");

            string queryString = string.Join("&", queryParams);
            string url = $"https://localhost:7299/api/venta/historial?{queryString}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener historial: {errorContent}");
            }

            return await response.Content.ReadFromJsonAsync<List<Venta>>();
        }

        // Obtener reporte de ventas
       
    }
}
