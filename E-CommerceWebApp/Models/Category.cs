namespace E_CommerceWebApp.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        // nav prop
        public IEnumerable<Product> Products { get; set; }
    }
}
