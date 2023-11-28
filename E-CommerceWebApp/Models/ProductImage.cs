using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceWebApp.Models
{
    [Table("ProductImage")]
    public class ProductImage
    {
        public int ProductImageID { get; set; }
        public byte[] ImageData { get; set; }
        public DateTime CreatedAt { get; set; }
        
    }
}
