using BlazorEcommerceApp.Shared.DTOs;

namespace BlazorEcommerceApp.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;

        List<Product> Products { get; set; }

        string Message { get; set; }

        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }

        Task GetProducts(string? categoryUrl = null);

        Task<ServiceResponse<Product>> GetProduct(int productId);

        Task SearchProducts(string search, int page);

        Task<List<string>> GetProductSearchSuggestions(string search);
    }
}
