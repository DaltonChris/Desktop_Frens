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
        readonly DispatcherTimer timer;
        int CurrentFrame = 0;
        readonly double MoveAmount = 8.5;
        bool MoveRight = true;
        readonly NotifyIcon TaskIcon;

        readonly int SlugSpriteCount = 6;
        readonly int DogSpriteCount = 7;
        int CurrentSpriteCount;

        bool isSlugFren = true;
        bool isDogFren = false;

        readonly Dictionary<string, BitmapImage> loadedImages = new Dictionary<string, BitmapImage>();
        ToolStripMenuItem settingsMenu;

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
                TaskIcon = new NotifyIcon();
                TaskIcon.Icon = Properties.Resources.slug_icon; // Replace with your icon path
                TaskIcon.Visible = true;
                TaskIcon.Text = "Desktop Fren";
                TaskIcon.DoubleClick += (s, e) => Show();
                // Create context menu for NotifyIcon
                TaskIcon.ContextMenuStrip = CreateContextMenu();
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
                var resourceName = "";
                // Get the next frame from the preloaded images
                if (isSlugFren) { 
                    resourceName = $"slug_{CurrentFrame + 1}";
                    CurrentSpriteCount = SlugSpriteCount;
                }
                if (isDogFren) { 
                    resourceName = $"Dog_{CurrentFrame + 1}";
                    CurrentSpriteCount = DogSpriteCount;
                }
                var imageSource = LoadImage(resourceName);
                animatedImage.Source = imageSource;
                // Update the current frame index
                CurrentFrame = (CurrentFrame + 1) % CurrentSpriteCount;
                // Get current position
                double currentX = Canvas.GetLeft(animatedImage);
                // Update position based on direction
                if (MoveRight){
                    currentX += MoveAmount;
                    if (currentX >= MainCanvas.Width)
                    {
                        currentX = MainCanvas.Width - animatedImage.ActualWidth;
                        MoveRight = false; // Change direction
                        animatedImage.RenderTransform = null; // flip 
                    }
                }else{
                    currentX -= MoveAmount;
                    if (currentX <= 10){ // Adjust if needed
                        currentX = 0;
                        MoveRight = true; // Change direction // Reset the image flip
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
            // Settings submenu
            settingsMenu = new ToolStripMenuItem("Settings");
            // Submenu Slug
            ToolStripMenuItem option1MenuItem = new ToolStripMenuItem("Slug - Fren");
            option1MenuItem.Click += SetSlugFren;
            option1MenuItem.Checked = isSlugFren; // Set check box to checked
            // Submenu Dog
            ToolStripMenuItem option2MenuItem = new ToolStripMenuItem("Dog - Fren");
            option2MenuItem.Click += SetDogFren;
            option2MenuItem.Checked = isDogFren; // Set check box to unchecked
            // Add submenu items to Settings
            settingsMenu.DropDownItems.Add(option1MenuItem);
            settingsMenu.DropDownItems.Add(option2MenuItem);
            menu.Items.Add(settingsMenu);
            // Separator
            menu.Items.Add(new ToolStripSeparator());
            // Exit item
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit");
            exitMenuItem.Click += ExitMenuItem_Click;
            menu.Items.Add(exitMenuItem);
            return menu;
        }
        private void SetSlugFren(object sender, EventArgs e)
        {
            // Handle Option 1 click
            isDogFren = false;
            isSlugFren = true;
            animatedImage.Height = 75;
            animatedImage.Width = 75;
            Canvas.SetTop(animatedImage, -15);
            UpdateCheckboxes();
        }
        private void SetDogFren(object sender, EventArgs e)
        {
            // Handle Option 2 click
            isSlugFren = false;
            isDogFren = true;
            animatedImage.Height = 100;
            animatedImage.Width = 125;
            Canvas.SetTop(animatedImage, -20);
            UpdateCheckboxes();
        }
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            // Handle Exit click
            System.Windows.Application.Current.Shutdown();
        }
        private void UpdateCheckboxes()
        {
            // Update checkboxes based on flags
            foreach (ToolStripMenuItem item in settingsMenu.DropDownItems)
            {
                if (item.Text == "Slug - Fren")
                {
                    item.Checked = isSlugFren;
                }
                else if (item.Text == "Dog - Fren")
                {
                    item.Checked = isDogFren;
                }
            }
        }

        private void LoadAllImages()
        {

            string[] imageNames = { "slug_1", "slug_2", "slug_3", "slug_4" };
            foreach (var name in imageNames)
            {
                loadedImages[name] = LoadImage(name);
            }
            

            string[] imageNamesDog = { "Dog_1", "Dog_2", "Dog_3", "Dog_4", "Dog_5", "Dog_6", "Dog_7" };
            foreach (var name in imageNamesDog)
            {
                loadedImages[name] = LoadImage(name);
            }
            
        }

        private BitmapImage LoadImage(string resourceName)
        {
            try
            {
                System.Drawing.Bitmap bitmap = null;

                if (isSlugFren)
                {
                    switch (resourceName) { 
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
                }
                
                if (isDogFren)
                {
                    switch (resourceName)
                    {
                        case "Dog_1":
                            bitmap = Properties.Resources.Dog_1;
                            break;
                        case "Dog_2":
                            bitmap = Properties.Resources.Dog_2;
                            break;
                        case "Dog_3":
                            bitmap = Properties.Resources.Dog_3;
                            break;
                        case "Dog_4":
                            bitmap = Properties.Resources.Dog_4;
                            break;
                        case "Dog_5":
                            bitmap = Properties.Resources.Dog_5;
                            break;
                        case "Dog_6":
                            bitmap = Properties.Resources.Dog_6;
                            break;
                        case "Dog_7":
                            bitmap = Properties.Resources.Dog_7;
                            break;
                        default:
                            return null;
                    }
                }


                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, ImageFormat.Png);
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
