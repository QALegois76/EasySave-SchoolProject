using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class GetSavingModeJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IModelViewJob _viewModel;
        private IJobMng _model;


        // constructor
        public GetSavingModeJobCommand(IJobMng model,IModelViewJob viewModel )
        {
            _viewModel = viewModel;
            _model = model;
        }

        public bool CanExecute(object parameter)
        {
            if (string.IsNullOrEmpty(_model.EditingJobName))
                return false;

            if (!_model.Jobs.ContainsKey(_model.EditingJobName))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _viewModel.FirePopMsgEvent(_model.Jobs[_model.EditingJobName].SavingMode.ToString());
        }
    }



}
