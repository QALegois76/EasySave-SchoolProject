using System;
using System.Windows.Input;

namespace LibEasySave
{
    public class RunAllJob : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IModelViewJob _modelView;

        public RunAllJob(IModelViewJob modelView)
        {
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (_modelView.Jobs.Count==0)
                return false;


            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            foreach (var item in _modelView.Jobs)
            {
                _modelView.RunJobCommand.Execute(item.Key);
            }
        }
    }



}
