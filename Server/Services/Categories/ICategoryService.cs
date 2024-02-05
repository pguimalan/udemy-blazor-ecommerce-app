﻿namespace BlazorEcommerceApp.Server.Services.Categories
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
    }
}