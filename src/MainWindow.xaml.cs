using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.Highlighting;

namespace STFUANDCODE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel _vm = new ViewModel();

        public static readonly DependencyProperty STFUAndRunCodeCommandProperty = DependencyProperty.Register(
            "STFUAndRunCodeCommand", typeof(ICommand), typeof(MainWindow), null);

        public MainWindow()
        {
            InitializeComponent();
            InitializeWindowSize();
            Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
            DataContext = _vm;

            SetBinding(STFUAndRunCodeCommandProperty, new Binding("STFUAndRunCodeCommand") { Mode = BindingMode.OneWay });
            InputBindings.Add(new KeyBinding(STFUAndRunCodeCommand, new KeyGesture(Key.F5)));

            InitializeEditor();
        }

        public ICommand STFUAndRunCodeCommand
        {
            get { return (ICommand)GetValue(STFUAndRunCodeCommandProperty); }
            set { SetValue(STFUAndRunCodeCommandProperty, value); }
        }

        private void InitializeEditor()
        {
            Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            this.Editor.Text =
@"using System;

public class Foo
{
    public static void Main(params string[] args)
    {
        // STFU and Code Here!
        
    }
}";
            Editor.CaretOffset = 137;
        }

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            _vm.Code = Editor.Document.Text;
        }

        private void InitializeWindowSize()
        {
            //http://stackoverflow.com/questions/847752/net-wpf-remember-window-size-between-sessions
            this.Top = Properties.Settings.Default.Top;
            this.Left = Properties.Settings.Default.Left;
            this.Height = Properties.Settings.Default.Height;
            this.Width = Properties.Settings.Default.Width;
            // Very quick and dirty - but it does the job
            if (Properties.Settings.Default.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //http://stackoverflow.com/questions/847752/net-wpf-remember-window-size-between-sessions
            if (WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Properties.Settings.Default.Top = RestoreBounds.Top;
                Properties.Settings.Default.Left = RestoreBounds.Left;
                Properties.Settings.Default.Height = RestoreBounds.Height;
                Properties.Settings.Default.Width = RestoreBounds.Width;
                Properties.Settings.Default.Maximized = true;
            }
            else
            {
                Properties.Settings.Default.Top = this.Top;
                Properties.Settings.Default.Left = this.Left;
                Properties.Settings.Default.Height = this.Height;
                Properties.Settings.Default.Width = this.Width;
                Properties.Settings.Default.Maximized = false;
            }

            Properties.Settings.Default.Save();
        }
    }
}
