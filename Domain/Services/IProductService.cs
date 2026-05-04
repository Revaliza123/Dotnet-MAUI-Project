using Microsoft.Maui.Media;
using ProjectMaui.Domain.Models;

namespace ProjectMaui.Domain.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task AddProduct(Product product, Product.ProductTypes type, FileResult? photo);
        Task UpdateProduct(Product product, Product.ProductTypes type, FileResult? newPhoto);
        Task DeleteProduct(Guid productId, Product.ProductTypes type);
        Task<string> SaveImageLocally(FileResult photo, Product product);
    }
}
