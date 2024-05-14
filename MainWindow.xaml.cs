using System.Windows;
/* ############################################
 * ### Dalton Christopher                   ###
 * ### Desktop-Frens - Windows - .NET8.0    ###
 * ### 05/2024                              ###
 * ############################################*/

namespace Desktop_Frens
{
    public partial class MainWindow : Window
    {
        readonly NotifyIcon TaskIcon = new();

        // Fren Objects
        public FrenObject? _Slug_Fren;
        public FrenObject? _Dog_Fren;
        public FrenObject? _Spooky_Fren;
        public FrenObject? _Frog_Fren;

        public MainWindow() // Main
        {
            try
            {
                // ToolBar settings
                this.ShowInTaskbar = false;
                this.Topmost = true;

                // init main window
                InitializeComponent();

                // Init Frens
                LoadFrenObjects();


                // Initialize NotifyIcon
                if(TaskIcon != null)
                {
                    //Icon and toolbar menu/display options
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

        /// <summary>
        /// Inits the Fren objects Using Fren Contructor
        /// </summary>
        void LoadFrenObjects()
        {
            _Slug_Fren = new("Slug", 6 , this,5.1, 88, this._AnimatedImg_1,60,60,-1);
            _Dog_Fren = new("Dog", 7 , this,7.4, 75 , _AnimatedImg_2,85,95,-5);
            _Spooky_Fren = new("Spooky", 8 , this,6.5, 85, _AnimatedImg_3, 125, 175, -60);
            _Frog_Fren = new("Frog", 7 , this,2.75, 125, _AnimatedImg_4, 75, 100, 10);
        }

        /// <summary>
        /// Set Fren Object On/Off via passed Fren
        /// </summary>
        /// <param name="fren"> The Fren to Update On/Off </param>
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
