using BlazorEcommerceApp.Shared.DTOs;

namespace BlazorEcommerceApp.Server.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                                   .Include(p => p.Variants)
                                   .Where(p => p.Featured)
                                   .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            var product = await _context.Products
                .Include(p => p.Variants)
                .ThenInclude(t => t.ProductType)
                .FirstOrDefaultAsync(p => p.Id == productId);            

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
                Data = await _context.Products                    
                                    .Include(p => p.Variants)
                                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                                    .Include(p => p.Variants)
                                    .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower())).ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetSearchSuggestions(string search)
        {
            var products = await FindProductsBySearchText(search);

            var result = new List<string>();

            foreach(var product in products)
            {
                if(product.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                    result.Add(product.Title);

                if(product.Description != null)
                {
                    var punctuation = product.Description.Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    var words = product.Description.Split()
                        .Select(s => s.Trim(punctuation));
                    foreach(var word in words)
                    {
                        if(word.Contains(search, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                            result.Add(word);
                    }
                }
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<ProductSearchResult>> SearchProductsAsync(string search, int page)
        {
            var pageResults = 2f;
            var pageCount = Math.Ceiling((await FindProductsBySearchText(search)).Count / pageResults);
            var products = await _context.Products
                                               .Include(p => p.Variants)
                                               .Where(p => p.Title.ToLower().Contains(search.ToLower())
                                               || p.Description.ToLower().Contains(search.ToLower()))
                                               .Skip((page - 1) * (int)pageResults)
                                               .Take((int)pageResults)
                                               .ToListAsync();

            var response = new ServiceResponse<ProductSearchResult>
            {
                Data = new ProductSearchResult
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        private Task<List<Product>> FindProductsBySearchText(string search)
        {
            return _context.Products
                                               .Include(p => p.Variants)
                                               .Where(p => p.Title.ToLower().Contains(search.ToLower())
                                               || p.Description.ToLower().Contains(search.ToLower())).ToListAsync();
        }
    }
}
