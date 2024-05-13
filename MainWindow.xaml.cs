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

        public FrenObject? _Slug_Fren;
        public FrenObject? _Dog_Fren;
        public FrenObject? _Spooky_Fren;
        public FrenObject? _Frog_Fren;


        public MainWindow()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }

        void LoadFrenObjects()
        {
            _Slug_Fren = new("Slug", 6, this, 65, this._AnimatedImg_1,75,75,-15);
            _Dog_Fren = new("Dog", 6, this, 40 , _AnimatedImg_2,100,125,-20);
            _Spooky_Fren = new("Spooky", 8, this, 85, _AnimatedImg_3, 100, 125, -20);
            _Frog_Fren = new("Frog", 7, this, 155, _AnimatedImg_4, 100, 125, -10);

            //SetFrenActive(_Dog_Fren);
            //SetFrenActive(_Slug_Fren);
            //SetFrenActive(_Spooky_Fren);
            SetFrenActive(_Frog_Fren);
        }

        static void SetFrenActive(FrenObject fren)
        {
            fren?.SetActive();
        }
        public static void DisableFren(FrenObject fren)
        {
            fren?.Disable();
        }

        public void SetSlugFren()
        {
            if (_Slug_Fren.IsActive())
            {
                _Slug_Fren.Disable();
            }
            else
                _Slug_Fren.SetActive();
        }
        public void SetDogFren()
        {
            if (_Dog_Fren.IsActive())
            {
                _Dog_Fren.Disable();
            }else
                _Dog_Fren.SetActive();
        }
        public void SetSpookyFren()
        {
            if (_Spooky_Fren.IsActive())
            {
                _Spooky_Fren.Disable();
            }
            else
                _Spooky_Fren.SetActive();
        }
        public void SetFrogFren()
        {
            if (_Frog_Fren.IsActive())
            {
                _Frog_Fren.Disable();
            }
            else
                _Frog_Fren.SetActive();
        }

    }

}
