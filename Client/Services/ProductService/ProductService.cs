using BlazorEcommerceApp.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerceApp.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public event Action ProductsChanged;

        public List<Product> Products { get; set; }
        public string Message { get; set; } = "Loading Products...";

        public ProductService(HttpClient httpClient)
        {
            Products = new List<Product>();
            _httpClient = httpClient;
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl == null ? 
                await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured") 
                : await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");

            if (result != null && result.Data != null)
                Products = result.Data;

            ProductsChanged.Invoke();
        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var product = await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return product;
        }

        public async Task SearchProducts(string search)
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{search}");

            if(result != null && result.Data != null)
                Products = result.Data;
            if(Products.Count == 0)
                Message = "No prodcuts found";
            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string search)
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchSuggestions/{search}");
            return result.Data;
        }
    }
}
