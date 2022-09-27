global using BlazorECommerce.Shared;
global using Microsoft.EntityFrameworkCore;
global using BlazorECommerce.Server.Data;
global using BlazorECommerce.Server.Services.ProductService;
global using BlazorECommerce.Server.Services.CategoryService;
global using BlazorECommerce.Server.Services.CartService;
global using BlazorECommerce.Server.Services.AuthService;
using Microsoft.AspNetCore.ResponseCompression;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//for DataContext [start]
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//for DataContext [end]

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//for swagger [start]
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//for swagger [end]

//for service [start]
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
//for service [end]

var app = builder.Build();

//for swagger [start]
app.UseSwaggerUI();
//for swagger [end]

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//for swagger [start]
app.UseSwagger();
//for swagger [end]

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
