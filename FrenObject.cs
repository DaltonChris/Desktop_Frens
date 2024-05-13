using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Image = System.Windows.Controls.Image;

/* ############################################
 * ### Dalton Christopher                   ###
 * ### Desktop-Frens - Windows - .NET8.0    ###
 * ### 05/2024                              ###
 * ############################################*/
namespace Desktop_Frens
{
    public class FrenObject
    {
        readonly string _Name;
        readonly int _SpriteCount = 6;
        int _CurrentFrame = 0;
        readonly double _MoveSpeed = 7.5;
        bool _IsActive = false;
        bool MoveRight = false;
        bool IsHalted = false;
        int _AnimationSpeed;
        readonly Dictionary<string, BitmapImage> _Images = [];
        readonly DispatcherTimer _Timer = new();
        readonly MainWindow _MainWindow;
        readonly Image _AnimatedSource;

        public FrenObject(string name, int spriteCount, MainWindow mainWin,double moveSpeed,int animSpeed ,Image image, int height, int width, int topOffset) 
        {
            _Name = name;
            _SpriteCount = spriteCount;
            _MainWindow = mainWin;
            _AnimatedSource = image;
            _AnimationSpeed = animSpeed;
            _MoveSpeed = moveSpeed;
            InitFren(height, width, topOffset);
            LoadImages();
        }

        public void SetActive()
        {
            _IsActive = true;
            if (_Timer != null)
            {
                _Timer.Interval = TimeSpan.FromMilliseconds(_AnimationSpeed); // animation speed
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
            _AnimatedSource.Source = null;
        }

        void LoadImages()
        {
            List<string> imageNames = [];
            for (int i = 1; i <= _SpriteCount; i++)
            {
                imageNames.Add($"{_Name}_{i}");
            }

            string[] imageNamesArray = [.. imageNames];

            foreach (var name in imageNamesArray)
            {
                _Images[name] = ImageManager.GetBitmapImage(name);
            }
        }
        public bool IsActive()
        {
            return _IsActive;
        }

        async void TranslateFren(object? sender, EventArgs e)
        {
            try
            {
                if (_IsActive)
                {
                    var haltChance = new Random().Next(0, 550);
                    var FlipChance = new Random().Next(0, 750);
                    if(FlipChance == 0)
                    {
                        ScaleTransform currentTransform = (ScaleTransform)_AnimatedSource.RenderTransform;
                        ScaleTransform scaleTransform = new(1, 1);
                        if (currentTransform == scaleTransform)
                        {
                            MoveRight = true; // Change direction
                            ScaleTransform flipTransform = new(-1, 1);
                            _AnimatedSource.RenderTransform = flipTransform;
                        }
                        else
                        {
                            MoveRight = false; // Change direction
                            _AnimatedSource.RenderTransform = null;
                        }
                    }
                    
                    if (haltChance == 0)
                    {
                        IsHalted = true;
                        // Slow anim rate multiplier
                        double animationInterval = _AnimationSpeed * 10;
                        _Timer.Interval = TimeSpan.FromMilliseconds(animationInterval); // animation speed
                        await Task.Delay(new Random().Next(2000, 5000));
                        IsHalted = false;
                        _Timer.Interval = TimeSpan.FromMilliseconds(_AnimationSpeed); // animation speed
                    }

                    string resourceName = $"{_Name}_{_CurrentFrame + 1}";
                    var imageSource = _Images[resourceName];
                    _AnimatedSource.Source = imageSource;
                    // Update the current frame index
                    _CurrentFrame = (_CurrentFrame + 1) % _SpriteCount;

                    // Get current position
                    double currentX = Canvas.GetLeft(_AnimatedSource);
                    // Update position based on direction
                    if (MoveRight && !IsHalted)
                    {
                         currentX += _MoveSpeed; // Move Postition (positive)
                        if (currentX >= _MainWindow.Width + 125)
                        {
                            //currentX = _MainWindow.Width - _AnimatedSource.ActualWidth; // Move position
                            MoveRight = false; // Change direction
                            _AnimatedSource.RenderTransform = null; // flip 
                        }
                    }
                    else if (!IsHalted)
                    {
                        currentX -= _MoveSpeed; // Move position (Negative)
                        if (currentX <= -50)
                        { // Adjust if needed
                            //currentX = 0;
                            MoveRight = true; // Change direction // Reset the image flip
                            _AnimatedSource.RenderTransformOrigin = new System.Windows.Point(0, 0);
                            ScaleTransform flipTransform = new(-1, 1);
                            _AnimatedSource.RenderTransform = flipTransform;
                        }
                    }
                    // Apply new position
                    Canvas.SetLeft(_AnimatedSource, currentX);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }

}
