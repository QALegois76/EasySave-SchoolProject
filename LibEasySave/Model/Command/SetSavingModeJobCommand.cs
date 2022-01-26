using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class SetSavingModeJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private const string HELP = "?";

        private IJobMng _model;
        private IModelViewJob _modelView;

        public SetSavingModeJobCommand(IJobMng model, IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter.ToString() == HELP)
                return true;

            ESavingMode mode;
            if (!Enum.TryParse(parameter.ToString().Trim().ToUpper(),out mode))
                return false;

            if (!_model.Jobs.ContainsKey(_model.EditingJobName))
                return false;

            if (_model.Jobs[_model.EditingJobName] == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            if (parameter.ToString()==HELP)
            {
                foreach (ESavingMode eSavingMode in (ESavingMode[])Enum.GetValues(typeof(ESavingMode)))
                {
                    _modelView.FirePopMsgEvent(eSavingMode.ToString());
                }
            }
            else
                _model.Jobs[_model.EditingJobName].SavingMode = (ESavingMode)Enum.Parse(typeof(ESavingMode), parameter.ToString().Trim().ToUpper());
        }
    }



}
