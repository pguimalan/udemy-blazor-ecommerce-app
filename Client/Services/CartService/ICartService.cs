﻿using BlazorEcommerceApp.Shared.DTOs;

namespace BlazorEcommerceApp.Client.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartProductResult>> GetCartProducts();
        Task RemoveProductFromCart(int productId, int productTypeId);
        Task UpdateQuantity(CartProductResult cartProduct);
        Task StoreCartItems(bool emptyLocalCart);        
        Task GetCartItemsCount();
    }
}
