using BlazorEcommerceApp.Shared.DTOs;
using Stripe.Checkout;

namespace BlazorEcommerceApp.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
        Task<ServiceResponse<bool>> FullfillOrder(HttpRequest request);
    }
}
