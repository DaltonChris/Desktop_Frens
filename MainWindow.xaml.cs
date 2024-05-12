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
using Color = System.Drawing.Color;
using System.Drawing;
using System.Windows.Interop;
using System.Drawing.Drawing2D;
using Point = System.Drawing.Point;
using Image = System.Drawing.Image;
using Microsoft.Diagnostics.Tracing.AutomatedAnalysis;

namespace Desktop_Frens
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
        static ContextMenuStrip MenuStrip = new();


        static ToolStripMenuItem SettingsMenu;
        static ToolStripMenuItem ExitMenuItem;


        static readonly Color BackgroundColour = Color.Black;
        static readonly Color TextColour = Color.Red;

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
                if (isSlugFren)
                {
                    resourceName = $"slug_{CurrentFrame + 1}";
                    CurrentSpriteCount = SlugSpriteCount;
                }
                if (isDogFren)
                {
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

        internal ContextMenuStrip CreateContextMenu()
        {
            MenuStrip.RenderMode = ToolStripRenderMode.Professional;
            MenuStrip.Renderer = new ToolStripSystemRenderer();
            // Settings submenu
            SettingsMenu = new ToolStripMenuItem("Settings")
            {
                BackColor = BackgroundColour,  // Set background color
                ForeColor = TextColour,      // Set text color
                Image = Desktop_frens.Properties.Resources.settings,
                ImageTransparentColor = BackgroundColour,
                Padding = new Padding(0),
                Margin = new Padding(0),
            };
            // Exit item
            ExitMenuItem = new ToolStripMenuItem("Exit")
            {
                BackColor = BackgroundColour,  // Set background color
                ForeColor = TextColour,      // Set text color
                Image = Properties.Resources.exit,
                ImageTransparentColor = BackgroundColour,
                Padding = new Padding(0),
                Margin = new Padding(0),
            };

            // Set ToolStrip LayoutStyle to Table
            MenuStrip.LayoutStyle = ToolStripLayoutStyle.Table;

            // Adjust the spacing between items
            MenuStrip.GripStyle = ToolStripGripStyle.Hidden;
            MenuStrip.Padding = new Padding(0);  // Remove any padding

            // Submenu Slug
            ToolStripMenuItem option1MenuItem = new ToolStripMenuItem("Slug - Fren");
            option1MenuItem.Click += SetSlugFren;
            option1MenuItem.Checked = isSlugFren;

            // Customize submenu item appearance
            option1MenuItem.BackColor = BackgroundColour;  // Set background color
            option1MenuItem.ForeColor = TextColour;      // Set text color
            option1MenuItem.BackgroundImageLayout = ImageLayout.None;
            option1MenuItem.Margin = new Padding(0);  // Set the margin to zero

            // Submenu Dog
            ToolStripMenuItem option2MenuItem = new ToolStripMenuItem("Dog - Fren");
            option2MenuItem.Click += SetDogFren;
            option2MenuItem.Checked = isDogFren;

            // Customize submenu item appearance
            option2MenuItem.BackColor = BackgroundColour;  // Set background color
            option2MenuItem.ForeColor = TextColour;      // Set text color
            option2MenuItem.ImageTransparentColor = BackgroundColour;
            option2MenuItem.Margin = new Padding(0);  // Set the margin to zero

            // Add submenu items to Settings
            SettingsMenu.DropDownItems.Add(option1MenuItem);
            SettingsMenu.DropDownItems.Add(option2MenuItem);

            MenuStrip.Items.Add(SettingsMenu);

            // Exit item
            ExitMenuItem.Click += ExitMenuItem_Click;

            MenuStrip.Items.Add(ExitMenuItem);

            return MenuStrip;
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
            foreach (ToolStripMenuItem item in SettingsMenu.DropDownItems)
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

        public class CustomColorTable : ProfessionalColorTable
        {
            //a bunch of other overrides...

            public override Color ToolStripBorder
            {
                get { return Color.FromArgb(100, 0, 0); }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return Color.FromArgb(64, 64, 64); }
            }
            public override Color ToolStripGradientBegin
            {
                get { return Color.FromArgb(64, 64, 64); }
            }
            public override Color ToolStripGradientEnd
            {
                get { return Color.FromArgb(64, 64, 64); }
            }
            public override Color ToolStripGradientMiddle
            {
                get { return Color.FromArgb(64, 64, 64); }
            }
            public override Color MenuItemBorder
            {
                get { return Color.FromArgb(100, 0, 0); }
            }
        }


    }

}
