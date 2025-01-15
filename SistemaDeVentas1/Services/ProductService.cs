using SistemaDeVentas1.Interfaces;
using SistemaDeVentas1.Models;

namespace SistemaDeVentas1.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Product?> AddProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7299/api/product", product);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Product>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al agregar producto: {error}");
        }

        public async Task<Product?> DeleteProductAsync(int productId)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7299/api/product/{productId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Product>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al eliminar producto: {error}");
        }

        public async Task<List<Product>> GetAllProductAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7299/api/product");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Product>>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener productos: {error}");
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7299/api/product/{productId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Product>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener producto: {error}");
        }

        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7299/api/product/{id}", product);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Product>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al actualizar producto: {error}");
        }
    }
}
