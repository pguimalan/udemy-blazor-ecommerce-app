using BlazorEcommerceApp.Client.Pages;
using BlazorEcommerceApp.Shared.DTOs;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Net.Security;
using static System.Net.WebRequestMethods;

namespace BlazorEcommerceApp.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CartService(ILocalStorageService localStorage, HttpClient http, AuthenticationStateProvider authenticationStateProvider)
        {
            _localStorage = localStorage;
            _http = http;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            if (await IsUserAuthenticated())
            {
                Console.WriteLine("User is authenticated.");
            }
            else
            {
                Console.WriteLine("User is not authenticated.");
            }

            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();            

            var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.ProductTypeId);

            if (sameItem == null)
                cart.Add(cartItem);
            else
                sameItem.Quantity += cartItem.Quantity;

            await _localStorage.SetItemAsync("cart", cart);
            await GetCartItemsCount();
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task<List<CartProductResult>> GetCartProducts()
        {
            if(await IsUserAuthenticated())
            {
                var response = await _http.GetFromJsonAsync<ServiceResponse<List<CartProductResult>>>("api/cart");
                return response.Data;
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
                var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);
                var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResult>>>();
                return cartProducts.Data;
            }
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            
            if (cart is null)
                return;

            var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);
            if (cart != null)
            {
                cart.Remove(cartItem);
                await _localStorage.SetItemAsync("cart", cart);
                await GetCartItemsCount();
            }
                
        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
           var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (localCart is null)
                return;

            await _http.PostAsJsonAsync("api/cart", localCart);

            if (emptyLocalCart)
                await _localStorage.RemoveItemAsync("cart");
        }

        public async Task UpdateQuantity(CartProductResult cartProduct)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (cart is null)
                return;

            var cartItem = cart.Find(x => x.ProductId == cartProduct.ProductId && x.ProductTypeId == cartProduct.ProductTypeId);
            if (cart != null)
            {
                cartItem.Quantity = cartProduct.Quantity;
                await _localStorage.SetItemAsync("cart", cart);
                OnChange.Invoke();
            }
        }

        public async Task GetCartItemsCount()
        {
            if (await IsUserAuthenticated())
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
            }

            OnChange.Invoke();
        }
    }
}
