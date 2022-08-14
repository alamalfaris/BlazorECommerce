
namespace BlazorECommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        Task GetProducts(string? categoryUrl = null);//kalo gak ada categoryUrl, defaultnya jadi null
        Task<ServiceResponse<Product>> GetProduct(int productId);
    }
}
