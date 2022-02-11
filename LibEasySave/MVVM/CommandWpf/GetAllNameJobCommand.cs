using LibEasySave.TranslaterSystem;
using System;
using System.Text;
using System.Windows.Input;

namespace LibEasySave
{
    public class GetAllNameJobCommandUI : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IJobMng _model;
        private IModelViewJob _modelView;

        public GetAllNameJobCommandUI(IJobMng model, IModelViewJob modelView)
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
                _modelView.FirePopMsgEventInfo(item.Value.Name);
            }

        }
    }
}
