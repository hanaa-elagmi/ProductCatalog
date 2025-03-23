using ProductCatalog.Models;

namespace ProductCatalog.ViewModel
{
    public class PaginationViewModel
    {
           public List<Product> Products { get; set; }=new List<Product>();
           public List<Category> Categories { get; set; } =new List<Category>();
           public int PageIndex { get; set; }
           public int PageCount { get; set; }
           public int CategoryId { get; set; }


    }
}
