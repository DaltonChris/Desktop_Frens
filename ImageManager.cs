using System.IO;
using System.Windows.Media.Imaging;

namespace Desktop_Frens
{
    public class ImageManager
    {
        // Singleton instance
        private static ImageManager? _instance;
        public static ImageManager Instance
        {
            get
            {
                _instance ??= new ImageManager();
                return _instance;
            }
        }

        private ImageManager()
        {
            // No need to instantiate ResourceManager here, as it's handled by Re_Source
        }

        public static byte[] GetImageData(string imageName)
        {
            try
            {
                byte[] imageData;

                // Get the image data from Re_Source using the imageName
                imageData = imageName switch
                {
                    "Dog_1" => Re_Source.Dog_1,
                    "Dog_2" => Re_Source.Dog_2,
                    "Dog_3" => Re_Source.Dog_3,
                    "Dog_4" => Re_Source.Dog_4,
                    "Dog_5" => Re_Source.Dog_5,
                    "Dog_6" => Re_Source.Dog_6,
                    "Dog_7" => Re_Source.Dog_7,
                    "Slug_1" => Re_Source.Slug_1,
                    "Slug_2" => Re_Source.Slug_2,
                    "Slug_3" => Re_Source.Slug_3,
                    "Slug_4" => Re_Source.Slug_4,
                    "Slug_5" => Re_Source.Slug_3,
                    "Slug_6" => Re_Source.Slug_2,
                    _ => throw new ArgumentException($"Image '{imageName}' not found."),
                };


                // Convert byte array to Bitmap
                if (imageData != null)
                {
                    return imageData;
                }
                else
                {
                    // Handle case where imageData is null
                    throw new InvalidOperationException("Image data is null.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during image retrieval
                Console.WriteLine($"Error loading image '{imageName}': {ex.Message}");
                throw new InvalidOperationException("Image data unable to load > is null.");
            }
        }

        // Method to get a System.Drawing.Image
        public static Image GetImage(string imageName)
        {
            byte[] imageData = GetImageData(imageName);
            if (imageData != null)
            {
                using MemoryStream stream = new(imageData);
                return Image.FromStream(stream);
            }
            else
            {
                // Handle case where image data is null
                throw new InvalidOperationException("Image data is null.");
            }
        }

        // Method to get a BitmapImage
        public static BitmapImage GetBitmapImage(string imageName)
        {
            byte[] imageData = GetImageData(imageName);
            if (imageData != null)
            {
                BitmapImage bitmapImage = new();
                using (MemoryStream memory = new(imageData))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = memory;
                    bitmapImage.EndInit();
                }
                bitmapImage.Freeze(); // Freeze the image to prevent further modifications
                return bitmapImage;
            }
            else
            {
                // Handle case where image data is null
                throw new InvalidOperationException("Image data is null.");
            }
        }


        // Method to get an image based on the specified return type
        public static object GetImage(string imageName, Type returnType)
        {
            if (returnType == typeof(Image))
            {
                return GetImage(imageName);
            }
            else if (returnType == typeof(BitmapImage))
            {
                return GetBitmapImage(imageName);
            }
            else if (returnType == typeof(Icon))
            {
                return GetIcon(imageName);
            }
            else
            {
                throw new ArgumentException("Unsupported return type.");
            }
        }
        public static Icon GetIcon(string iconName)
        {
            byte[] iconData = iconName switch
            {
                "slug_icon" => Re_Source.slug_icon,
                // Add more cases for other icon names as needed
                _ => throw new ArgumentException($"Icon '{iconName}' not found."),
            };

            // Convert byte[] to Icon
            using MemoryStream ms = new(iconData);
            return new Icon(ms);
        }
    }
}
