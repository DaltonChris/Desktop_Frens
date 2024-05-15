using System.Diagnostics;
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
        bool MoveRight = true; // direction flag
        bool IsHalted = false; // halt flag
        bool IsRun = false; // Run flag
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
            ScaleTransform flipTransform = new(-1, 1); // Reverse scale (x)
            _AnimatedSource.RenderTransform = flipTransform; // Flip image using scale
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
                for (int i = 1; i <= 8; i++)
                {
                    imageNames.Add($"{_Name}_Run_{i}"); // Add each name
                }
            }
            else if (_Name == "Spooky")
            {
                for (int i = 1; i <= _SpriteCount; i++)
                {
                    imageNames.Add($"{_Name}_Idle_{i}"); // Add each name
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
        void FlipFren()
        {
            // set scale to opposite
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
        void TranslateFren(object? sender, EventArgs e)
        {
            try
            {
                if (_IsActive) // If Fren is set active
                {
                    var runChance = new Random().Next(0, 225);
                    var haltChance = new Random().Next(0, 125);
                    var FlipChance = new Random().Next(0, 350);
                    
                    // If rolls a 0
                    if (runChance == 0) RunFren();
                    if (FlipChance == 0) FlipFren();
                    if (haltChance == 0) HaltFren();// If random halt chance = 0

                    // Hault | Run/Idle Alt cycles
                    if (_Name == "Spooky" && IsHalted) HaltedUpdateIdleFrenFrame();
                    else if (_Name == "Dog" && IsHalted) HaltedUpdateFrenFrame();
                    else if (_Name == "Dog" && IsRun) RunUpdateFrenFrame(); // Run
                    else UpdateFrenFrame(); // Normal

                    var frogMulti = new Random().Next(0, 5);
                    // (If is Frog and between last frame or 1-3 : Speed up To simulate A hop
                    if ((_Name == "Frog" || _Name == "Frog_B") && (_CurrentFrame == 7 || _CurrentFrame <= 2))
                        if(frogMulti == 0) _MoveSpeed = _DefaultMove * 6; // Speed * 7~ 
                        else _MoveSpeed = _DefaultMove * frogMulti + 2; // Speed * 4~ 
                    else if (_Name == "Dog" && IsRun) _MoveSpeed = _DefaultMove * 3;
                    else
                        _MoveSpeed = _DefaultMove; // Normal Move speed

                    // Move the Fren
                    double currentX = Translate();
                    Canvas.SetLeft(_AnimatedSource, currentX);// Apply new position
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}");
            }
        }

        double Translate()
        {                    // Get current position
            double currentX = Canvas.GetLeft(_AnimatedSource);
            if (MoveRight && !IsHalted) currentX = MoveFrenRight(currentX);
            else if (!IsHalted) currentX = MoveFrenLeft(currentX); // else if not halted
            return currentX;
        }

        async void RunFren()
        {
            IsRun = true;
            _MoveSpeed = _DefaultMove * 20; // Set the faster speed for running
            _CurrentFrame = 0; // Reset the frame index
            await Task.Delay(new Random().Next(2500, 7500)); // Delay range for running
            IsRun = false;
            _MoveSpeed = _DefaultMove; // Revert to the original move speed
        }


        async void HaltFren()
        {
            IsHalted = true; // Halted flag
            _CurrentFrame = 0; // Reset the frame index
            if (_Name != "Dog" && _Name != "Spooky"){
                double animationInterval = _AnimationSpeed * 10; // Slow anim at halt
                _Timer.Interval = TimeSpan.FromMilliseconds(animationInterval); // animation speed
            }
            // Delay / Wait
            await Task.Delay(new Random().Next(3500, 8750)); // Delay range
            IsHalted = false; // Reset flag
            _Timer.Interval = TimeSpan.FromMilliseconds(_AnimationSpeed); // animation speed
        }

        void RunUpdateFrenFrame()
        {
            string runName = $"{_Name}_Run_{_CurrentFrame + 1}"; // Get image by name and fram
            var imageSource_ = _Images[runName]; // Retieve from array
            _AnimatedSource.Source = imageSource_; // update image
            _CurrentFrame = (_CurrentFrame + 1) % _SpriteCount; // Update current frame index
        }

        void HaltedUpdateFrenFrame()
        {
            string haltName = $"{_Name}_Sit_{_CurrentFrame + 1}"; // Get image by name and fram
            var imageSource_ = _Images[haltName]; // Retieve from array
            _AnimatedSource.Source = imageSource_; // update image
            _CurrentFrame = (_CurrentFrame + 1) % (_SpriteCount - 1); // Update current frame index
        }
        void HaltedUpdateIdleFrenFrame()
        {
            string haltName = $"{_Name}_Idle_{_CurrentFrame + 1}"; // Get image by name and fram
            var imageSource_ = _Images[haltName]; // Retieve from array
            _AnimatedSource.Source = imageSource_; // update image
            _CurrentFrame = (_CurrentFrame + 1) % _SpriteCount; // Update current frame index
        }
        void UpdateFrenFrame()
        {
            string resourceName = $"{_Name}_{_CurrentFrame + 1}"; // Get image by name and fram
            var imageSource = _Images[resourceName]; // Retieve from array
            _AnimatedSource.Source = imageSource; // update image
            _CurrentFrame = (_CurrentFrame + 1) % _SpriteCount; // Update current frame index
        }

        double MoveFrenRight(double currentX)
        {
            currentX += _MoveSpeed; // Move Postition (positive)
            if (currentX >= _MainWindow.Width + 150)
            {
                //currentX = _MainWindow.Width - _AnimatedSource.ActualWidth; // Move position
                MoveRight = false; // Change direction
                _AnimatedSource.RenderTransform = null; // flip 
            }
            return currentX;
        }

        double MoveFrenLeft(double currentX)
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
            return currentX;
        }
    }

}
