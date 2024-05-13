using System.CodeDom;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;
//using Microsoft.Diagnostics.Tracing.AutomatedAnalysis;

namespace Desktop_Frens
{
    public partial class MainWindow : Window
    {
        readonly DispatcherTimer timer = new();
        int CurrentFrame = 0;
        readonly double MoveAmount = 8.5;
        bool MoveRight = true;
        readonly NotifyIcon TaskIcon = new();

        readonly int SlugSpriteCount = 6;
        readonly int DogSpriteCount = 6;
        int CurrentSpriteCount;

        bool isSlugFren = true;
        bool isDogFren = false;

        readonly Dictionary<string, BitmapImage> loadedImages = [];

        FrenObject _Slug_Fren;
        FrenObject _Dog_Fren;




        public MainWindow()
        {
            try
            {
                
                // Preload images
                LoadAllImages();
                // init main window
                InitializeComponent();
                LoadFrenObjects();
                this.ShowInTaskbar = false;
                this.Topmost = true;
                // Initialize NotifyIcon
                if(TaskIcon != null)
                {
                    TaskIcon.Icon = ImageManager.GetIcon("slug_icon"); // Replace with your icon path
                    TaskIcon.Visible = true;
                    TaskIcon.Text = "Desktop Fren";
                    TaskIcon.DoubleClick += (s, e) => Show();
                    // Create context menu for NotifyIcon
                    var settingsMenu = new SettingsMenu(this);
                    // Assign the context menu to TaskIcon
                    TaskIcon.ContextMenuStrip = settingsMenu._menuStrip;
                }
                // Start the timer if not null
                if (timer != null)
                {
                    timer.Interval = TimeSpan.FromMilliseconds(55); // animation speed
                    timer.Tick += TranslateFrenAnim;
                    timer.Start();
                }
                // Flip the initial image (Comment these out if your sprites default is facing right)
                ScaleTransform flipTransform = new(-1, 1);
                animatedImage.RenderTransform = flipTransform;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }

        void LoadFrenObjects()
        {
            _Slug_Fren = new("Slug", 6, this, 30, this._AnimatedImg_1,75,75,-15);
            _Dog_Fren = new("Dog", 6, this, 40 , _AnimatedImg_2,100,125,-20);

            SetFrenActive(_Dog_Fren);
            SetFrenActive(_Slug_Fren);
        }
        void SetFrenActive(FrenObject fren)
        {
            if (fren != null)
            {
                fren.SetActive();
            }
        }

        private void TranslateFrenAnim(object? sender, EventArgs e)
        {
            try
            {
                var resourceName = "";
                // Get the next frame from the preloaded images
                if (isSlugFren)
                {
                    resourceName = $"Slug_{CurrentFrame + 1}";
                    CurrentSpriteCount = SlugSpriteCount;
                }
                if (isDogFren)
                {
                    resourceName = $"Dog_{CurrentFrame + 1}";
                    CurrentSpriteCount = DogSpriteCount;
                }
                var imageSource = loadedImages[resourceName];
                animatedImage.Source = imageSource;
                // Update the current frame index
                CurrentFrame = (CurrentFrame + 1) % CurrentSpriteCount;
                // Get current position
                double currentX = Canvas.GetLeft(animatedImage);
                // Update position based on direction
                if (MoveRight)
                {
                    currentX += MoveAmount;
                    if (currentX >= MainCanvas.Width)
                    {
                        currentX = MainCanvas.Width - animatedImage.ActualWidth;
                        MoveRight = false; // Change direction
                        animatedImage.RenderTransform = null; // flip 
                    }
                }
                else
                {
                    currentX -= MoveAmount;
                    if (currentX <= 10)
                    { // Adjust if needed
                        currentX = 0;
                        MoveRight = true; // Change direction // Reset the image flip
                        animatedImage.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                        ScaleTransform flipTransform = new(-1, 1);
                        animatedImage.RenderTransform = flipTransform;
                    }
                }
                // Apply new position
                Canvas.SetLeft(animatedImage, currentX);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public void SetSlugFren()
        {
            // Handle Option 1 click
            isDogFren = false;
            isSlugFren = true;
            animatedImage.Height = 75;
            animatedImage.Width = 75;
            Canvas.SetTop(animatedImage, -15);
        }
        public void SetDogFren()
        {
            // Handle Option 2 click
            isSlugFren = false;
            isDogFren = true;
            animatedImage.Height = 100;
            animatedImage.Width = 125;
            Canvas.SetTop(animatedImage, -20);
        }


        private void LoadAllImages()
        {

            string[] imageNames = ["Slug_1", "Slug_2", "Slug_3", "Slug_4", "Slug_5", "Slug_6"];
            foreach (var name in imageNames)
            {
                loadedImages[name] = ImageManager.GetBitmapImage(name);
            }


            string[] imageNamesDog = ["Dog_1", "Dog_2", "Dog_3", "Dog_4", "Dog_5", "Dog_6", "Dog_7"];
            foreach (var name in imageNamesDog)
            {
                loadedImages[name] = ImageManager.GetBitmapImage(name);
            }

        }

    }

}
