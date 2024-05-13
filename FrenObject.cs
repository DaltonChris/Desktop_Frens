using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Image = System.Windows.Controls.Image;

namespace Desktop_Frens
{
    internal class FrenObject
    {
        readonly string _Name;
        readonly int _SpriteCount = 6;
        int _CurrentFrame = 0;
        readonly double MoveAmount = 8.5;
        bool _IsActive = false;
        bool MoveRight = true;
        int _Speed;
        readonly Dictionary<string, BitmapImage> _Images = [];
        readonly DispatcherTimer _Timer = new();
        readonly MainWindow _MainWindow;
        Image _AnimatedSource;

        public FrenObject(string name, int spriteCount, MainWindow mainWin, int speed ,Image image, int height, int width, int topOffset) 
        {
            _Name = name;
            _SpriteCount = spriteCount;
            _MainWindow = mainWin;
            _AnimatedSource = image;
            _Speed = speed;
            InitFren(height, width, topOffset);
            LoadImages();
        }

        public void SetActive()
        {
            _IsActive = true;
            if (_Timer != null)
            {
                _Timer.Interval = TimeSpan.FromMilliseconds(_Speed); // animation speed
                _Timer.Tick += TranslateFren;
                _Timer.Start();
            }
            
        }

        void InitFren(int height, int width, int topOffset)
        {
            _AnimatedSource.Height = height;
            _AnimatedSource.Width = width;
            Canvas.SetTop(_AnimatedSource, topOffset);
        }

        public void Disable()
        {
            _IsActive = false;
        }

        void LoadImages()
        {
            List<string> imageNames = new List<string>();
            for (int i = 1; i <= _SpriteCount; i++)
            {
                imageNames.Add($"{_Name}_{i}");
            }

            string[] imageNamesArray = imageNames.ToArray();

            foreach (var name in imageNamesArray)
            {
                _Images[name] = ImageManager.GetBitmapImage(name);
            }
        }

        void TranslateFren(object? sender, EventArgs e)
        {
            try
            {
                string resourceName = $"{_Name}_{_CurrentFrame + 1}";
                var imageSource = _Images[resourceName];
                _AnimatedSource.Source = imageSource;
                // Update the current frame index
                _CurrentFrame = (_CurrentFrame + 1) % _SpriteCount;
                // Get current position
                double currentX = Canvas.GetLeft(_AnimatedSource);
                // Update position based on direction
                if (MoveRight)
                {
                    currentX += MoveAmount;
                    if (currentX >= _MainWindow.Width)
                    {
                        currentX = _MainWindow.Width - _AnimatedSource.ActualWidth;
                        MoveRight = false; // Change direction
                        _AnimatedSource.RenderTransform = null; // flip 
                    }
                }
                else
                {
                    currentX -= MoveAmount;
                    if (currentX <= 10)
                    { // Adjust if needed
                        currentX = 0;
                        MoveRight = true; // Change direction // Reset the image flip
                        _AnimatedSource.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                        ScaleTransform flipTransform = new(-1, 1);
                        _AnimatedSource.RenderTransform = flipTransform;
                    }
                }
                // Apply new position
                Canvas.SetLeft(_AnimatedSource, currentX);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }

}
