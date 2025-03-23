using Azure;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Interfaces;
using ProductCatalog.Models;
using ProductCatalog.Reposatories;
using ProductCatalog.ViewModel;
using System.Diagnostics;

namespace ProductCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProduct productRepo;

        public HomeController(IProduct productRepo)
        {
            this.productRepo = productRepo;
        }
        public IActionResult Index(PaginationViewModel pagination)
        {
            pagination.Products = productRepo.GetAllProducts();
            pagination.Categories = productRepo.GetCategories();

            if (pagination.PageIndex < 1)
            {
                pagination.PageIndex = 1;
            }
            if (pagination.CategoryId !=0)
            {
                pagination.Products = pagination.Products.Where(p => p.CategoryId == pagination.CategoryId).ToList();
            }
           

            var totalProducts = pagination.Products.Count();

            var totalPages = (int)Math.Ceiling(totalProducts / (double)8);

            if (pagination.PageIndex > totalPages)
            {
                pagination.PageIndex = totalPages;
            }

            pagination.PageCount = totalPages;
            pagination.Products = pagination.Products.Skip((pagination.PageIndex - 1) * 8).Take(8).ToList();
            return View(pagination);
            
        }

        public IActionResult Details(int id)
        {
            var product = productRepo.GetProductById(id);
            return View(product);
        }
        
    }
}
