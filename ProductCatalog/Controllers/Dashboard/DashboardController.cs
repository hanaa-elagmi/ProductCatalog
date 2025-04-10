using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Interfaces;
using ProductCatalog.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ProductCatalog.Models;
using ProductCatalog.Reposatories;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Controllers.Dashboard
{
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {
        private readonly IProduct productRepo;

        public DashboardController(IProduct ProductRepo)
        {
            productRepo = ProductRepo;
        }
        public IActionResult Index(PaginationViewModel pagination)
        {
            var Products=productRepo.GetAdminsProducts();
            pagination.Products = Products;

            if (pagination.PageIndex < 1)
            {
                pagination.PageIndex = 1;
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

        public IActionResult CreateProduct()
        {
            var categories = productRepo.GetCategories().ToList();
            ViewBag.categories = categories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(ProductViewModel productViewModel)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);


            if (ModelState.IsValid && userName != null) 
            {
              
                //var userid = userId;
                //productRepo.AddProduct(productViewModel, userid);

                var projectFolder = Directory.GetCurrentDirectory();
                var relativeImagesPath = Path.Combine("wwwroot", "Images");
                var fullImagesPath = Path.Combine(projectFolder, relativeImagesPath);
                var fileName = $"{Guid.NewGuid()}_{productViewModel.Image.FileName}";
                var fullImagePath = Path.Combine(fullImagesPath, fileName);
                using (var stream = new FileStream(fullImagePath, FileMode.Create))
                {
                    productViewModel.Image.CopyTo(stream);
                    stream.Flush();
                }
                var url = $"{Request.Scheme}://{Request.Host}/Images/{fileName}";
                 var myProduct=productRepo.AddProduct(productViewModel, userName, url);




                return RedirectToAction(nameof(Index));
            }

            return View(productViewModel);
        }
        public IActionResult EditProduct(int id)
        {
            var categories = productRepo.GetCategories().ToList();
            ViewBag.categories = categories;
            var product=productRepo.GetProductById(id);
            if (product != null) 
            {
                ProductViewModel productViewModel=new ProductViewModel();
                productViewModel.CategoryId = product.CategoryId;
                productViewModel.Name = product.Name;
                productViewModel.Id= id;
                productViewModel.Price = product.Price;
                productViewModel.IsDeleted = product.IsDeleted;
                productViewModel.Duration= product.Duration;
                productViewModel.StartDate = product.StartDate;

                return View(productViewModel);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(ProductViewModel product)
        {
            
            var result=productRepo.UpdateProduct(product);
            if(result is null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            productRepo.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult EditStartDate()
        {
            productRepo.EditStartDate();
            return RedirectToAction(nameof(Index));
        }

    }
}
