using System.ComponentModel.DataAnnotations;

namespace Task.Model
{
    public class Product
    {
        //name, description, quantity, and price.
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string? Name { get; set; }
       
        public string? Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
