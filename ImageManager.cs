using Desktop_Frens.Properties;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
/* ############################################
 * ### Dalton Christopher                   ###
 * ### Desktop-Frens - Windows - .NET8.0    ###
 * ### 05/2024                              ###
 * ############################################*/
namespace Desktop_Frens
{
    public class ImageManager // Manages Images innit bruv
    {
        // Singleton instance
        private static ImageManager? _instance;
        public static ImageManager Instance{
            get{ // Single boi brrt
                _instance ??= new ImageManager();
                return _instance;
            }
        }
        private ImageManager()
        {
            // No need to instantiate:  handled by Re_Source
        }

        // Method to get an image based on the specified return type
        public static object GetImage(string imageName, Type returnType)
        {
            if (returnType == typeof(Image))
            {
                return GetImg(imageName);
            }
            else if (returnType == typeof(BitmapImage))
            {
                return GetImgBitmap(imageName);
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

        // 
        private static Bitmap GetImgData(string imageName)
        {
            // Get the Type object of the Re_Source class
            Type type = typeof(Re_Source);

            // Get the PropertyInfo obj from Re_Source."NAME"
            PropertyInfo? propertyInfo = type.GetProperty(imageName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            if (propertyInfo != null)
            {
                // Get the bitmap 
                Bitmap? imageData = (Bitmap?)propertyInfo.GetValue(null, null);
                if (imageData != null)
                {
                    return imageData;
                }
            }
            // Shes cooked
            Console.WriteLine($"Property '{imageName}' not found in class '{type.FullName}'.");
            throw new InvalidOperationException($"Image data is null or property not found. Property '{imageName}' not found in class '{type.FullName}'.");
        }

        // Method to get a System.Drawing.Image
        private static Image GetImg(string imageName)
        {
            Bitmap imageData = GetImgData(imageName);
            if (imageData != null)
            {
                using MemoryStream stream = new();
                imageData.Save(stream, System.Drawing.Imaging.ImageFormat.Png); // Save the bitmap to the memory stream as PNG
                stream.Seek(0, SeekOrigin.Begin); // Reset the stream position to the beginning
                return Image.FromStream(stream); // Create an Image from the memory stream
            }
            else
            {
                // Handle case where image data is null
                throw new InvalidOperationException("Image data is null.");
            }
        }


        // Method to get a BitmapImage
        private static BitmapImage GetImgBitmap(string imageName)
        {
            Bitmap imageData = GetImgData(imageName);
            if (imageData != null)
            {
                // Convert Bitmap to byte array
                MemoryStream memoryStream = new();
                imageData.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageBytes = memoryStream.ToArray();

                // Create BitmapImage from byte array
                BitmapImage bitmapImage = new();
                using (MemoryStream stream = new(imageBytes))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
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

        // Gets the Icon
        private static Icon GetIcon(string iconName)
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
