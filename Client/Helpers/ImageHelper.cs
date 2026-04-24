namespace ProjectMaui.Client.Helpers
{
    public static class ImageHelper
    {
        public static ImageSource Resolve(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return "placeholder.jpg";

            var localPath = Path.Combine(
                FileSystem.AppDataDirectory, "product-images", imagePath);

            if (File.Exists(localPath))
                return ImageSource.FromFile(localPath);

            var fileName = Path.GetFileName(imagePath);
            return ImageSource.FromFile(fileName);
        }
    }
}