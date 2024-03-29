global using BlazorECommerce.Shared;
global using System.Net.Http.Json;
global using BlazorECommerce.Client.Services.ProductService;
global using BlazorECommerce.Client.Services.CategoryService;
global using BlazorECommerce.Client.Services.CartService;
global using BlazorECommerce.Client.Services.AuthService;
global using BlazorECommerce.Client.Services.OrderService;
global using BlazorECommerce.Client.Services.AddressService;
global using BlazorECommerce.Client.Services.ProductTypeService;
global using Microsoft.AspNetCore.Components.Authorization;
using BlazorECommerce.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//for Blazored.LocalStorage [start]
builder.Services.AddBlazoredLocalStorage();
//for Blazored.LocalStorage [end]

builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//for services [start]
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
//for services [end]

//for AuthenticationStateProvider [start]
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
//for AuthenticationStateProvider [end]

await builder.Build().RunAsync();
