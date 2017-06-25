using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowWatcher
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var mutex = new Mutex(false, "Window Watcher");

            if (!mutex.WaitOne(TimeSpan.FromSeconds(5), false))
            {
                MessageBox.Show("Another instance of Window Watcher is already running!",
                    "Window Watcher",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                using (new MainForm())
                {
                    Application.Run();
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}