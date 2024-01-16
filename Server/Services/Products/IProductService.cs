namespace BlazorEcommerceApp.Server.Services.Products
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProdudctsAsync();
    }
}
