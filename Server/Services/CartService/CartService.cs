﻿using BlazorEcommerceApp.Server.Migrations;
using BlazorEcommerceApp.Server.Services.AuthService;
using BlazorEcommerceApp.Shared;
using BlazorEcommerceApp.Shared.DTOs;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace BlazorEcommerceApp.Server.Services.CartService
{

    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public CartService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

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
                cartItems.UserId = _authService.GetUserId()
            );
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var cartItemsCount = (await _context.CartItems.Where(c => c.UserId == _authService.GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int> { Data = cartItemsCount };
        }

        public async Task<ServiceResponse<List<CartProductResult>>> GetDbCartProducts(int? userId = null)
        {
            userId ??= _authService.GetUserId();

            return await GetCartProducts(await _context.CartItems
                .Where(ci => ci.UserId == userId).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = _authService.GetUserId();

            var sameItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId && ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == cartItem.UserId);

            if (sameItem == null)
            {
                _context.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId && ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == _authService.GetUserId());

            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist."
                };
            }

            dbCartItem.Quantity = cartItem.Quantity;
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int prooductId, int productTypeId)
        {
            var dbCartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == prooductId && ci.ProductTypeId == productTypeId && ci.UserId == _authService.GetUserId());

            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist."
                };
            }

            _context.CartItems.Remove(dbCartItem);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
