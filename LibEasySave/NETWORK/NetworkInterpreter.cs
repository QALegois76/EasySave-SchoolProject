using LibEasySave.AppInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.Network
{
    class NetworkInterpreter
    {
        private IModelViewJob _modelViewJob;
        private IViewDataModel _modelViewDataModel;

        public NetworkInterpreter(IModelViewJob modelViewJob , IViewDataModel modelViewDataModel )
        {
            this._modelViewJob = modelViewJob;
            this._modelViewDataModel = modelViewDataModel;
        }


        public void Interprete(NetworkInfo networkInfo)
        {
            switch (networkInfo.Command)
            {
                case ENetorkCommand.Unknown:
                    break;

                case ENetorkCommand.Update:
                    break;

                case ENetorkCommand.Add:
                    _modelViewJob.AddJobCommand.Execute(networkInfo.Parameter);
                    break;


                default:
                    break;
            }
            // do stuff
        }


    }
}
