namespace BlazorEcommerceApp.Shared.DTOs
{
    public class OrderDetailsResult
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderDetailsProductsResult> Products { get; set; }
    }
}
