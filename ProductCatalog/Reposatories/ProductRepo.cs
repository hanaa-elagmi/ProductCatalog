using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductCatalog.Data;
using ProductCatalog.Interfaces;
using ProductCatalog.Models;
using ProductCatalog.ViewModel;



namespace ProductCatalog.Reposatories
{
    public class ProductRepo : IProduct
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Product AddProduct(ProductViewModel productVM,string userId,string url)
        {
            Product product = new Product();
            product.Name = productVM.Name;
            product.Price = productVM.Price;
            product.Image = url;
            product.CreationDate = productVM.CreationDate;
            product.Duration = productVM.Duration;
            product.StartDate = productVM.StartDate;
            product.UserId=userId;
            product.CategoryId = productVM.CategoryId;
            product.EndData= product.StartDate.AddDays(product.Duration);
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            return product;
        }

        public void DeleteProduct(int id)
        {
            var product=dbContext.Products.Find(id);
            product.IsDeleted=!product.IsDeleted;
            product.updatedDate = DateTime.Now;
            dbContext.SaveChanges();
        }

        public List<Product> GetAllProducts()
        {
            var products = dbContext.Products.Where(p => p.IsDeleted == false&& p.EndData >=DateTime.Now).ToList();
            
           return products;
        }

        public Product GetProductById(int id)
        {
            var product = dbContext.Products.Find(id);
            return product;
        }

        public Product UpdateProduct(ProductViewModel newproduct)
        {
            
            var oldproduct = dbContext.Products.Find(newproduct.Id);
            if (oldproduct != null)
            {
                oldproduct.Duration = newproduct.Duration;
                oldproduct.StartDate = newproduct.StartDate;
                oldproduct.Price = newproduct.Price;
                oldproduct.Name = newproduct.Name;
                var projectFolder = Directory.GetCurrentDirectory();
                var relativeImagesPath = Path.Combine("wwwroot", "Images");
                var fullImagesPath = Path.Combine(projectFolder, relativeImagesPath);
                var fileName = $"{Guid.NewGuid()}_{newproduct.Image.FileName}";
                var fullImagePath = Path.Combine(fullImagesPath, fileName);
                using (var stream = new FileStream(fullImagePath, FileMode.Create))
                {
                    newproduct.Image.CopyTo(stream);
                    stream.Flush();
                }

                oldproduct.Image = $"/Images/{fileName}";
                oldproduct.CategoryId=newproduct.CategoryId;
                oldproduct.updatedDate = DateTime.Now;
                oldproduct.EndData = newproduct.StartDate.AddDays(newproduct.Duration);

                dbContext.Update(oldproduct);
                dbContext.SaveChanges();
                return oldproduct;
            }
            else
            {
                return null;
            }
        }

        public List<Product> GetAdminsProducts()
        {
            var products= dbContext.Products.ToList();
            return products;
        }

        public List<Category> GetCategories() 
        {
            var categories= dbContext.Categories.ToList();
            return categories;
        }
    }
}
