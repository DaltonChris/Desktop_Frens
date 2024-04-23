using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPF_Desktop_Fren
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;
        private int CurrentFrame = 0;
        private readonly string FileDirct = @"F:\C#_Repos\WPF_Desktop_Fren\Sprites"; // Path to sprite folder
        private readonly string[] FramePaths;
        private readonly double MoveAmount = 8.5; // Movement speed
        private bool MoveRight = true; // Direction flag

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                this.ShowInTaskbar = false; // Hide from taskbar and make always on top
                this.Topmost = true;
                FramePaths = new string[] // Initialize the frame paths with the concatenated directory path
                {
                FileDirct + @"\Slug-1.png", // Sprite names
                FileDirct + @"\Slug-2.png",
                FileDirct + @"\Slug-3.png",
                FileDirct + @"\Slug-4.png",
                FileDirct + @"\Slug-3.png",
                FileDirct + @"\Slug-2.png",
                };
                // Start the timer
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(55); // animation speed
                timer.Tick += TranslateFrenAnim;
                timer.Start();
                // Flip the initial image (Comment these out if your sprites default is facing right)
                ScaleTransform flipTransform = new(-1, 1);
                animatedImage.RenderTransform = flipTransform;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }  
        }

        private async void TranslateFrenAnim(object sender, EventArgs e)
        {
            try
            {
                // Load the next frame asynchronously
                var framePath = FramePaths[CurrentFrame];
                var imageSource = await LoadImageAsync(framePath);
                animatedImage.Source = imageSource;
                // Update the current frame index
                CurrentFrame = (CurrentFrame + 1) % FramePaths.Length;
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
                    if (currentX <= 10) // Adjust if needed
                    {
                        currentX = 0;
                        MoveRight = true; // Change direction
                                          // Reset the image flip
                        animatedImage.RenderTransformOrigin = new Point(0.5, 0.5);
                        ScaleTransform flipTransform = new(-1, 1);
                        animatedImage.RenderTransform = flipTransform;
                    }
                }

                // Apply new position
                Canvas.SetLeft(animatedImage, currentX);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }



        private static async Task<BitmapImage> LoadImageAsync(string path)
        {
            try
            {
                BitmapImage bitmapImage = new();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(path);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                await bitmapImage.Dispatcher.InvokeAsync(() =>
                {
                    bitmapImage.Freeze();
                }, System.Windows.Threading.DispatcherPriority.Render);

                return bitmapImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
                return null;
            }
        }

    }
}
