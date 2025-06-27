using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoesStoreApi.Model
{
    public class Shoes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public double Price { get; set; }
        public string Sizes { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
