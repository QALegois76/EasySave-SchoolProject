using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class GetSavingModeJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private IModelViewJob _viewModel;


        // constructor
        public GetSavingModeJobCommand(IModelViewJob viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (string.IsNullOrEmpty(_viewModel.ActivName))
                return false;

            if (!_viewModel.Jobs.ContainsKey(_viewModel.ActivName))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _viewModel.FireEvent(_viewModel.Jobs[_viewModel.ActivName].SourceFolder);
        }
    }



}
