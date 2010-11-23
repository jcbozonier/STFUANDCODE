using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;

namespace STFUANDCODE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += (sndr, evt) =>
            {
                var editor = new MainWindow();
                //var watcher = new FileSystemWatcher();
                //watcher.Created += (sndr, evt) => { editor. };
                //watcher.Changed += (sndr, evt) => { };

                editor.Show();
            };
        }
    }
}
