namespace BlazorEcommerceApp.Server.Services.Products
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();

        Task<ServiceResponse<Product>> GetProductAsync(int productId);

        Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);
        
        Task<ServiceResponse<List<Product>>> SearchProductsAsync(string search);

        Task<ServiceResponse<List<string>>> GetSearchSuggestions(string search);

        Task<ServiceResponse<List<Product>>> GetFeaturedProductsAsync();
    }
}
