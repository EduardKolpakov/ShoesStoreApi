using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoesStoreApi.Model
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int ShoesId { get; set; }

        [ForeignKey("ShoesId")]
        public Shoes Shoes { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
