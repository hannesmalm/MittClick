namespace MittClick.Models
{
    public interface IImageService
    {
        byte[] ConvertToByteArray(IFormFile file);
    }

    public class ImageService : IImageService
    {
        public byte[] ConvertToByteArray(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }

}
