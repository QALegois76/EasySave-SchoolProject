using LibEasySave.AppInfo;
using LibEasySave.Network;
using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace LibEasySave
{
    public class ModelViewJobs : ObservableObject, IModelViewJob
    {
        #region VARIABLES
        // event 
        private const string HELP = "?";
        private const string ALL = "ALL";

        public event MsgSenderEventHandler OnPopingMsgInfo;
        public event MsgSenderEventHandler OnPopingMsgError;

        public event GuidSenderEventHandler OnEditing;
        public event GuidSenderEventHandler OnAdding;
        public event GuidSenderEventHandler OnRemoving;
        public event GuidSenderEventHandler OnRunAll;

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
        protected ICommand _commandUnknownJobCommand;
        protected ICommand _openJobFileJobCommand;
        protected ICommand _saveJobFileJobCommand;

        protected IJobMng _model = null;
        #endregion
        // public


        public Guid EditingJob => _model.EditingJob;
        string IModelViewJob.HELP => HELP;
        string IModelViewJob.ALL => ALL;

        public List<string> JobsName
        {
            get
            {

                List<string> output = new List<string>();
                foreach (var item in this._model.BaseJober)
                {
                    output.Add(item.Value.Job.Name);
                }
                return output;
            }
        }
        public ICommand AddJobCommand => _addJobCommand;
        public ICommand RemoveJobCommand => _removeJobCommand;
        public ICommand EditJobCommand => _editJobCommand;
        public ICommand RenameJobCommand => _renameJobCommand;
        public ICommand SetSrcRepJobCommand => _setSrcRepJobCommand;
        public ICommand GetSrcRepJobCommand => _getSrcRepJobCommand;
        public ICommand SetDestRepJobCommand => _setDestRepJobCommand ;
        public ICommand GetDestRepJobCommand => _getDestRepJobCommand;
        public ICommand SetSavingModeJobCommand => _setSavingModeJobCommand;
        public ICommand GetSavingModeJobCommand => _getSavingModeJobCommand;
        public ICommand GetNameJobCommand => _getNameJobCommand;
        public ICommand RunJobCommand => _runJobCommand;
        public ICommand RunAllJobCommand => _runAllJobCommand;
        public ICommand GetAllNameJobCommand => _getAllNameJobCommand;
        public ICommand CommandUnknownJobCommand => _commandUnknownJobCommand;
        public ICommand ExitJobCommand => _exitJobCommand;
        public ICommand OpenJobFile => _openJobFileJobCommand;
        public ICommand SaveJobFile => _saveJobFileJobCommand;
        //ICommand IModelViewJob.EditSetting { get; }

        public ELangCode LangCodeEN => ELangCode.EN;
        public ELangCode LangCodeFR => ELangCode.FR;

        public IJobMng Model => _model;
        // internal
        #endregion

        // constructor
        public ModelViewJobs(IJobMng model)
        {
            _model = model;

            _editJobCommand = new EditJobCommand(_model, this);

            _addJobCommand = new AddJobCommand(_model,this);
            _renameJobCommand = new RenameJobCommand(_model,this);
            _removeJobCommand = new RemoveJobCommand(_model,this);

            _getAllNameJobCommand = new GetAllNameJobCommand(_model,this);
            _getDestRepJobCommand = new GetRepDestJobCommand(_model,this);
            _getSrcRepJobCommand = new GetRepSrcJobCommand(_model,this);
            _getSavingModeJobCommand = new GetSavingModeJobCommand(_model, this);

            _setDestRepJobCommand = new SetRepDestJobCommand(_model,this);
            _setSavingModeJobCommand = new SetSavingModeJobCommand(_model,this);
            _setSrcRepJobCommand = new SetRepSrcJobCommand(_model,this);

            _runAllJobCommand = new RunAllJob(_model, this);
            _runJobCommand = new RunCommand(_model, this);

            _commandUnknownJobCommand = new UnknownJobCommand(this);

            _exitJobCommand = new ExitJobCommand();

            _openJobFileJobCommand = new OpenJobFileJobCommand(this);
            _saveJobFileJobCommand = new SaveJobFileJobCommand(_model);
    }

        void IModelViewJob.FirePopMsgEventInfo(string msg, object param = null) => OnPopingMsgInfo?.Invoke(this, new MsgEventArgs(msg, param));

        void IModelViewJob.FireEditingEvent(Guid g) => OnEditing?.Invoke(this, new GuidSenderEventArg(g) );
        void IModelViewJob.FireAddingEvent(Guid g) => OnAdding?.Invoke(this, new GuidSenderEventArg(g) );
        void IModelViewJob.FireRemovingEvent(Guid g) => OnRemoving?.Invoke(this, new GuidSenderEventArg(g) );
        void IModelViewJob.FireRunJobEvent(Guid g) => OnRunAll?.Invoke(this, new GuidSenderEventArg(g) );

        void IModelViewJob.FirePopMsgEventError(string msg, object param) => OnPopingMsgError?.Invoke(this, new MsgEventArgs(msg, param));
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

    public class GuidSenderEventArg : EventArgs
    {
        protected Guid _guid;

        public Guid Guid { get => _guid; set => _guid = value; }

        public GuidSenderEventArg(Guid guid)
        {
            _guid = guid;
        }
    }

    public delegate void GuidSenderEventHandler(object sender, GuidSenderEventArg e);



    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public interface IModelViewJob : INotifyPropertyChanged
    {
        // event
        event MsgSenderEventHandler OnPopingMsgError;
        event MsgSenderEventHandler OnPopingMsgInfo;
        event GuidSenderEventHandler OnEditing;
        event GuidSenderEventHandler OnAdding;
        event GuidSenderEventHandler OnRemoving;
        event GuidSenderEventHandler OnRunAll;



        
        string HELP {get;}
        string ALL {get;}

        Guid EditingJob { get; }
        List<string> JobsName { get; }

        ICommand AddJobCommand { get; }
        ICommand RemoveJobCommand { get; }
        ICommand EditJobCommand { get; }
        ICommand RenameJobCommand { get; }
        ICommand SetSrcRepJobCommand { get; }
        ICommand GetSrcRepJobCommand { get; }
        ICommand SetDestRepJobCommand { get; }
        ICommand GetDestRepJobCommand { get; }
        ICommand SetSavingModeJobCommand { get; }
        ICommand GetSavingModeJobCommand { get; }
        ICommand GetNameJobCommand { get; }
        ICommand RunJobCommand { get; }
        ICommand RunAllJobCommand { get; }
        ICommand GetAllNameJobCommand { get; }
        ICommand ExitJobCommand { get; }
        ICommand CommandUnknownJobCommand { get; }
        ICommand OpenJobFile { get; }
        ICommand SaveJobFile { get; }



        ELangCode LangCodeEN { get; }
        ELangCode LangCodeFR { get; }



        IJobMng Model { get; }
        //methods

        internal void FirePopMsgEventInfo(string msg, object param = null);
        internal void FirePopMsgEventError(string msg, object param = null);
        internal void FireEditingEvent(Guid g);
        internal void FireAddingEvent(Guid g);
        internal void FireRemovingEvent(Guid g);
        internal void FireRunJobEvent(Guid g);



        

    }



}
