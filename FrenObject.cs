﻿using System.Diagnostics;
using System.Windows;
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
    public enum FrenType
    {
        Slug,
        Spooky,
        Dog,
        Frog,
        Frog_B,
    }

    public class FrenObject
    {
        readonly string _Name; // frens name
        readonly int _SpriteCount = 6; // Amount of sprites in anim set
        int _CurrentFrame = 0; // active image frame
        double _MoveSpeed = 7.5; // move amount per tick
        readonly double _DefaultMove; // Dupe to keep rate
        bool _IsActive = false; // flag
        bool _MoveRight = true; // direction flag
        bool _IsHalted = false; // halt flag
        bool _IsRun = false; // Run flag
        int _FlipOffset;
        readonly int _AnimationSpeed; // anim rate (ms) tick time
        readonly Dictionary<string, BitmapImage> _Images = []; // Image dict
        readonly DispatcherTimer _Timer; // timer
        readonly MainWindow _MainWindow;
        readonly Image _AnimatedSource; // Canvas image source
        readonly ScaleTransform _ScaleDefault = new(1, 1);
        readonly ScaleTransform _ScaleFlip = new(-1, 1);
        double _TopOffset;
        double _TopOffsetAdjust;
        FrenType _Type;

        //Constructor
        public FrenObject(string name, int spriteCount, MainWindow mainWin, double moveSpeed, int animSpeed, Image image, int height, int width, int topOffset)
        {
            _Name = name;
            _SpriteCount = spriteCount;
            _MainWindow = mainWin;
            _AnimatedSource = image;
            _AnimationSpeed = animSpeed;
            _MoveSpeed = moveSpeed;
            _DefaultMove = _MoveSpeed;
            _Timer = new DispatcherTimer();
            InitFren(height, width, topOffset);
            LoadImages();
        }
        // Public Active getter
        public bool IsActive()
        {
            return _IsActive;
        }
        // Disable fren method
        public void Disable()
        {
            _IsActive = false; // set flag
            _AnimatedSource.Visibility = Visibility.Collapsed; // disable image
            _Timer.Tick -= UpdateFren;
            _Timer.Interval = TimeSpan.Zero;
        }
        public void SetActive()
        {
            if (_Timer != null)
            {
                _IsActive = true; // Set flag
                _AnimatedSource.Visibility = Visibility.Visible; // show image
                _Timer.Interval = TimeSpan.FromMilliseconds(_AnimationSpeed);
                _Timer.Tick += UpdateFren; // Call Translate fren each tick interval
                _Timer.Start(); // start timer
            }

        }
        // Init Fren objects image on cavas L/W/H
        void InitFren(int height, int width, int topOffset)
        {
            _AnimatedSource.Height = height;
            _AnimatedSource.Width = width;
            Canvas.SetTop(_AnimatedSource, topOffset);
            _AnimatedSource.RenderTransform = _ScaleFlip; // Flip image using scale
            _TopOffset = Canvas.GetTop(_AnimatedSource);
            _TopOffsetAdjust = _TopOffset - 10;
        }
        void LoadImages() // Load the images for fren type
        {
            List<string> imageNames = []; // List of image names
            for (int i = 1; i <= _SpriteCount; i++){
                if (_Name == "Frog_B" || _Name == "Frog" || _Name == "Spooky" 
                                      || _Name == "Dog" || _Name == "Frog_B")
                {
                    if (_Name == "Dog")
                    {
                        imageNames.Add($"{_Name}_{i}"); // Add each name
                        imageNames.Add($"{_Name}_Run_{i}"); // Add Run name
                        imageNames.Add($"{_Name}_Idle_{i}"); // Idles
                    }
                    else
                    {
                        imageNames.Add($"{_Name}_Idle_{i}"); // Idles
                        imageNames.Add($"{_Name}_{i}"); // Add each name
                    }
                }
                else imageNames.Add($"{_Name}_{i}"); // Add each name
            }
            // To array
            string[] imageNamesArray = [.. imageNames];

            foreach (var name in imageNamesArray)
            {   // use image manager to ge tthe bitmap Images
                _Images[name] = (BitmapImage)ImageManager.GetImage(name, typeof(BitmapImage));
            }
        }

        public void PublicFlip()
        {
            FlipFren();
        }

        void FlipFren()
        {
            ScaleTransform? currentTransform = (ScaleTransform)_AnimatedSource.RenderTransform;
            if(currentTransform == null) return;

            if (currentTransform == _ScaleDefault) // If the current scale is the default (1, 1)
            {
                _MoveRight = true; // Set the direction to move right
                _AnimatedSource.RenderTransform = _ScaleFlip;
            }
            else 
            {
                _MoveRight = false; // Set the direction to move left
                _AnimatedSource.RenderTransform = _ScaleDefault;
            }
        }

        void UpdateFren(object? sender, EventArgs e)
        {
            try
            {
                if (_IsActive) // If Fren is set active
                {
                    int FlipChance;
                    int runChance = new Random().Next(0, 225);
                    int haltChance = new Random().Next(0, 215);
                    if (!_IsHalted) { 
                        FlipChance = new Random().Next(0, 310);
                        if (FlipChance == 0) FlipFren();
                    }
                    // If rolls a 0
                    if (runChance == 0) RunFren();
                    if (haltChance == 0) HaltFren();// If random halt chance = 0

                    // Hault | Run/Idle Alt cycles
                    if ((_Name == "Spooky" || _Name == "Frog_B" || _Name == "Frog" || _Name == "Dog") 
                        && _IsHalted) IdleFrenFrames();

                    else if (_Name == "Dog" && _IsRun) RunUpdateFrenFrame(); // Run
                    else UpdateFrenFrame(); // Normal

                    // (If is Frog and between last frame or 1-3 : Speed up To simulate A hop
                    if ((_Name == "Frog" || _Name == "Frog_B") && (_CurrentFrame == 7 || _CurrentFrame <= 2))
                        _MoveSpeed = _DefaultMove * 40; // Speed * 7~ 
                    else if (_Name == "Dog" && _IsRun) _MoveSpeed = _DefaultMove * 3;
                    else
                        _MoveSpeed = _DefaultMove; // Normal Move speed

                    // Move the Fren
                    if(!_IsHalted)
                    {
                        double currentX = Translate();
                        Canvas.SetLeft(_AnimatedSource, currentX);// Apply new position
                    }

                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: UPDATE FREN :{ex.Message} {ex}");
            }
        }



        async void RunFren()
        {
            _IsRun = true;
            _MoveSpeed = _DefaultMove * 20; // Set the faster speed for running
            _CurrentFrame = 0; // Reset the frame index
            await Task.Delay(new Random().Next(2500, 7500)); // Delay range for running
            _IsRun = false;
            _MoveSpeed = _DefaultMove; // Revert to the original move speed
        }
        async void HaltFren()
        {
            _IsHalted = true; // Halted flag
            _CurrentFrame = 0; // Reset the frame index
            if (_Name == "Slug") {
                double animationInterval = _AnimationSpeed * 10; // Slow anim at halt
                _Timer.Interval = TimeSpan.FromMilliseconds(animationInterval); // animation speed
            }
            await Task.Delay(new Random().Next(3500, 12750)); // Delay range
            _IsHalted = false; // Reset flag
            _Timer.Interval = TimeSpan.FromMilliseconds(_AnimationSpeed); // animation speed
        }

        void RunUpdateFrenFrame()
        {
            Canvas.SetTop(_AnimatedSource, _TopOffset);
            string runName = $"{_Name}_Run_{_CurrentFrame + 1}"; // Get image by name and fram
            var imageSource_ = _Images[runName]; // Retieve from array
            _AnimatedSource.Source = imageSource_; // update image
            _CurrentFrame = (_CurrentFrame + 1) % _SpriteCount; // Update current frame index
        }
        void IdleFrenFrames()
        {
            string haltName = $"{_Name}_Idle_{_CurrentFrame + 1}"; // Get image by name and fram
            var imageSource_ = _Images[haltName]; // Retieve from array
            _AnimatedSource.Source = imageSource_; // update image
            if (_Name == "Dog" && _TopOffset != _TopOffsetAdjust)
            {
                Canvas.SetTop(_AnimatedSource, _TopOffsetAdjust);
            }

            _CurrentFrame = (_CurrentFrame + 1) % _SpriteCount; // Update current frame index
        }
        void UpdateFrenFrame()
        {
            Canvas.SetTop(_AnimatedSource, _TopOffset);
            string resourceName = $"{_Name}_{_CurrentFrame + 1}"; // Get image by name and fram
            var imageSource = _Images[resourceName]; // Retieve from array
            _AnimatedSource.Source = imageSource; // update image
            _CurrentFrame = (_CurrentFrame + 1) % _SpriteCount; // Update current frame index
        }

        #region Move Fren (Translate)
        double Translate()
        {                    // Get current position
            double currentX = Canvas.GetLeft(_AnimatedSource);
            if (_MoveRight && !_IsHalted) currentX = MoveFrenRight(currentX);
            else if (!_IsHalted) currentX = MoveFrenLeft(currentX); // else if not halted
            return currentX;
        }
        double MoveFrenRight(double currentX)
        {
            currentX += _MoveSpeed; // Move Postition (positive)
            if (currentX >= _MainWindow.Width + 55)
            {
                _MoveRight = false; // Change direction
                _AnimatedSource.RenderTransform = null; // flip 
            }
            return currentX;
        }
        double MoveFrenLeft(double currentX)
        {
            currentX -= _MoveSpeed; // Move position (Negative)
            if (currentX <= -50)
            {
                _MoveRight = true; // Change direction // Reset the image flip
                _AnimatedSource.RenderTransformOrigin = new System.Windows.Point(0, 0); // Set point
                _AnimatedSource.RenderTransform = _ScaleFlip; // Flip image using scale
            }
            return currentX;
        }
        #endregion
    }
}

