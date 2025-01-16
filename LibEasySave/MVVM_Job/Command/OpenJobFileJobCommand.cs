using LibEasySave.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace LibEasySave
{
    public class OpenJobFileJobCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        IModelViewJob _modelView;

        public OpenJobFileJobCommand(IModelViewJob modelView)
        {
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is string))
                return false;

            if (!File.Exists(parameter.ToString()))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;
            string fileContent = File.ReadAllText(parameter.ToString());
            var temp = JSONDeserializer<List<Job>>.Deserialize(fileContent);

            if (temp == null)
                return;

            foreach (var item in _modelView.Model.BaseJober)
            {
                _modelView.RemoveJobCommand.Execute(item.Key);
            }

            foreach (var item in temp)
            {
                _modelView.AddJobCommand.Execute(item.Guid);
                _modelView.Model.BaseJober[item.Guid].Job.Name = item.Name;
                _modelView.Model.BaseJober[item.Guid].Job.SavingMode = item.SavingMode;
                _modelView.Model.BaseJober[item.Guid].Job.SourceFolder = item.SourceFolder;
                _modelView.Model.BaseJober[item.Guid].Job.DestinationFolder = item.DestinationFolder;
                _modelView.Model.BaseJober[item.Guid].Job.IsEncrypt = item.IsEncrypt;
            }

        }
    }


}
