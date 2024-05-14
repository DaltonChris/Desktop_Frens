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
        readonly string _Name; // frens name
        readonly int _SpriteCount = 6; // Amount of sprites in anim set
        int _CurrentFrame = 0; // active image frame
        double _MoveSpeed = 7.5; // move amount per tick
        readonly double _DefaultMove; // Dupe to keep rate
        bool _IsActive = false; // flag
        bool MoveRight = false; // direction flag
        bool IsHalted = false; // halt flag
        int _AnimationSpeed; // anim rate (ms) tick time
        readonly Dictionary<string, BitmapImage> _Images = []; // Image dict
        readonly DispatcherTimer _Timer = new(); // timer
        readonly MainWindow _MainWindow; 
        readonly Image _AnimatedSource; // Canvas image source

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spriteCount"></param>
        /// <param name="mainWin"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="animSpeed"></param>
        /// <param name="image"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="topOffset"></param>
        public FrenObject(string name, int spriteCount, MainWindow mainWin,double moveSpeed,int animSpeed ,Image image, int height, int width, int topOffset) 
        {
            _Name = name;
            _SpriteCount = spriteCount;
            _MainWindow = mainWin;
            _AnimatedSource = image;
            _AnimationSpeed = animSpeed;
            _MoveSpeed = moveSpeed;
            InitFren(height, width, topOffset);
            _DefaultMove = _MoveSpeed;
            LoadImages();
        }

        public void SetActive()
        {
            _IsActive = true; // Set flag
            if (_Timer != null)
            {
                // animation speed (interval of calls) (movealso)
                _Timer.Interval = TimeSpan.FromMilliseconds(_AnimationSpeed);
                _Timer.Tick += TranslateFren; // Call Translate fren each tick interval
                _Timer.Start(); // start timer
            }
            
        }

        // Init Fren objects image on cavas L/W/H
        void InitFren(int height, int width, int topOffset)
        {
            _AnimatedSource.Height = height;
            _AnimatedSource.Width = width;
            Canvas.SetTop(_AnimatedSource, topOffset);
        }

        // Disable fren method
        public void Disable() 
        {
            _IsActive = false; // set flag
            _AnimatedSource.Source = null; // Null image
        }

        void LoadImages() // Load the images for fren type
        {
            List<string> imageNames = []; // List of image names
            for (int i = 1; i <= _SpriteCount; i++)
            {
                imageNames.Add($"{_Name}_{i}"); // Add each name
            }
            if(_Name == "Dog")
            {
                for (int i = 1; i <= 6; i++)
                {
                    imageNames.Add($"{_Name}_Sit_{i}"); // Add each name
                }
            }
            // To array
            string[] imageNamesArray = [.. imageNames];

            foreach (var name in imageNamesArray)
            {   // use image manager to ge tthe bitmap Images
                _Images[name] = ImageManager.GetBitmapImage(name);
            }
        }
        // Public Active getter
        public bool IsActive()
        {
            return _IsActive;
        }

        async void TranslateFren(object? sender, EventArgs e)
        {
            try
            {
                if (_IsActive) // If Fren is set active
                {
                    var haltChance = new Random().Next(0, 550);
                    var FlipChance = new Random().Next(0, 450);

                    if(FlipChance == 0) // If rolls a 0
                    {
                        // set scale to opposite
                        ScaleTransform currentTransform = (ScaleTransform)_AnimatedSource.RenderTransform;
                        ScaleTransform scaleTransform = new(1, 1);
                        if (currentTransform == scaleTransform){
                            MoveRight = true; // Change direction
                            ScaleTransform flipTransform = new(-1, 1);
                            _AnimatedSource.RenderTransform = flipTransform;
                        }
                        else{
                            MoveRight = false; // Change direction
                            _AnimatedSource.RenderTransform = null;
                        }
                    }

                    if (haltChance == 0) // If random halt chance = 0
                    {
                        IsHalted = true; // Halted flag
                        // Slow anim rate multiplier

                        if(_Name != "Dog")
                        {
                            double animationInterval = _AnimationSpeed * 10; // Slow anim at halt
                            _Timer.Interval = TimeSpan.FromMilliseconds(animationInterval); // animation speed
                        }
                        // Delay / Wait
                        await Task.Delay(new Random().Next(3500, 6200)); // Delay range
                        IsHalted = false; // Reset flag
                        _Timer.Interval = TimeSpan.FromMilliseconds(_AnimationSpeed); // animation speed
                    }

                    if (_Name == "Dog" && IsHalted)
                    {
                        string haltName = $"{_Name}_Sit_{_CurrentFrame + 1}"; // Get image by name and fram
                        var imageSource_ = _Images[haltName]; // Retieve from array
                        _AnimatedSource.Source = imageSource_; // update image
                        _CurrentFrame = (_CurrentFrame + 1) % (_SpriteCount - 1); // Update current frame index
                    }
                    else
                    {
                        string resourceName = $"{_Name}_{_CurrentFrame + 1}"; // Get image by name and fram
                        var imageSource = _Images[resourceName]; // Retieve from array
                        _AnimatedSource.Source = imageSource; // update image
                        _CurrentFrame = (_CurrentFrame + 1) % _SpriteCount; // Update current frame index
                    }
                    

                    // (If is Frog and between 7,1,2,3 Speed up To simulate A hop
                    if (_Name == "Frog" && (_CurrentFrame == 7 || _CurrentFrame <= 3))
                        _MoveSpeed = _DefaultMove * 4; // Speed * 4~ 
                    else
                        _MoveSpeed = _DefaultMove; // Normal Move speed

                    // Get current position
                    double currentX = Canvas.GetLeft(_AnimatedSource);

                    // If is moving right and not halted
                    if (MoveRight && !IsHalted)// Update position based on direction
                    {
                         currentX += _MoveSpeed; // Move Postition (positive)
                        if (currentX >= _MainWindow.Width + 125)
                        {
                            //currentX = _MainWindow.Width - _AnimatedSource.ActualWidth; // Move position
                            MoveRight = false; // Change direction
                            _AnimatedSource.RenderTransform = null; // flip 
                        }
                    }
                    else if (!IsHalted) // else if not halted
                    {
                        currentX -= _MoveSpeed; // Move position (Negative)
                        if (currentX <= -50)
                        { // Adjust if needed
                            //currentX = 0;
                            MoveRight = true; // Change direction // Reset the image flip
                            _AnimatedSource.RenderTransformOrigin = new System.Windows.Point(0, 0); // Set point
                            ScaleTransform flipTransform = new(-1, 1); // Reverse scale (x)
                            _AnimatedSource.RenderTransform = flipTransform; // Flip image using scale
                        }
                    }
                    Canvas.SetLeft(_AnimatedSource, currentX);// Apply new position
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }

}
