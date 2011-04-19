using System;
using System.Windows;
using System.Windows.Controls.Primitives;
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
        private readonly CodeModel _vm = new CodeModel();

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
    }

}
