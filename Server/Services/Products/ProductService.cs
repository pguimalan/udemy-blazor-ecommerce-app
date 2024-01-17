namespace BlazorEcommerceApp.Server.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            var response = new ServiceResponse<Product>();

            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, this product does not exist";
            }
            else response.Data = product;

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.ToListAsync()
            };

            return response;
        }
    }
}
