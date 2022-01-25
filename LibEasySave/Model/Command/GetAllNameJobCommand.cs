using System;
using System.Text;
using System.Windows.Input;

namespace LibEasySave
{
    public class GetAllNameJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IModelViewJob _viewModel;

        public GetAllNameJobCommand(IModelViewJob modelView)
        {
            _viewModel = modelView;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            StringBuilder output = new StringBuilder("");

            foreach ( var item in _viewModel.Jobs)
            {
                output.AppendLine(item.Value.Name);
            }

            _viewModel.FireEvent(output.ToString());
        }
    }
}
