using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowWatcher
{
    public partial class SettingsView : BaseView
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        public override void Initialize(LogRepository repository)
        {
            Text = "Settings";

            using (var reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                var load = (reg.GetValue(Application.ProductName) != null);
                _loadCheckBox.Checked = load;

                if (load)
                {
                    reg.SetValue(Application.ProductName, Assembly.GetExecutingAssembly().Location);
                }
            }
        }

        private void chkLoadStartUp_CheckedChanged(object sender, EventArgs e)
        {
            using (var reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (_loadCheckBox.Checked)
                {
                    reg.SetValue(Application.ProductName, Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    reg.DeleteValue(Application.ProductName, false);
                }
            }
        }
    }
}