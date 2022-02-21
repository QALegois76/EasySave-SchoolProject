using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace LibEasySave
{
    public class ModelViewJobs : ObservableObject, INotifyPropertyChanged, IModelViewJob
    {
        #region VARIABLES
        // event 
        private const string HELP = "?";
        private const string ALL = "ALL";


        public event PropertyChangedEventHandler PropertyChanged;

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

        protected IJobMng _model = null;
        #endregion
        // public


        public Guid EditingJob => _model.EditingJob;
        string IModelViewJob.HELP => HELP;
        string IModelViewJob.ALL => ALL;


        public List<String> JobsName
        {
            get
            {

                List<String> output = new List<string>();
                foreach (var item in this._model.Jobs)
                {
                    output.Add(item.Value.Name);
                }
                return output;
            }
        }
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
        // teste
        ICommand IModelViewJob.CommandUnknownJobCommand => _commandUnknownJobCommand;
        ICommand IModelViewJob.ExitJobCommand => _exitJobCommand;
        ICommand IModelViewJob.OpenJobFile { get; }
        ICommand IModelViewJob.SaveJobFile { get; }
        ICommand IModelViewJob.EditSetting { get; }

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
    public interface IModelViewJob
    {
        // event
        public event MsgSenderEventHandler OnPopingMsgError;
        public event MsgSenderEventHandler OnPopingMsgInfo;
        public event GuidSenderEventHandler OnEditing;
        public event GuidSenderEventHandler OnAdding;
        public event GuidSenderEventHandler OnRemoving;
        public event GuidSenderEventHandler OnRunAll;



        // prop
        public string HELP {get;}
        public string ALL {get;}
        public Guid EditingJob { get; }
        public List<string> JobsName { get; }

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
        public ICommand CommandUnknownJobCommand { get; }
        public ICommand OpenJobFile { get; }
        public ICommand SaveJobFile { get; }
        public ICommand EditSetting { get; }



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
