global using BlazorECommerce.Shared;
global using Microsoft.EntityFrameworkCore;
global using BlazorECommerce.Server.Data;
global using BlazorECommerce.Server.Services.ProductService;
global using BlazorECommerce.Server.Services.CategoryService;
global using BlazorECommerce.Server.Services.CartService;
global using BlazorECommerce.Server.Services.AuthService;
global using BlazorECommerce.Server.Services.OrderService;
global using BlazorECommerce.Server.Services.PaymentService;
global using BlazorECommerce.Server.Services.AddressService;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAddressService, AddressService>();
//for service [end]

//for authentication [start]
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
//for authentication [end]

//HttpContextAccessor for access User in the Services [start]
builder.Services.AddHttpContextAccessor();
//HttpContextAccessor for access User in the Services [end]

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

//for authentication [start]
app.UseAuthentication();
app.UseAuthorization();
//for authentication [end]

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
