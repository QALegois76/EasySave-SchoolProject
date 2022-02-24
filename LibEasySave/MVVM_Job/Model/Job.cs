using LibEasySave.AppInfo;
using LibEasySave.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;


namespace LibEasySave
{

    [Serializable]
    public class Job : ObservableObject, IJob
    {
        [JsonProperty]
        private bool _isEncrypt = false;
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
        public bool IsEncrypt { get => _isEncrypt; set { _isEncrypt = value; FireEvent(nameof(IsEncrypt)); } }
    [JsonIgnore]
        public string Name { get => _name; set { _name = value; FireEvent(nameof(Name)); } }
        [JsonIgnore]
        public string DestinationFolder { get => _repDest; set { _repDest = value; FireEvent(nameof(DestinationFolder));  } }
        [JsonIgnore]
        public string SourceFolder { get => _repSrc; set { _repSrc = value; FireEvent(nameof(SourceFolder)); } }
        [JsonIgnore]
        public ESavingMode SavingMode { get => _savingMode; set { _savingMode = value; FireEvent(nameof(SavingMode)); } }
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


        public IJob Copy(string name = null ,Guid? guid = null)
        {
            Job output;
            output = new Job(null);
            output._name = (string.IsNullOrWhiteSpace(name))? _name : name;
            output._repDest = this._repDest;
            output._repSrc = this._repSrc;
            output._savingMode = this._savingMode;
            output._guid =(!guid.HasValue)?Guid.NewGuid(): guid.Value;
            output._isEncrypt = this._isEncrypt;
            return output;
        }

        private void FireEvent(string name  = null)
         {
            this.PropChanged(name);
            NetworkMng.Instance.SendNetworkCommad(ENetorkCommand.UpdateJobData, this);
        }
    }

    public interface IJob : INotifyPropertyChanged
    {
        public bool IsEncrypt { get; set; }
        string Name { get; set; }
        string DestinationFolder { get; set; }
        string SourceFolder { get; set; }
        ESavingMode SavingMode { get; set; }

        Guid Guid { get; }

        IJob Copy(string name = null, Guid? guid = null);

    }

    [Serializable]
    public enum ESavingMode
    {
        FULL,
        DIFF
    }
}
