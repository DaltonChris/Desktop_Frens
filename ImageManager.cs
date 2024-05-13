using System;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Desktop_Frens
{
    public class ImageManager
    {
        // Singleton instance
        private static ImageManager _instance;
        public static ImageManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ImageManager();
                return _instance;
            }
        }

        private ImageManager()
        {
            // No need to instantiate ResourceManager here, as it's handled by Re_Source
        }

        public byte[] GetImageData(string imageName)
        {
            try
            {
                byte[] imageData = null;

                // Get the image data from Re_Source using the imageName
                switch (imageName)
                {
                    case "Dog_1":
                        imageData = Re_Source.Dog_1;
                        break;
                    case "Dog_2":
                        imageData = Re_Source.Dog_2;
                        break;
                    case "Dog_3":
                        imageData = Re_Source.Dog_3;
                        break;
                    case "Dog_4":
                        imageData = Re_Source.Dog_4;
                        break;
                    case "Dog_5":
                        imageData = Re_Source.Dog_5;
                        break;
                    case "Dog_6":
                        imageData = Re_Source.Dog_6;
                        break;
                    case "Dog_7":
                        imageData = Re_Source.Dog_7;
                        break;
                    case "Slug_1":
                        imageData = Re_Source.Slug_1;
                        break;
                    case "Slug_2":
                        imageData = Re_Source.Slug_2;
                        break;
                    case "Slug_3":
                        imageData = Re_Source.Slug_3;
                        break;
                    case "Slug_4":
                        imageData = Re_Source.Slug_4;
                        break;
                    case "Slug_5":
                        imageData = Re_Source.Slug_3;
                        break;
                    case "Slug_6":
                        imageData = Re_Source.Slug_2;
                        break;
                    default:
                        throw new ArgumentException($"Image '{imageName}' not found.");
                }


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
                return null;
            }
        }

        public Image GetSprite(string imageName)
        {
            try
            {
                byte[] imageData = null;

                // Get the image data from Re_Source using the imageName
                switch (imageName)
                {
                    case "Dog_1":
                        imageData = Re_Source.Dog_1;
                        break;
                    case "Dog_2":
                        imageData = Re_Source.Dog_2;
                        break;
                    case "Dog_3":
                        imageData = Re_Source.Dog_3;
                        break;
                    case "Dog_4":
                        imageData = Re_Source.Dog_4;
                        break;
                    case "Dog_5":
                        imageData = Re_Source.Dog_5;
                        break;
                    case "Dog_6":
                        imageData = Re_Source.Dog_6;
                        break;
                    case "Dog_7":
                        imageData = Re_Source.Dog_7;
                        break;
                    case "Slug_1":
                        imageData = Re_Source.Slug_1;
                        break;
                    case "Slug_2":
                        imageData = Re_Source.Slug_2;
                        break;
                    case "Slug_3":
                        imageData = Re_Source.Slug_3;
                        break;
                    case "Slug_4":
                        imageData = Re_Source.Slug_4;
                        break;
                    default:
                        throw new ArgumentException($"Image '{imageName}' not found.");
                }

                // Convert byte array to Image
                if (imageData != null)
                {
                    using (var stream = new System.IO.MemoryStream(imageData))
                    {
                        return System.Drawing.Image.FromStream(stream);
                    }
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
                return null;
            }
        }

        public BitmapImage LoadImage(string resourceName)
        {
            try
            {
                byte[] imageData = GetImageData(resourceName);
                if (imageData != null)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    using (MemoryStream memory = new MemoryStream(imageData))
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
                    System.Windows.MessageBox.Show("Image data is null.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading image: {ex.Message}");
                return null;
            }
        }

        public Icon GetIcon(string iconName)
        {
            byte[] iconData = null;

            switch (iconName)
            {
                case "slug_icon":
                    iconData = Re_Source.slug_icon;
                    break;
                // Add more cases for other icon names as needed
                default:
                    throw new ArgumentException($"Icon '{iconName}' not found.");
            }

            // Convert byte[] to Icon
            using (MemoryStream ms = new MemoryStream(iconData))
            {
                return new Icon(ms);
            }
        }
        public object GetImage(string imageName, Type returnType)
        {
            if (returnType == typeof(System.Drawing.Image))
            {
                return GetSprite(imageName);
            }
            else if (returnType == typeof(System.Windows.Controls.Image))
            {
                // Load the image using BitmapImage
                BitmapImage bitmapImage = (BitmapImage)LoadImage(imageName);

                // Create a new instance of System.Windows.Controls.Image
                System.Windows.Controls.Image wpfImage = new System.Windows.Controls.Image();
                wpfImage.Source = bitmapImage;
                return wpfImage;
            }
            else
            {
                throw new ArgumentException("Unsupported return type.");
            }
        }

    }
}
