using LibEasySave.Model;
using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RunCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IJobMng _model;
        private IModelViewJob _modelView;

        public RunCommand(IJobMng model, IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string))
                return false;

            string name = parameter.ToString();

            if (string.IsNullOrEmpty(name))
                return false;

            if (!_model.Jobs.ContainsKey(name))
                return false;

            if (_model.Jobs[name] == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            if (JobSaverStrategy.Save(_model.Jobs[parameter.ToString()]))
            {
                _modelView.FirePopMsgEvent("Sucess : job " + parameter.ToString() + " done");
            }
            else
            {
                _modelView.FirePopMsgEvent("Fail : job " + parameter.ToString());
            }

        }
    }



}
