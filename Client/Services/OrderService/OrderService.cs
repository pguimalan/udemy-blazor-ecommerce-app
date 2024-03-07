using BlazorEcommerceApp.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorEcommerceApp.Client.Services.OrderService
{

    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly IAuthService _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient http,
            IAuthService authenticationStateProvider,
            NavigationManager navigationManager)
        {
            _http = http;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<OrderDetailsResult> GetOrderDetails(int orderId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsResult>>($"api/order/{orderId}");
            return result.Data;
        }

        public async Task<List<OrderOverviewResult>> GetOrders()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResult>>>("api/order");
            return result.Data;
        }

        public async Task<string> PlaceOrder()
        {
            if(await IsUserAuthenticated())
            {
                var result = await _http.PostAsync("api/payment/checkout", null); 
                var url = await result.Content.ReadAsStringAsync();
                return url;
            }
            else
            {
                return "login";
            }
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return await _authenticationStateProvider.IsUserAuthenticated();
        }
    }
}
