/* ############################################
 * ### Dalton Christopher                   ###
 * ### Desktop-Frens - Windows - .NET8.0    ###
 * ### 05/2024                              ###
 * ############################################*/
using System.Windows.Media.Imaging;

namespace Desktop_Frens
{
    public class SettingsMenu // Menu for Setting the frens 
    {
        // Colours
        private readonly Color _backgroundColour = Color.Black;
        private readonly Color _textColour = Color.Red;

        //Menu Strip and reference to the _mainWindow
        public ContextMenuStrip _menuStrip;
        readonly MainWindow _mainWindow;

        // flags for checked in menu
        readonly bool _isAllFrens = false;
        private bool _isSlugFren = false;
        private bool _isDogFren = false;
        private bool _isSpookyFren = false;
        private bool _isFrogFren = false;
        private bool _isBlueFrogFren = false;
        readonly Font _Font;

        // Contructo
        public SettingsMenu(MainWindow mainWindow)
        {
            _Font = MakeFont();
            _mainWindow = mainWindow;
            _menuStrip = CreateContextMenu(); // Initialize the context menu strip
        }

        // MenuStrip getter
        public ContextMenuStrip GetMenuStrip()
        {
            return _menuStrip;
        }

        static Font MakeFont()
        {
            // Define the font attributes
            FontStyle style = FontStyle.Bold;
            string familyName = "Consolas";
            float size = 12; // You can adjust the size as needed

            Font customFont = new Font(familyName, size, style);
            return customFont;
        }

        // Method to build the inital Context Menu
        private ContextMenuStrip CreateContextMenu()
        {
            var menuStrip = new ContextMenuStrip
            {
                RenderMode = ToolStripRenderMode.Professional,
                Renderer = new ToolStripProfessionalRenderer(new MenuColorTable()),
                BackColor = Color.DarkRed,
                ForeColor = _textColour,
                Font = _Font,
                Opacity = 0.75f,
                ShowItemToolTips = true,

            };
            var settingsMenu = CreateSettingsMenuItem();
            var flipperMenu = CreateFlipItem();
            var changeScreenMenuItem = CreateChangeScreenMenuItem();
            var exitMenuItem = CreateExitMenuItem();

            menuStrip.Items.Add(settingsMenu);
            menuStrip.Items.Add(changeScreenMenuItem);
            menuStrip.Items.Add(flipperMenu);
            menuStrip.Items.Add(exitMenuItem);
            foreach (var item in 
                menuStrip.Items.OfType<ToolStripMenuItem>().Where(x => x.HasDropDown))
            {
                if (item.DropDown is ToolStripDropDownMenu toolStrip)
                {
                    toolStrip.AllowTransparency = true;
                    toolStrip.Opacity = 0.75;
                }
            }
            return menuStrip;
        }

        // update menu flags -> check boxy bois
        void UpdateCheckboxes()
        {
            // Update checkboxes based on flags
            foreach (ToolStripMenuItem item in _menuStrip.Items)
            {
                if (item.Text == "Settings")
                {
                    foreach (ToolStripMenuItem subItem in item.DropDownItems)
                    {
                        if (subItem.Text == "Slug - Fren")
                        {
                            subItem.Checked = _isSlugFren;
                        }
                        else if (subItem.Text == "Dog - Fren")
                        {
                            subItem.Checked = _isDogFren;
                        }
                        else if (subItem.Text == "Spooky - Fren")
                        {
                            subItem.Checked = _isSpookyFren;
                        }
                        else if (subItem.Text == "Frog Pink - Fren")
                        {
                            subItem.Checked = _isFrogFren;
                        }
                        else if (subItem.Text == "Frog Blue - Fren")
                        {
                            subItem.Checked = _isBlueFrogFren;
                        }
                    }
                    break; // Exit the loop after updating settings submenu
                }
            }
        }

        ToolStripMenuItem CreateFlipItem()
        {
            var flipAllItem = new ToolStripMenuItem("Flip")
            {
                BackColor = _backgroundColour,
                ForeColor = _textColour,
                Padding = new Padding(2),
                Margin = new Padding(1),
                Font = _Font,
                Image = (Image)ImageManager.GetImage("Switch", typeof(Image))
            };
            flipAllItem.Click += (sender, e) =>
            {
                _mainWindow.FlipFrens();
            };
            return flipAllItem;
        }

        // Build the tool Strip items
        private ToolStripMenuItem CreateSettingsMenuItem()
        {
            var settingsMenu = new ToolStripMenuItem("Settings")
            {
                BackColor = _backgroundColour,
                ForeColor = _textColour,
                Padding = new Padding(0),
                Margin = new Padding(1),
                Image = (Image)ImageManager.GetImage("Settings", typeof(Image)),
                Font = _Font,
            };
            var option0MenuItem = CreateSubMenuItem("All - Frens", _isAllFrens, "All");
            var option1MenuItem = CreateSubMenuItem("Slug - Fren", _isSlugFren, "Slug_3");
            var option2MenuItem = CreateSubMenuItem("Dog - Fren", _isDogFren, "Dog_1");
            var option3MenuItem = CreateSubMenuItem("Spooky - Fren", _isSpookyFren, "Spooky_Icon");
            var option4MenuItem = CreateSubMenuItem("Frog Pink - Fren", _isFrogFren, "Frog_5");
            var option5MenuItem = CreateSubMenuItem("Frog Blue - Fren", _isBlueFrogFren, "Frog_B_5");

            settingsMenu.DropDownItems.Add(option0MenuItem);
            settingsMenu.DropDownItems.Add(option1MenuItem);
            settingsMenu.DropDownItems.Add(option2MenuItem);
            settingsMenu.DropDownItems.Add(option3MenuItem);
            settingsMenu.DropDownItems.Add(option4MenuItem);
            settingsMenu.DropDownItems.Add(option5MenuItem);

            // Assign the click events
            option0MenuItem.Click += (sender, e) => SetAllFrens();
            option1MenuItem.Click += (sender, e) => SetSlugFren();
            option2MenuItem.Click += (sender, e) => SetDogFren();
            option3MenuItem.Click += (sender, e) => SetSpookyFren();
            option4MenuItem.Click += (sender, e) => SetFrogFren();
            option5MenuItem.Click += (sender, e) => SetBlueFrogFren();
            return settingsMenu;
        }
        public void AllEnabled()
        {
            SetAllFrens();
        }
        async void SetAllFrens() // call all Fren Setters 
        {
            SetSlugFren();
            await Task.Delay(85);
            SetDogFren();
            await Task.Delay(185);
            SetSpookyFren();
            await Task.Delay(165);
            SetFrogFren();
            await Task.Delay(285);
            SetBlueFrogFren();
        }
        private void SetSlugFren() // SLug
        {
            if (_mainWindow._Slug_Fren == null) return;
            if (_isSlugFren) _isSlugFren = false;
            else _isSlugFren = true;
            MainWindow.SetFrenActive(_mainWindow._Slug_Fren);
            UpdateCheckboxes();
        }
        private void SetDogFren() // Dog
        {

            if (_mainWindow._Dog_Fren == null) return;
            if (_isDogFren) _isDogFren = false;
            else _isDogFren = true;
            MainWindow.SetFrenActive(_mainWindow._Dog_Fren);
            UpdateCheckboxes();
        }
        private void SetSpookyFren() // Spooky
        {
            if (_mainWindow._Spooky_Fren == null) return;
            if (_isSpookyFren) _isSpookyFren = false;
            else _isSpookyFren = true;
            MainWindow.SetFrenActive(_mainWindow._Spooky_Fren);
            UpdateCheckboxes();
        }
        private void SetFrogFren() // Frog
        {
            if (_mainWindow._Frog_Fren == null) return;
            if (_isFrogFren) _isFrogFren = false;
            else _isFrogFren = true;
            MainWindow.SetFrenActive(_mainWindow._Frog_Fren);
            UpdateCheckboxes();
        }
        private void SetBlueFrogFren() // Frog
        {
            if (_mainWindow._Frog_B_Fren == null) return;
            if (_isBlueFrogFren) _isBlueFrogFren = false;
            else _isBlueFrogFren = true;
            MainWindow.SetFrenActive(_mainWindow._Frog_B_Fren);
            UpdateCheckboxes();
        }
        // Sub Menu builder
        private ToolStripMenuItem CreateSubMenuItem(string text, bool isChecked, String image)
        {
            var menuItem = new ToolStripMenuItem(text)
            {
                BackColor = _backgroundColour,
                ForeColor = _textColour,
                Checked = isChecked,
                Margin = new Padding(1),
                Image = (Image)ImageManager.GetImage(image, typeof(Image)),

            };

            return menuItem;
        }
        // Menu Item Builder
        private ToolStripMenuItem CreateExitMenuItem()
        {
            var exitMenuItem = new ToolStripMenuItem("Exit")
            {
                BackColor = _backgroundColour,
                ForeColor = _textColour,
                Padding = new Padding(2),
                Margin = new Padding(0),
                Image = (Image)ImageManager.GetImage("Exit", typeof(Image)),
                Font = _Font,
            };
            exitMenuItem.Click += (sender, e) =>
            {
                System.Windows.Application.Current.Shutdown();
            };
            return exitMenuItem;
        }
        private ToolStripMenuItem CreateChangeScreenMenuItem()
        {
            var changeScreenMenuItem = new ToolStripMenuItem("Change Screen")
            {
                BackColor = _backgroundColour,
                ForeColor = _textColour,
                Padding = new Padding(2),
                Margin = new Padding(1),
                Font = _Font,
                Image = (Image)ImageManager.GetImage("Screen", typeof(Image)),
            };

            // Add submenu items for changing to each screen
            var screen1MenuItem = CreateScreenChangeSubMenuItem("Display - 1", "DISPLAY1");
            var screen2MenuItem = CreateScreenChangeSubMenuItem("Display - 2", "DISPLAY2");
            var screen3MenuItem = CreateScreenChangeSubMenuItem("Display - 3", "DISPLAY3");

            // Add submenu items to the main change screen menu item
            changeScreenMenuItem.DropDownItems.Add(screen1MenuItem);
            changeScreenMenuItem.DropDownItems.Add(screen2MenuItem);
            changeScreenMenuItem.DropDownItems.Add(screen3MenuItem);

            return changeScreenMenuItem;
        }

        private ToolStripMenuItem CreateScreenChangeSubMenuItem(string text, string screenName)
        {
            var menuItem = new ToolStripMenuItem(text)
            {
                BackColor = _backgroundColour,
                ForeColor = _textColour,
                Padding = new Padding(2),
                Margin = new Padding(1),
                Font = _Font,
                Image = (Image)ImageManager.GetImage("Screen", typeof(Image)),
                //CheckOnClick = true,
            };

            menuItem.Click += (sender, e) =>
            {
                _mainWindow.ChangeScreen(screenName);
            };

            return menuItem;
        }




    }

    public class MenuColorTable : ProfessionalColorTable
    {
        public MenuColorTable()
        {
            UseSystemColors = false;
        }

        public override Color ImageMarginGradientBegin => Color.DarkRed;
        public override Color ImageMarginGradientMiddle => Color.DarkRed;
        public override Color ImageMarginGradientEnd => Color.DarkRed;
        public override Color ToolStripBorder => Color.DarkRed;
        public override Color StatusStripBorder => Color.DarkRed;
        public override Color MenuBorder => Color.DarkRed;
        public override Color MenuItemBorder => Color.DarkRed;
        public override Color MenuItemSelected => Color.DarkRed;
        public override Color MenuItemSelectedGradientBegin => Color.DarkRed;
        public override Color MenuItemSelectedGradientEnd => Color.DarkRed;
        public override Color MenuStripGradientBegin => Color.DarkRed;
        public override Color MenuStripGradientEnd => Color.DarkRed;
        public override Color ToolStripContentPanelGradientEnd => Color.DarkRed;
        public override Color ToolStripContentPanelGradientBegin => Color.DarkRed;
        public override Color ToolStripGradientBegin => Color.DarkRed;
        public override Color ToolStripGradientMiddle => Color.DarkRed;
        public override Color ToolStripGradientEnd => Color.DarkRed;
        public override Color ToolStripPanelGradientBegin => Color.DarkRed;
        public override Color ToolStripPanelGradientEnd => Color.DarkRed;
        public override Color ToolStripDropDownBackground => Color.DarkRed;
        public override Color ButtonCheckedHighlightBorder => Color.DarkRed;
        public override Color ImageMarginRevealedGradientBegin => Color.DarkRed;
        public override Color ImageMarginRevealedGradientMiddle => Color.DarkRed;
        public override Color ImageMarginRevealedGradientEnd => Color.DarkRed;
        public override Color CheckSelectedBackground => Color.DarkRed;
        public override Color CheckBackground => Color.DarkRed;
        public override Color ButtonCheckedHighlight => Color.DarkRed;
    }


}
