using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorEcommerceApp.Shared
{
    public class ProductVariant
    {
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        
        public int ProductTypeId { get; set; }

        public ProductType ProductType { get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }
    }
}
