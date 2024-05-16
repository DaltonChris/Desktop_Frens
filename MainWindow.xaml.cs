/* ############################################
 * ### Dalton Christopher                   ###
 * ### Desktop-Frens - Windows - .NET8.0    ###
 * ### 05/2024                              ###
 * ############################################*/

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Desktop_Frens
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")] // Mouse shit
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008; //flags
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        readonly NotifyIcon TaskIcon = new();
        // Fren Objects
        public FrenObject? _Slug_Fren;
        public FrenObject? _Dog_Fren;
        public FrenObject? _Spooky_Fren;
        public FrenObject? _Frog_Fren;
        public FrenObject? _Frog_B_Fren;
        public FrenObject? _Frog_G_Fren;


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
                if (TaskIcon != null)
                {
                    //Icon and toolbar menu/display options
                    TaskIcon.Icon = (Icon)ImageManager.GetImage("slug_icon", typeof(Icon));
                    TaskIcon.Visible = true;
                    TaskIcon.Text = "Desktop Fren";
                    // Create context menu for NotifyIcon
                    var settingsMenu = new SettingsMenu(this);
                    // Assign the context menu to TaskIcon
                    TaskIcon.ContextMenuStrip = settingsMenu._menuStrip;
                    settingsMenu.AllEnabled();

                    TaskIcon.Click += (s, e) => MenuClick();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }

        void LoadFrenObjects()
        {
            _Slug_Fren = new(ID.Slug, 6, this, 0.4, 95, this._AnimatedImg_1, 50, 50, 11); // -1
            _Dog_Fren = new(ID.Dog, 7, this, 7.4, 75, _AnimatedImg_2, 85, 95, -5); // -5
            _Spooky_Fren = new(ID.Spooky, 8, this, 5.9, 85, _AnimatedImg_3, 110, 110, -50); // -52
            _Frog_Fren = new(ID.Frog, 7, this, 0.3, 135, _AnimatedImg_4, 75, 100, 10); //10  move-1.3
            _Frog_B_Fren = new(ID.Frog_B, 7, this, 0.3, 135, _AnimatedImg_5, 95, 115, -5); // -5 move-2
            _Frog_G_Fren = new(ID.Frog_G, 7, this, 0.3, 135, _AnimatedImg_6, 85, 105, 5); // -5 move-2
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

        public void FlipFrens()
        {
            _Slug_Fren.PublicFlip();
            _Dog_Fren.PublicFlip();
            _Slug_Fren.PublicFlip();
            _Frog_B_Fren.PublicFlip();
            _Frog_Fren.PublicFlip();
            _Spooky_Fren.PublicFlip();
            _Frog_G_Fren.PublicFlip();
        }

        static void MenuClick()
        {
            // Simulate right mouse button down event
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            // Simulate right mouse button up event
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public void ChangeScreen(string screenName)
        {
            // Get the names of all screens
            string allScreenNames = string.Join("\n", Screen.AllScreens.Select(screen => screen.DeviceName));

            // Show a message box with the screen names
            //System.Windows.MessageBox.Show("Screen Names:\n" + allScreenNames);

            // Check if the specified screen name exists
            var targetScreen = Screen.AllScreens.FirstOrDefault(screen => screen.DeviceName.EndsWith(screenName));
            if (targetScreen != null)
            {

                this.Left = targetScreen.Bounds.Left - 25;
                if(screenName == "DISPLAY3")this.Top = targetScreen.Bounds.Top + 25;
                else if (screenName == "DISPLAY2") this.Top = targetScreen.Bounds.Top + 15;
                else if (screenName == "DISPLAY1") this.Top = targetScreen.Bounds.Top + 15;

                this.Width = targetScreen.WorkingArea.Width;
                this.Height = targetScreen.WorkingArea.Height;

                // Position the canvas at the bottom corner
                MainCanvas.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                MainCanvas.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                Canvas.SetLeft(MainCanvas, 0);
                Canvas.SetTop(MainCanvas, 0);
            }
            else
            {
                // Display a message indicating that the specified screen was not found
                System.Windows.MessageBox.Show($"Screen '{screenName}' not found.");
            }
        }

    }

}
