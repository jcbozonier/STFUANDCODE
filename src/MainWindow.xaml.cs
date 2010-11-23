using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.ComponentModel;
using ICSharpCode.NRefactory;
using ICSharpCode.AvalonEdit.Highlighting;

namespace STFUANDCODE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
        }

        private void UpdateResult(string source)
        {
            using (var tr = new StringReader(source))
            using (var parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, tr))
            {
                parser.Parse();
                var parses = parser.Errors.Count == 0;
                SetParseStatus(parses);

                //var csVisitor = new CsVisitor();
                //parser.CompilationUnit.AcceptVisitor(csVisitor, null);
            }
        }

        private void SetParseStatus(bool parses)
        {
            var statusText = string.Empty;
            var statusBackgroundColor = Colors.Transparent;
            if (parses)
            {
                statusText = "Parses";
                statusBackgroundColor = Colors.Green;
            }
            else
            {
                statusText = "Doesn't Parse!";
                statusBackgroundColor = Colors.Red;
            }
            ParseStatusText = statusText;
            ParseStatusBackground = new SolidColorBrush(statusBackgroundColor);
        }

        private string _parseStatusText;
        public string ParseStatusText
        {
            get { return _parseStatusText; }
            set
            {
                if (string.Equals(_parseStatusText, value))
                    return;
                _parseStatusText = value;
                FirePropertyChanged("ParseStatusText");
            }
        }

        private Brush _parseStatusBackground;
        public Brush ParseStatusBackground
        {
            get { return _parseStatusBackground; }
            set
            {
                if (value == _parseStatusBackground)
                    return;
                _parseStatusBackground = value;
                FirePropertyChanged("ParseStatusBackground");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void FirePropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            var newSource = Editor.Document.Text;
            UpdateResult(newSource);
        }
    }
}
