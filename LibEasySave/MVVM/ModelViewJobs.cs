using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace LibEasySave
{
    public class ModelViewJobs : INotifyPropertyChanged, IModelViewJob
    {
        #region VARIABLES
        // event 
        private event MsgSenderEventHandler _onPopingMsg;
        private event EventHandler _onEditing;

        event MsgSenderEventHandler IModelViewJob.OnPopingMsg { add => _onPopingMsg += value; remove => _onPopingMsg -= value; }
        event EventHandler IModelViewJob.OnEditing { add => _onEditing += value; remove => _onEditing -= value; }


        public event PropertyChangedEventHandler PropertyChanged;

        #region private & protected
        protected ICommand _addJobCommand;
        protected ICommand _removeJobCommand;
        protected ICommand _editJobCommand;
        protected ICommand _renameJobCommand;
        protected ICommand _setSrcRepJobCommand;
        protected ICommand _getSrcRepJobCommand;
        protected ICommand _setDestRepJobCommand;
        protected ICommand _getDestRepJobCommand;
        protected ICommand _setSavingModeJobCommand;
        protected ICommand _getSavingModeJobCommand;
        protected ICommand _getNameJobCommand;
        protected ICommand _runAllJobCommand;
        protected ICommand _runJobCommand;
        protected ICommand _getAllNameJobCommand;
        protected ICommand _exitJobCommand;

        protected IJobMng _model = null;
        #endregion
        // public


        public string EditingJobName => _model.EditingJobName;

        public string[] JobsName => (new List<string>(_model.Jobs.Keys)).ToArray();

        ICommand IModelViewJob.AddJobCommand => _addJobCommand;
        ICommand IModelViewJob.RemoveJobCommand => _removeJobCommand;
        ICommand IModelViewJob.EditJobCommand => _editJobCommand;
        ICommand IModelViewJob.RenameJobCommand => _renameJobCommand;
        ICommand IModelViewJob.SetSrcRepJobCommand => _setSrcRepJobCommand;
        ICommand IModelViewJob.GetSrcRepJobCommand => _getSrcRepJobCommand;
        ICommand IModelViewJob.SetDestRepJobCommand => _setDestRepJobCommand ;
        ICommand IModelViewJob.GetDestRepJobCommand => _getDestRepJobCommand;
        ICommand IModelViewJob.SetSavingModeJobCommand => _setSavingModeJobCommand;
        ICommand IModelViewJob.GetSavingModeJobCommand => _getSavingModeJobCommand;
        ICommand IModelViewJob.GetNameJobCommand => _getNameJobCommand;
        ICommand IModelViewJob.RunJobCommand => _runJobCommand;
        ICommand IModelViewJob.RunAllJobCommand => _runAllJobCommand;
        ICommand IModelViewJob.GetAllNameJobCommand => _getAllNameJobCommand;
        ICommand IModelViewJob.ExitJobCommand => _exitJobCommand;
        // internal
        #endregion

        // constructor
        public ModelViewJobs(IJobMng model)
        {
            _model = model;

            _editJobCommand = new EditJobCommand(_model, this);

            _addJobCommand = new AddJobCommand(_model);
            _renameJobCommand = new RenameJobCommand(_model);
            _removeJobCommand = new RemoveJobCommand(_model);

            _getAllNameJobCommand = new GetAllNameJobCommand(_model,this);
            _getDestRepJobCommand = new GetRepDestJobCommand(_model,this);
            _getSrcRepJobCommand = new GetRepSrcJobCommand(_model,this);
            _getSavingModeJobCommand = new GetSavingModeJobCommand(_model, this);

            _setDestRepJobCommand = new SetRepDestJobCommand(_model);
            _setSavingModeJobCommand = new SetSavingModeJobCommand(_model,this);
            _setSrcRepJobCommand = new SetRepSrcJobCommand(_model);

            _runAllJobCommand = new RunAllJob(_model, this);
            _runJobCommand = new RunCommand(_model, this);

            _exitJobCommand = new ExitJobCommand();
        }


        void IModelViewJob.FirePopMsgEvent(string msg, object param = null) => _onPopingMsg?.Invoke(this, new MsgEventArgs(msg, param));

        void IModelViewJob.FireEditingEvent() => _onEditing?.Invoke(this, EventArgs.Empty);
    }


    /// /////////////////////////////////////////////////////////////////////////////////////////


    public delegate void MsgSenderEventHandler(object sender, MsgEventArgs eMsg );

    public class MsgEventArgs : EventArgs
    {
        private string _msg = null;
        private object _param = null;

        public string Msg => _msg;
        public object Param => _param;

        public MsgEventArgs(string msg , object param = null)
        {
            _msg = msg;
            _param = param;
        }
    }

    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface IModelViewJob
    {
        // event
        public event MsgSenderEventHandler OnPopingMsg;
        public event EventHandler OnEditing;


        // prop
        public string EditingJobName { get; }
        public string[] JobsName { get; }

        public ICommand AddJobCommand { get; }
        public ICommand RemoveJobCommand { get; }
        public ICommand EditJobCommand { get; }
        public ICommand RenameJobCommand { get; }
        public ICommand SetSrcRepJobCommand { get; }
        public ICommand GetSrcRepJobCommand { get; }
        public ICommand SetDestRepJobCommand { get; }
        public ICommand GetDestRepJobCommand { get; }
        public ICommand SetSavingModeJobCommand { get; }
        public ICommand GetSavingModeJobCommand { get; }
        public ICommand GetNameJobCommand { get; }
        public ICommand RunJobCommand { get; }
        public ICommand RunAllJobCommand { get; }
        public ICommand GetAllNameJobCommand { get; }
        public ICommand ExitJobCommand { get; }


        //methods

        internal void FirePopMsgEvent(string msg, object param = null);
        internal void FireEditingEvent();



        

    }



}
