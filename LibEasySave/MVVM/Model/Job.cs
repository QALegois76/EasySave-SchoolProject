using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace LibEasySave
{

    [Serializable]
    public class Job : IJob , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [JsonProperty]
        protected string _name = null;
        [JsonProperty]
        protected string _repSrc = null;
        [JsonProperty]
        protected string _repDest = null;
        [JsonProperty]
        protected ESavingMode _savingMode = ESavingMode.FULL;
        [JsonProperty]
        private Guid _guid;
        
        [JsonIgnore]
        public string Name { get => _name; set => _name = value; }
        [JsonIgnore]
        public string DestinationFolder { get => _repDest; set { _repDest = value;  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DestinationFolder)));  } }
        [JsonIgnore]
        public string SourceFolder { get => _repSrc; set { _repSrc = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceFolder))); } }
        [JsonIgnore]
        public ESavingMode SavingMode { get => _savingMode; set { _savingMode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SavingMode))); } }
        [JsonIgnore]
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
            return output;
        }
    }

    public interface IJob
    {
        string Name { get; set; }
        string DestinationFolder { get; set; }
        string SourceFolder { get; set; }
        ESavingMode SavingMode { get; set; }

        Guid Guid { get; }

        IJob Copy(bool isNew = true, string name = null);

    }

    [Serializable]
    public enum ESavingMode
    {
        FULL,
        DIFF
    }
}
