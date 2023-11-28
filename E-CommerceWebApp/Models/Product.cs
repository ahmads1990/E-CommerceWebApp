namespace E_CommerceWebApp.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public ProductImage? ProductImage { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
    }
}
