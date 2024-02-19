using BlazorEcommerceApp.Shared.DTOs;

namespace BlazorEcommerceApp.Server.Services.CartService
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResult>>> GetCartProducts(List<CartItem> cartItems);

        Task<ServiceResponse<List<CartProductResult>>> StoreCartItems(List<CartItem> cartItems);

        Task<ServiceResponse<int>> GetCartItemsCount();

        Task<ServiceResponse<List<CartProductResult>>> GetDbCartProducts();        
    }
}
