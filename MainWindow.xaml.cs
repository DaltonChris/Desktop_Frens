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
        readonly NotifyIcon TaskIcon = new();

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
        }

        public static void SetFrenActive(FrenObject fren)
        {
            if (fren == null) return;
            if (fren.IsActive())
                fren.Disable();
            else
                fren.SetActive();
        }

    }

}
