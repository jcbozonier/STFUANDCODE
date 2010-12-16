using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Input;
using System.IO;
using ICSharpCode.NRefactory;

namespace STFUANDCODE
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string compilationLog;
        private ICommand stfuAndCodeCmd;

        public ICommand StfuAndRunCodeCommand
        {
            get { return stfuAndCodeCmd ?? (stfuAndCodeCmd = new StfuAndRunCodeCommand(this)); }
        }

        private string _code;

        public string Code
        {
            get { return _code; }
            set
            {
                if (string.Equals(_code, value))
                    return;
                _code = value;
                Parse();
            }
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

        private void Parse()
        {
            using (var tr = new StringReader(Code))
            using (var parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, tr))
            {
                parser.Parse();
                var parses = parser.Errors.Count == 0;
                SetParseStatus(parses);
            }
        }

        private void SetParseStatus(bool parses)
        {
            string statusText;
            Color statusBackgroundColor;
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

        public string CompilationLog
        {
            get { return compilationLog; }
            set
            {
                compilationLog = value;
                FirePropertyChanged("CompilationLog");
            }
        }

        
    }
}
