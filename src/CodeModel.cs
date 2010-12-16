using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Input;
using System.IO;
using ICSharpCode.NRefactory;

namespace STFUANDCODE
{
    public class CodeModel : INotifyPropertyChanged
    {
        private Brush _parseStatusVisual;
        private string _parseStatusText;
        private string _code;
        private string _compilationLog;
        private ICommand _stfuAndCodeCmd;

        public ICommand StfuAndRunCodeCommand
        {
            get { return _stfuAndCodeCmd ?? (_stfuAndCodeCmd = new StfuAndRunCodeCommand(this)); }
        }

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

        public string ParseStatusText
        {
            get { return _parseStatusText; }
            set
            {
                if (string.Equals(_parseStatusText, value)) return;
                _parseStatusText = value;
                RaisePropertyChanged("ParseStatusText");
            }
        }


        public Brush ParseStatusVisual
        {
            get { return _parseStatusVisual; }
            set
            {
                if (value == _parseStatusVisual)
                    return;
                _parseStatusVisual = value;
                RaisePropertyChanged("ParseStatusVisual");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CompilationLog
        {
            get { return _compilationLog; }
            set
            {
                _compilationLog = value;
                RaisePropertyChanged("CompilationLog");
            }
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
            string brushResource;
            if (parses)
            {
                statusText = "Parses";
                brushResource = "Parse";
            }
            else
            {
                statusText = "Doesn't Parse!";
                brushResource = "NoParse";
            }
            ParseStatusText = statusText;
            ParseStatusVisual = (Brush)App.Current.TryFindResource(brushResource);
        }

        private void RaisePropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
    }
}
