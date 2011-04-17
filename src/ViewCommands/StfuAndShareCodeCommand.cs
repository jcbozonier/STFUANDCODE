using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace STFUANDCODE.ViewCommands
{
  public class StfuAndShareCodeCommand : ICommand
  {
    private CodeModel _ViewModel;

    public StfuAndShareCodeCommand(CodeModel viewModel)
    {
      _ViewModel = viewModel;
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

      var location = GitHub.CreateGist(codeToShare);
      viewModel.SharedLocation = location;
    }
  }
}
