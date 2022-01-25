using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class GetRepDestJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;


        private IModelViewJob _viewModel;


        // constructor
        public GetRepDestJobCommand(IModelViewJob viewModel)
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

            _viewModel.FireEvent(_viewModel.Jobs[_viewModel.ActivName].DestinationFolder);
        }
    }



}
