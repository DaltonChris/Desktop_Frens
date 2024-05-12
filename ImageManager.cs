using System;
using System.Drawing;
using System.Resources;

namespace Desktop_Frens
{
    public class ImageManager
    {
        private readonly ResourceManager _resourceManager;

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
            _resourceManager = new ResourceManager("Desktop_Frens.Re_Source", typeof(ImageManager).Assembly);
        }

        public Bitmap GetImage(string imageName)
        {
            try
            {
                object obj = _resourceManager.GetObject(imageName);
                if (obj is Bitmap bitmap)
                {
                    return bitmap;
                }
                else
                {
                    // Handle the case where the object is not a Bitmap
                    throw new ArgumentException($"The resource '{imageName}' is not a Bitmap.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during image retrieval
                Console.WriteLine($"Error loading image '{imageName}': {ex.Message}");
                return null;
            }
        }

        public Icon GetIcon(string iconName)
        {
            return (Icon)_resourceManager.GetObject(iconName);
        }
    }
}
