using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace LibEasySave
{

    public class Job : IJob , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isEncrypt = false;
        protected string _name = null;
        protected string _repSrc = null;
        protected string _repDest = null;
        protected ESavingMode _savingMode = ESavingMode.FULL;
        private Guid _guid;

        public bool IsEncrypt { get => _isEncrypt; set => _isEncrypt = value; }

        public string Name { get => _name; set => _name = value; }
        public string DestinationFolder { get => _repDest; set { _repDest = value;  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DestinationFolder)));  } }
        public string SourceFolder { get => _repSrc; set { _repSrc = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceFolder))); } }
        public ESavingMode SavingMode { get => _savingMode; set { _savingMode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SavingMode))); } }
        public Guid Guid => _guid;
        


        //constructor
        public Job(string name , string repSrc = null, string repDest =null, ESavingMode savingMode = ESavingMode.FULL )
        {
            _guid = Guid.NewGuid();
            _name = name;
            _repDest = repDest;
            _repSrc = repSrc;
            _savingMode = savingMode;
        }

        public IJob Copy(bool isNew = true,string name = null )
        {
            Job output;
            output = new Job(null);
            output._name = (string.IsNullOrWhiteSpace(name))? _name : name;
            output._repDest = this._repDest;
            output._repSrc = this._repSrc;
            output._savingMode = this._savingMode;
            output._guid =(isNew)?Guid.NewGuid(): this._guid;
            output._isEncrypt = this._isEncrypt;
            return output;
        }
    }

    public interface IJob
    {
        public bool IsEncrypt { get; set; }
        string Name { get; set; }
        string DestinationFolder { get; set; }
        string SourceFolder { get; set; }
        ESavingMode SavingMode { get; set; }

        Guid Guid { get; }

        IJob Copy(bool isNew = true, string name = null);

    }

    public enum ESavingMode
    {
        FULL,
        DIFF
    }
}
