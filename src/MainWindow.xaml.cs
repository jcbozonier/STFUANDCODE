using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Highlighting;

namespace STFUANDCODE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel _vm = new ViewModel();

        public MainWindow()
        {
            InitializeComponent();
            new WindowSizeController(this); //lifetime controlled by window through event
            DataContext = _vm;
            InitializeEditor();
        }

        private void InitializeEditor()
        {
            Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            Editor.Text = Initial.Text;
        }

        private void OnEditorTextChanged(object sender, EventArgs e)
        {
            _vm.Code = Editor.Document.Text;
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}
