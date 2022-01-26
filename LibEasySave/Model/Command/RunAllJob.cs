using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RunAllJob : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IJobMng _model;
        private IModelViewJob _modelView;

        public RunAllJob(IJobMng model , IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (_model.Jobs.Count==0)
                return false;


            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            foreach (var item in _model.Jobs)
            {
                _modelView.RunJobCommand.Execute(item.Key);
            }
        }
    }



}
