using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace WPF_Desktop_Fren
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;
        private int CurrentFrame = 0;
        private readonly string FileDirct = @"Sprites\"; // Path to sprite folder
        private readonly double MoveAmount = 8.5; // Movement speed
        private bool MoveRight = true; // Direction flag
        private NotifyIcon notifyIcon;

        private Dictionary<string, BitmapImage> loadedImages = new Dictionary<string, BitmapImage>();


        private readonly string[] FramePaths =
        {
        "pack://application:,,,/WPF_Desktop_Fren;component/Resources/slug-1.png",
        "pack://application:,,,/WPF_Desktop_Fren;component/Resources/slug-2.png",
        "pack://application:,,,/WPF_Desktop_Fren;component/Resources/slug-3.png",
        "pack://application:,,,/WPF_Desktop_Fren;component/Resources/slug-4.png",
        "pack://application:,,,/WPF_Desktop_Fren;component/Resources/slug-3.png",
        "pack://application:,,,/WPF_Desktop_Fren;component/Resources/slug-2.png",
        };


        public MainWindow()
        {
            try
            {
                // Preload images
                LoadAllImages();
                InitializeComponent();
                this.ShowInTaskbar = false;
                this.Topmost = true;

                // Initialize NotifyIcon
                notifyIcon = new NotifyIcon();
                notifyIcon.Icon = new System.Drawing.Icon(@"Icons\slug_icon.ico"); // Replace with your icon path
                notifyIcon.Visible = true;
                notifyIcon.Text = "Desktop Fren";
                notifyIcon.DoubleClick += (s, e) => Show();

                // Create context menu for NotifyIcon
                notifyIcon.ContextMenuStrip = CreateContextMenu();




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
                System.Windows.MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }

        private void TranslateFrenAnim(object sender, EventArgs e)
        {
            try
            {
                // Get the next frame from the preloaded images
                var resourceName = $"slug_{CurrentFrame + 1}";
                var imageSource = LoadImage(resourceName);

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
                System.Windows.MessageBox.Show($"Error: {ex.Message}");
            }
        }

        internal ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem settingsMenuItem = new ToolStripMenuItem("Settings");
            settingsMenuItem.Click += SettingsMenuItem_Click;
            menu.Items.Add(settingsMenuItem);

            return menu;
        }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            // Open settings menu or window
            // You can replace this with the actual settings window or menu logic
            System.Windows.MessageBox.Show("Settings menu clicked!");
        }



        private void LoadAllImages()
        {
            string[] imageNames = { "slug_1", "slug_2", "slug_3", "slug_4" };

            foreach (var name in imageNames)
            {
                loadedImages[name] = LoadImage(name);
            }
        }

        private BitmapImage LoadImage(string resourceName)
        {
            try
            {
                System.Drawing.Bitmap bitmap = null;

                switch (resourceName)
                {
                    case "slug_1":
                        bitmap = Properties.Resources.slug_1;
                        break;
                    case "slug_2":
                        bitmap = Properties.Resources.slug_2;
                        break;
                    case "slug_3":
                        bitmap = Properties.Resources.slug_3;
                        break;
                    case "slug_4":
                        bitmap = Properties.Resources.slug_4;
                        break;
                    case "slug_5":
                        bitmap = Properties.Resources.slug_3;
                        break;
                    case "slug_6":
                        bitmap = Properties.Resources.slug_2;
                        break;
                    default:
                        return null;
                }

                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    memory.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                }

                bitmapImage.Freeze(); // Freeze the image to prevent further modifications

                return bitmapImage;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading image: {ex.Message}");
                return null;
            }
        }





    }
}
