using System.IO;
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
        public static ImageManager Instance
        {
            get // Single boi brrt
            {
                _instance ??= new ImageManager();
                return _instance;
            }
        }

        private ImageManager()
        {
            // No need to instantiate:  handled by Re_Source
        }

        /// <summary>
        /// Loads Bitmaps from .Resx
        /// </summary>
        /// <param name="imageName"> Name of Resource image to load </param>
        /// <returns> Bitmap of the image </returns>
        /// <exception cref="ArgumentException"> Not ideal </exception>
        /// <exception cref="InvalidOperationException"> Not ideal 2 </exception>
        public static Bitmap GetImageData(string imageName)
        {
            try
            {
                Bitmap imageData;

                // Get the image data from Re_Source using the imageName
                imageData = imageName switch
                {
                    #region Dog
                    "Dog_1" => Re_Source.Dog_1,
                    "Dog_2" => Re_Source.Dog_2,
                    "Dog_3" => Re_Source.Dog_3,
                    "Dog_4" => Re_Source.Dog_4,
                    "Dog_5" => Re_Source.Dog_5,
                    "Dog_6" => Re_Source.Dog_6,
                    "Dog_7" => Re_Source.Dog_7,
                    "Dog_Sit_1" => Re_Source.Dog_Sit_1,
                    "Dog_Sit_2" => Re_Source.Dog_Sit_2,
                    "Dog_Sit_3" => Re_Source.Dog_Sit_3,
                    "Dog_Sit_4" => Re_Source.Dog_Sit_4,
                    "Dog_Sit_5" => Re_Source.Dog_Sit_5,
                    "Dog_Sit_6" => Re_Source.Dog_Sit_6,
                    "Dog_Run_1" => Re_Source.Dog_Run_1,
                    "Dog_Run_2" => Re_Source.Dog_Run_2,
                    "Dog_Run_3" => Re_Source.Dog_Run_3,
                    "Dog_Run_4" => Re_Source.Dog_Run_4,
                    "Dog_Run_5" => Re_Source.Dog_Run_5,
                    "Dog_Run_6" => Re_Source.Dog_Run_6,
                    "Dog_Run_7" => Re_Source.Dog_Run_7,
                    "Dog_Run_8" => Re_Source.Dog_Run_8,
                    #endregion
                    #region Slug
                    "Slug_1" => Re_Source.Slug_1,
                    "Slug_2" => Re_Source.Slug_2,
                    "Slug_3" => Re_Source.Slug_3,
                    "Slug_4" => Re_Source.Slug_4,
                    "Slug_5" => Re_Source.Slug_3,
                    "Slug_6" => Re_Source.Slug_2,
                    #endregion
                    #region Spooky
                    "Spooky_1" => Re_Source.Spooky_1,
                    "Spooky_2" => Re_Source.Spooky_2,
                    "Spooky_3" => Re_Source.Spooky_3,
                    "Spooky_4" => Re_Source.Spooky_4,
                    "Spooky_5" => Re_Source.Spooky_5,
                    "Spooky_6" => Re_Source.Spooky_6,
                    "Spooky_7" => Re_Source.Spooky_7,
                    "Spooky_8" => Re_Source.Spooky_8,
                    #endregion
                    #region Frog
                    "Frog_1" => Re_Source.Frog_1,
                    "Frog_2" => Re_Source.Frog_2,
                    "Frog_3" => Re_Source.Frog_3,
                    "Frog_4" => Re_Source.Frog_4,
                    "Frog_5" => Re_Source.Frog_5,
                    "Frog_6" => Re_Source.Frog_6,
                    "Frog_7" => Re_Source.Frog_7,
                    "Frog_B_1" => Re_Source.Frog_B_1,
                    "Frog_B_2" => Re_Source.Frog_B_2,
                    "Frog_B_3" => Re_Source.Frog_B_3,
                    "Frog_B_4" => Re_Source.Frog_B_4,
                    "Frog_B_5" => Re_Source.Frog_B_5,
                    "Frog_B_6" => Re_Source.Frog_B_6,
                    "Frog_B_7" => Re_Source.Frog_B_7,

                    #endregion
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
            Bitmap imageData = GetImageData(imageName);
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
        public static BitmapImage GetBitmapImage(string imageName)
        {
            Bitmap imageData = GetImageData(imageName);
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
        
        /// <summary>
        /// Method to Get Icons form Resource .resx
        /// </summary>
        /// <param name="iconName"> Icon to get </param>
        /// <returns> The icon what u think </returns>
        /// <exception cref="ArgumentException"> Shes fooked </exception>
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
