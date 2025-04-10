using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ProductCatalog.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }= DateTime.Now;
        public decimal Price { get; set; }
        public DateTime StartDate {  get; set; }
        public DateTime EndData { get; set; }
        public int Duration { get; set; }
        public DateTime? updatedDate { get; set; }
        public string Image { get; set; }
        public bool IsDeleted { get; set; }
        
        //user Name
        public string UserName { get; set; }
        //nav prop
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
