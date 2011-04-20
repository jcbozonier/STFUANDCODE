using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Highlighting;

namespace STFUANDCODE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CodeModel _vm = new CodeModel();

        public MainWindow()
        {
            InitializeComponent();
            new WindowSizeController(this); //lifetime controlled by window through event
            DataContext = _vm;
            InitializeEditor();
            CheckForAutoUpdate();
        }

        private void InitializeEditor()
        {
            Editor.FontFamily = new FontFamily("Consolas,Courier New");
            Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            Editor.Text = Initial.Text;
            Editor.CaretOffset = 136;
        }

        private void OnEditorTextChanged(object sender, EventArgs e)
        {
            _vm.Code = Editor.Document.Text;
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void ToggleButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            var toggleButton = ((ToggleButton) sender);
            Editor.ShowLineNumbers = toggleButton.IsChecked.HasValue && toggleButton.IsChecked.Value;
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var i = 0;
        }

        private void CheckForAutoUpdate()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += CheckForAutoUpdate_DoWork;
            bw.RunWorkerAsync();
        }

        const string CurrentVersionNumberUrl = "https://github.com/downloads/jcbozonier/STFUANDCODE/CurrentVersion.txt";
        private void CheckForAutoUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            var wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CurrentVersionNumber_DownloadCompleted);
            wc.DownloadStringAsync(new Uri(CurrentVersionNumberUrl + "?" + Guid.NewGuid().ToString()));
        }

        private const string CurrentVersionUrl = "https://github.com/downloads/jcbozonier/STFUANDCODE/STFU and Code Setup.exe";
        void CurrentVersionNumber_DownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                return;

            var versionString = e.Result;
            var availableVersion = new Version(versionString);
            var thisVersion = Assembly.GetExecutingAssembly().GetName().Version;
            if (availableVersion <= thisVersion)
                return;

            if (MessageBoxResult.Yes != MessageBox.Show("A new version is available.  Would you like to install it now?", "Install New Version?",
                            MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes))
                return;

            var wc = new WebClient();
            wc.DownloadDataCompleted += new DownloadDataCompletedEventHandler(wc_DownloadDataCompleted);
            wc.DownloadDataAsync(new Uri(CurrentVersionUrl + "?" + Guid.NewGuid().ToString()));
        }

        void wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }

            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, "STFU and Code Setup.exe");
            if (File.Exists(tempFile))
                File.Delete(tempFile);
            File.WriteAllBytes(tempFile, e.Result);
            Process.Start(tempFile);
            Dispatcher.Invoke(new Action(Close));
        }
    }

}
