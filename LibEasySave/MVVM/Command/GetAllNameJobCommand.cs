using System;
using System.Text;
using System.Windows.Input;

namespace LibEasySave
{
    public class GetAllNameJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IJobMng _model;
        private IModelViewJob _modelView;

        public GetAllNameJobCommand(IJobMng model, IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            foreach ( var item in _model.Jobs)
            {
                _modelView.FirePopMsgEvent(item.Value.Name);
            }

        }
    }
}
