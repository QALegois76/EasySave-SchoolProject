using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace LibEasySave
{
    public class ModelViewJobs : INotifyPropertyChanged
    {
        #region VARIABLES
        public event PropertyChangedEventHandler PropertyChanged;

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

        protected string _activJob = null;

        protected Dictionary<string, IJob> _model = new Dictionary<string, IJob>();


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

        public string ActivJob => _activJob;

        #region public

        public string Name 
        {
            get => (_activJob == null) ? null : _model[_activJob].SourceFolder; 
            set 
            {
                if (_activJob == null)
                    return;

                _model[_activJob].Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }   
        
        public string SourceFolder 
        {
            get => (_activJob == null) ? null : _model[_activJob].SourceFolder; 
            set 
            {
                if (_activJob == null)
                    return;

                _model[_activJob].SourceFolder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceFolder)));
            }
        }     
        public string DestinationFolder 
        {
            get => (_activJob == null) ? null : _model[_activJob].DestinationFolder; 
            set 
            {
                if (_activJob == null)
                    return;

                _model[_activJob].DestinationFolder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DestinationFolder)));
            }
        }
            
        public ESavingMode? SavingMode 
        {
            get 
            {
                if (_activJob == null)
                    return null;
                else 
                   return _model[_activJob].SavingMode;
            }
            set 
            {
                if (_activJob == null || value == null)
                    return;

                _model[_activJob].SavingMode =value.Value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SavingMode)));
            }
        }


        #endregion
        #endregion

        // constructor
        public ModelViewJobs(Dictionary<string,IJob> jobs)
        {
            _model = jobs;

            _addJobCommand = new AddJobCommand(_model);

        }

    }



}
