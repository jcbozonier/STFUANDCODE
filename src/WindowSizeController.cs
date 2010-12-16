using System.ComponentModel;
using System.Windows;

namespace STFUANDCODE
{
    public class WindowSizeController
    {
        public WindowSizeController(Window window)
        {
            //http://stackoverflow.com/questions/847752/net-wpf-remember-window-size-between-sessions
            window.Top = Properties.Settings.Default.Top;
            window.Left = Properties.Settings.Default.Left;
            window.Height = Properties.Settings.Default.Height;
            window.Width = Properties.Settings.Default.Width;
            // Very quick and dirty - but it does the job
            if (Properties.Settings.Default.Maximized)
            {
                window.WindowState = WindowState.Maximized;
            }
            window.Closing += OnWindowClosing;
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            var window = (Window) sender;

            //http://stackoverflow.com/questions/847752/net-wpf-remember-window-size-between-sessions
            if (window.WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Properties.Settings.Default.Top = window.RestoreBounds.Top;
                Properties.Settings.Default.Left = window.RestoreBounds.Left;
                Properties.Settings.Default.Height = window.RestoreBounds.Height;
                Properties.Settings.Default.Width = window.RestoreBounds.Width;
                Properties.Settings.Default.Maximized = true;
            }
            else
            {
                Properties.Settings.Default.Top = window.Top;
                Properties.Settings.Default.Left = window.Left;
                Properties.Settings.Default.Height = window.Height;
                Properties.Settings.Default.Width = window.Width;
                Properties.Settings.Default.Maximized = false;
            }

            Properties.Settings.Default.Save();
        }
    }
}