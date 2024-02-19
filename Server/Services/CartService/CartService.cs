﻿using BlazorEcommerceApp.Shared.DTOs;
using System.Security.Claims;

namespace BlazorEcommerceApp.Server.Services.CartService
{

    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<CartProductResult>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResult>>
            {
                Data = new List<CartProductResult>()
            };

            foreach (var cartItem in cartItems)
            {
                var product = await _context.Products
                    .Where(p => p.Id == cartItem.ProductId)
                    .FirstOrDefaultAsync();

                if (product is null)
                    continue;

                var productVariant = await _context.ProductVariants
                    .Where(v => v.ProductId == cartItem.ProductId && v.ProductTypeId == cartItem.ProductTypeId)
                    .Include(p => p.ProductType)
                    .FirstOrDefaultAsync();

                if (productVariant is null)
                    continue;

                var cartProduct = new CartProductResult
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    ProductTypeId = productVariant.ProductTypeId,
                    Quantity = cartItem.Quantity
                };

                result.Data.Add(cartProduct);
            }

            return result;
        }

        public async Task<ServiceResponse<List<CartProductResult>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItems =>
                cartItems.UserId = GetUserId()
            );
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var cartItemsCount = (await _context.CartItems.Where(c=>c.UserId==GetUserId()).ToListAsync()).Count;    
            return new ServiceResponse<int> { Data = cartItemsCount };
        }

        public async Task<ServiceResponse<List<CartProductResult>>> GetDbCartProducts()
        {
            return await GetCartProducts(await _context.CartItems
                .Where(ci => ci.UserId == GetUserId()).ToListAsync());
        }
    }
}
