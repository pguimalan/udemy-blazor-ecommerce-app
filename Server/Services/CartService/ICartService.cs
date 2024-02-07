namespace BlazorEcommerceApp.Server.Services.CartService
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResult>>> GetCartProducts(List<CartItem> cartItems);
    }
}
