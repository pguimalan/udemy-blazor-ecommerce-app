using System.Net.Http.Json;

namespace BlazorEcommerceApp.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public List<Product> Products { get; set; }

        public ProductService(HttpClient httpClient)
        {
            Products = new List<Product>();
            _httpClient = httpClient;
        }

        public async Task GetProducts()
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product");

            if (result != null && result.Data != null)
                Products = result.Data;

        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var product = await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return product;
        }
    }
}
