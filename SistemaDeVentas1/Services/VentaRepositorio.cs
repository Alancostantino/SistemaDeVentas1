using SistemaDeVentas1.Interfaces;
using SistemaDeVentas1.Models;
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

        public async Task<Venta> Registrar(Venta entidad)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7299/api/venta/registrar", entidad);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Venta>();
            }
            else
            {
                throw new Exception("Error al registrar la venta.");
            }
        }

        public async Task<List<Venta>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            // Codificar los parámetros para evitar problemas con caracteres especiales
            var url = $"https://localhost:7299/api/venta/historial?buscarPor={Uri.EscapeDataString(buscarPor)}&fechaInicio={Uri.EscapeDataString(fechaInicio)}&fechaFin={Uri.EscapeDataString(fechaFin)}";

            // Agregar numeroVenta solo si buscarPor es "NumeroDocumento" y numeroVenta no está vacío
            if (buscarPor == "NumeroDocumento" && !string.IsNullOrWhiteSpace(numeroVenta))
            {
                url += $"&numeroVenta={Uri.EscapeDataString(numeroVenta)}";
            }

            try
            {
                Console.WriteLine($"URL generada: {url}");  // Verifica la URL generada

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var ventas = await response.Content.ReadFromJsonAsync<List<Venta>>();
                    if (ventas == null)
                    {
                        throw new Exception("La respuesta del servidor no contiene datos válidos.");
                    }
                    return ventas;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error del servidor: {errorMessage}");  // Verifica el error del servidor
                    throw new Exception($"Error al obtener el historial de ventas. Código de estado: {response.StatusCode}. Detalles: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error de red al intentar obtener el historial de ventas.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Error al deserializar la respuesta del servidor.", ex);
            }
        }
        public async Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin)
        {
            var url = $"https://localhost:7299/api/venta/reporte?FechaInicio={fechaInicio}&FechaFin={fechaFin}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DetalleVenta>>();
            }
            else
            {
                throw new Exception("Error al generar el reporte.");
            }
        }
    }

}
