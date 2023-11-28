namespace E_CommerceWebApp.ViewModel
{
    public class CreateProductViewModel
    {
        [Display(Name ="Product Image")]
        public IFormFile ProductImage { get; set; }
        [Required, Display(Name ="Product Name")]
        public string ProductName { get; set; }
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }

        public Product toProduct()
        {
            byte[] productImageBytes = null;
            if (ProductImage != null) {
                using (var memoryStream = new MemoryStream())
                {
                    // Copy the file content to the memory stream
                    ProductImage.CopyTo(memoryStream);
                    // Get the byte array
                    productImageBytes = memoryStream.ToArray();
                }
            }
            return new Product
            {
                ProductName = ProductName,
                Description = Description,
                Price = Price,
                ProductImage = new ProductImage { CreatedAt = DateTime.Now, ImageData = productImageBytes }
            };
        }
    }
}
