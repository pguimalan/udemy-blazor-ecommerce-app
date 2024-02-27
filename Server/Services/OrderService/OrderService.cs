using BlazorEcommerceApp.Server.Services.AuthService;
using BlazorEcommerceApp.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorEcommerceApp.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(DataContext context, ICartService cartService, IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<OrderDetailsResult>> GetOrderDetails(int orderId)
        {
            var result = new ServiceResponse<OrderDetailsResult>();
            var order = await _context.Orders
                            .Include(o => o.OrderItems)
                            .ThenInclude(p => p.Product)
                            .Include(oi => oi.OrderItems)
                            .ThenInclude(pt => pt.ProductType)
                            .Where(o => o.UserId == _authService.GetUserId() && o.OrderId == orderId)
                            .OrderByDescending(d => d.OrderDateTime)
                            .FirstOrDefaultAsync();

            if(order == null)
            {
                result.Success = false;
                result.Message = "Order not found.";
                return result;
            }

            var orderDetailsResponse = new OrderDetailsResult
            {
                OrderDate = order.OrderDateTime,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductsResult>()
            };

            order.OrderItems.ForEach(item =>
                orderDetailsResponse.Products.Add(new OrderDetailsProductsResult
                {
                    ProductId = item.ProductId,
                    ImageUrl = item.Product.ImageUrl,
                    ProductType = item.ProductType.Name,
                    Quantity = item.Quantity.Value,
                    Title = item.Product.Title,
                    Price = item.TotalPrice
                })
            );

            result.Data = orderDetailsResponse;

            return result;

        }

        public async Task<ServiceResponse<List<OrderOverviewResult>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverviewResult>>();
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == _authService.GetUserId())
                .OrderByDescending(o => o.OrderDateTime)
                .ToListAsync();

            var orderResult = new List<OrderOverviewResult>();
            orders.ForEach(o => orderResult.Add(new OrderOverviewResult
            {
                Id = o.OrderId,
                OrderDate = o.OrderDateTime,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ? $"{o.OrderItems.First().Product.Title} and " +
                $"{o.OrderItems.Count - 1} more..." : 
                o.OrderItems.First().Product.Title,
                ProductImgUrl = o.OrderItems.First().Product.ImageUrl
            }));

            response.Data = orderResult;
            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;
            products.ForEach(product => totalPrice += product.Price * product.Quantity.Value);

            var orderItems = new List<OrderItem>();
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity.Value
            }));

            var order = new Order
            {
                UserId = _authService.GetUserId(),
                OrderDateTime = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);

            _context.CartItems.RemoveRange(_context.CartItems.Where(c => c.UserId == _authService.GetUserId()));

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
