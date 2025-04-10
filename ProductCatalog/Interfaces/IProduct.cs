using ProductCatalog.Models;
using ProductCatalog.ViewModel;

namespace ProductCatalog.Interfaces
{
    public interface IProduct
    {
        public Product AddProduct (ProductViewModel product,string userId,string url);
        public Product UpdateProduct (ProductViewModel product);
        public void DeleteProduct (int id);
        public List<Product> GetAllProducts();
        public Product GetProductById (int id);
        public List<Product> GetAdminsProducts();
        public List<Category> GetCategories();
        public List<Product> EditStartDate();

    }
}
