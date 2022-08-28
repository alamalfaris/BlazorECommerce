global using BlazorECommerce.Shared;
global using System.Net.Http.Json;
global using BlazorECommerce.Client.Services.ProductService;
global using BlazorECommerce.Client.Services.CategoryService;
global using BlazorECommerce.Client.Services.CartService;
using BlazorECommerce.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//for Blazored.LocalStorage [start]
builder.Services.AddBlazoredLocalStorage();
//for Blazored.LocalStorage [end]

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//for services [start]
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
//for services [end]

await builder.Build().RunAsync();
