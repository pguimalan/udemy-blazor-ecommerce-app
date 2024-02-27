using BlazorEcommerceApp.Shared.DTOs;

namespace BlazorEcommerceApp.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task PlaceOrder();

        Task<List<OrderOverviewResult>> GetOrders();

        Task<OrderDetailsResult> GetOrderDetails(int orderId);
    }
}
