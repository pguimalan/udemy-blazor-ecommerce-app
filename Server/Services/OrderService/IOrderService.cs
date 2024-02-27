using BlazorEcommerceApp.Shared.DTOs;

namespace BlazorEcommerceApp.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();

        Task<ServiceResponse<List<OrderOverviewResult>>> GetOrders();

        Task<ServiceResponse<OrderDetailsResult>> GetOrderDetails(int orderId);
    }
}
