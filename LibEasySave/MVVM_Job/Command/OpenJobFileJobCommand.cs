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
                _modelView.Model.EditingJob = item.Guid;
                SetRepDestJobCommand setRepDestJobCommand = new SetRepDestJobCommand(_modelView.Model, _modelView);
                setRepDestJobCommand.Execute(item.DestinationFolder);

                SetRepSrcJobCommand setRepSrcJobCommand = new SetRepSrcJobCommand(_modelView.Model, _modelView);
                setRepSrcJobCommand.Execute(item.SourceFolder);

                SetSavingModeJobCommand setSavingModeJobCommand = new SetSavingModeJobCommand(_modelView.Model, _modelView);
                setSavingModeJobCommand.Execute(item.SavingMode);

                _modelView.Model.BaseJober[item.Guid].Job.IsEncrypt = item.IsEncrypt;
            }

        }
    }


}
