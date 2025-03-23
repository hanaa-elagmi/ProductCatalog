using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.ViewModel
{
    public class ProductViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }

        [RegularExpression(@"^(1[0-9]|20|[1-9])$", ErrorMessage = "Duration must be between 1 and 20.")]
        public int Duration { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public bool IsDeleted { get; set; }

        //user id
        //public string UserId { get; set; }

        public int CategoryId { get; set; }
    }
}
