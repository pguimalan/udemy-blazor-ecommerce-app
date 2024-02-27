namespace BlazorEcommerceApp.Shared.DTOs
{
    public class OrderOverviewResult
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Product { get; set; }
        public string ProductImgUrl { get; set; }
    }

    public class OrderDetailsProductsResult
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ProductType { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
