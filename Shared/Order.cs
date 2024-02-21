using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime;

namespace BlazorEcommerceApp.Shared
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDateTime { get; set; } = DateTime.Now;

        [Column(TypeName ="decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}