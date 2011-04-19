using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;

namespace STFUANDCODE.ViewCommands
{
  public class StfuAndShareCodeCommand : ICommand
  {
    private CodeModel _ViewModel;
    private Dispatcher _UiDispatcher;

    public StfuAndShareCodeCommand(CodeModel viewModel)
    {
      _ViewModel = viewModel;
      _UiDispatcher = Dispatcher.CurrentDispatcher;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
      var viewModel = _ViewModel;
      var codeToShare = viewModel.Code;

      viewModel.SharedLocation = "Saving...";

      ThreadPool.QueueUserWorkItem(x=>{
        var location = GitHub.CreateGist(codeToShare);
        _UiDispatcher.Invoke((Action)(()=>viewModel.SharedLocation = location));
      });
    }
  }
}
