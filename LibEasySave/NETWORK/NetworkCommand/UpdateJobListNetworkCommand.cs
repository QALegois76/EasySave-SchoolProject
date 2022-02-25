using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LibEasySave.Network
{
    public class UpdateJobListNetworkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        IJobMng _model;
        IModelViewJob _modelView;

        public UpdateJobListNetworkCommand(IJobMng model, IModelViewJob modelView)
        {
            _model = model;
            _modelView = modelView;
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is JArray))
                return false;

            if ((parameter as JArray).Count == 0)
                return false;

            if ((parameter as JArray).First.ToObject<Job>() == null)
                return false;

            //if (((parameter as JArray).First. as Job) == null)
            //    return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            var keys = _model.BaseJober.Keys;
            foreach (var k in keys)
            {
                RemoveJobCommand removeJob = new RemoveJobCommand(_model, _modelView);
                removeJob.Execute(k);
            }

            foreach (var item in (JArray)parameter)
            {
                AddJobCommand addJob = new AddJobCommand(_model, _modelView);

                var j = item.ToObject<Job>();
                 
                addJob.Execute(j.Guid);

                UpdateDataJobNetworkCommand updateDataJobNetworkCommand = new UpdateDataJobNetworkCommand(_model, _modelView);
                updateDataJobNetworkCommand.Execute(j);
            }


        }
    }
}
