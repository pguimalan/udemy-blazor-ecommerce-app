global using BlazorEcommerceApp.Shared;

using BlazorEcommerceApp.Client;
using BlazorEcommerceApp.Client.Services.CartService;
using BlazorEcommerceApp.Client.Services.CategoryService;
using BlazorEcommerceApp.Client.Services.ProductService;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();


builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
