namespace E_CommerceWebApp.Services
{
    public class ImageService
    {
        public static readonly List<string> AllowedImageTypes = new List<string> { "png", "jpg", "jpeg" };
        public static readonly float ImageMaxSizeMB = 2;

        public bool CheckImage(IFormFile imageFile)
        {
            // Check image size
            float imageSize = ByteToMegaByte(imageFile.Length);
            if (imageSize == 0 || imageSize>ImageMaxSizeMB) 
                return false; 

            // Check image extention
            string imageType = imageFile.ContentType.Split("/").Last();
            if (!AllowedImageTypes.Contains(imageType))
                return false;

            return true;
        }
        private float ByteToMegaByte(float bytes) => bytes / 1000000;
    }
}
