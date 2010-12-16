using System.Windows;

namespace STFUANDCODE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += (sndr, evt) =>
            {
                var editor = new MainWindow();
                editor.Show();
            };
        }
    }
}
