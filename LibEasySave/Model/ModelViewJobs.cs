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
        event MsgSenderEventHandler OnPopingMsg;

        public event PropertyChangedEventHandler PropertyChanged;

        // private & protected
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

        protected string _activJob = null;

        protected Dictionary<string, IJob> _model = new Dictionary<string, IJob>();
        // public

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



        // internal

        internal Dictionary<string, IJob> Jobs { get => _model; }

        IJob IModelViewJob.CurrentJob => (_activJob == null || !_model.ContainsKey(_activJob)) ? null : _model[_activJob];

        string IModelViewJob.ActivName { get => _activJob; set => _activJob = value; }

        Dictionary<string, IJob> IModelViewJob.Jobs => _model;


        #endregion

        // constructor
        public ModelViewJobs(Dictionary<string,IJob> model)
        {
            _model = model;

            _addJobCommand = new AddJobCommand(this);
            _getAllNameJobCommand = new GetAllNameJobCommand(this);
            _getDestRepJobCommand = new GetRepDestJobCommand(this);
            _getSrcRepJobCommand = new GetRepSrcJobCommand(this);
            _editJobCommand = new EditJobCommand(this);
            _renameJobCommand = new RenameJobCommand(this);
            _removeJobCommand = new RemoveJobCommand(this);
            _setDestRepJobCommand = new SetRepDestJobCommand(this);
            _setSavingModeJobCommand = new SetSavingModeJobCommand(this);
            _setSrcRepJobCommand = new SetRepSrcJobCommand(this);
            _runAllJobCommand = new RunAllJob(this);
            _runJobCommand = new RunCommand(this);
            
        }


        public void FireEvent(string msg, object param = null) => OnPopingMsg?.Invoke(this, new MsgEventArgs(msg, param));
    }


    /// /////////////////////////////////////////////////////////////////////////////////////////


    public delegate void MsgSenderEventHandler(object sender, MsgEventArgs eMsg );

    public class MsgEventArgs : EventArgs
    {
        string _msg = null;
        object _param = null;

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

        public IJob CurrentJob { get; }



        public void FireEvent(string msg, object param = null);


        public string ActivName { get; internal set; }
        internal Dictionary<string, IJob> Jobs { get; }

        

    }



}
