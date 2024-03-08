global using BlazorEcommerceApp.Shared;
global using BlazorEcommerceApp.Client;
global using BlazorEcommerceApp.Client.Services.AuthService;
global using BlazorEcommerceApp.Client.Services.CartService;
global using BlazorEcommerceApp.Client.Services.CategoryService;
global using BlazorEcommerceApp.Client.Services.ProductService;
global using BlazorEcommerceApp.Client.Services.OrderService;
global using BlazorEcommerceApp.Client.Services.AddressService;
global using Microsoft.AspNetCore.Components.Authorization;
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
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
