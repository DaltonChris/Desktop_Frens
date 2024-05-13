using System.Windows.Forms;

namespace Desktop_Frens
{
    public class SettingsMenu
    {
        private readonly Color _backgroundColour = Color.Black;
        private readonly Color _textColour = Color.Red;
        private bool _isSlugFren = true;
        private bool _isDogFren = false;
        private bool _isSpookyFren = false;
        private bool _isFrogFren = false;
        public ContextMenuStrip _menuStrip;
        readonly MainWindow _mainWindow;

        public SettingsMenu(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _menuStrip = CreateContextMenu(); // Initialize the context menu strip
        }

        public ContextMenuStrip GetMenuStrip()
        {
            return _menuStrip;
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var menuStrip = new ContextMenuStrip
            {
                RenderMode = ToolStripRenderMode.Professional,
                Renderer = new ToolStripSystemRenderer()
            };

            var settingsMenu = CreateSettingsMenuItem();
            var exitMenuItem = CreateExitMenuItem();

            menuStrip.Items.Add(settingsMenu);
            menuStrip.Items.Add(exitMenuItem);

            return menuStrip;
        }

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
                        else if (subItem.Text == "Frog - Fren")
                        {
                            subItem.Checked = _isFrogFren;
                        }
                    }
                    break; // Exit the loop after updating settings submenu
                }
            }
        }

        private ToolStripMenuItem CreateSettingsMenuItem()
        {
            var settingsMenu = new ToolStripMenuItem("Settings")
            {
                BackColor = _backgroundColour,
                ForeColor = _textColour,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            var option1MenuItem = CreateSubMenuItem("Slug - Fren", _isSlugFren);
            var option2MenuItem = CreateSubMenuItem("Dog - Fren", _isDogFren);
            var option3MenuItem = CreateSubMenuItem("Spooky - Fren", _isSpookyFren);
            var option4MenuItem = CreateSubMenuItem("Frog - Fren", _isFrogFren);

            settingsMenu.DropDownItems.Add(option1MenuItem);
            settingsMenu.DropDownItems.Add(option2MenuItem);
            settingsMenu.DropDownItems.Add(option3MenuItem);
            settingsMenu.DropDownItems.Add(option4MenuItem);

            // Assign the click events
            option1MenuItem.Click += (sender, e) => SetSlugFren();
            option2MenuItem.Click += (sender, e) => SetDogFren();
            option3MenuItem.Click += (sender, e) => SetSpookyFren();
            option4MenuItem.Click += (sender, e) => SetFrogFren();

            return settingsMenu;
        }

        private void SetSlugFren()
        {
            _isSlugFren = true;
            _mainWindow.SetSlugFren();
            UpdateCheckboxes();
        }
        private void SetDogFren()
        {
            _isDogFren = true;
            _mainWindow.SetDogFren();
            UpdateCheckboxes();
        }
        private void SetSpookyFren()
        {
            _isSpookyFren = true;
            _mainWindow.SetSpookyFren();
            UpdateCheckboxes();
        }
        private void SetFrogFren()
        {
            _isFrogFren = true;
            _mainWindow.SetFrogFren();
            UpdateCheckboxes();
        }
        private ToolStripMenuItem CreateSubMenuItem(string text, bool isChecked)
        {
            var menuItem = new ToolStripMenuItem(text)
            {
                BackColor = _backgroundColour,
                ForeColor = _textColour,
                Checked = isChecked,
                Margin = new Padding(0)
            };

            return menuItem;
        }
        private ToolStripMenuItem CreateExitMenuItem()
        {
            var exitMenuItem = new ToolStripMenuItem("Exit")
            {
                BackColor = _backgroundColour,
                ForeColor = _textColour,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            exitMenuItem.Click += (sender, e) =>
            {
                System.Windows.Application.Current.Shutdown();
            };

            return exitMenuItem;
        }

        public class CustomColorTable : ProfessionalColorTable //Colour overrides..
        {
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
