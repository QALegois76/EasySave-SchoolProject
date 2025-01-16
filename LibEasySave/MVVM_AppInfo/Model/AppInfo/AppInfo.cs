using LibEasySave.TranslaterSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LibEasySave.AppInfo
{

    [Serializable]
    public class AppInfo : ObservableObject, IAppInfo
    {
        //public event EventHandler OnLangUpdate;

        [JsonIgnore]
        private int _priorityFilerunningnumber = 0;

        #region private members
        [JsonProperty]
        private string _filterFileDialog = "Easy Save files (*.esv)|*.esv";
        [JsonProperty]
        private string _extensionFile = ".esv";

        [JsonProperty]
        private ELangCode _activeLang = ELangCode.EN;

        [JsonProperty]
        private EModeIHM _ihmMode = EModeIHM.Client;

        [JsonProperty]
        private List<string> _priorityExt = new List<string>();

        [JsonProperty]
        private List<string> _allowSaveExt = new List<string>();

        [JsonProperty]
        private List<string> _jobApps = new List<string>();
        #endregion

 
        [JsonIgnore]
        public string FilterFileDialog { get => _filterFileDialog; set { _filterFileDialog = value; PropChanged(nameof(FilterFileDialog)); } }
        [JsonIgnore]
        public string EasySaveFileExt { get => _extensionFile; set { _extensionFile = value; PropChanged(nameof(EasySaveFileExt)); } }
        [JsonIgnore]
        public ELangCode ActivLang { get => _activeLang; set { _activeLang = value;  PropChanged(nameof(ActivLang)); } }
        [JsonIgnore]
        public EModeIHM ModeIHM { get => _ihmMode; set { _ihmMode = value; PropChanged(nameof(ModeIHM)); } }
        [JsonIgnore]
        public List<string> PriorityExt { get => _priorityExt; set { _priorityExt = value; PropChanged(nameof(PriorityExt)); } }
        [JsonIgnore]
        public  List<string> AllowExt{ get => _allowSaveExt; set { _allowSaveExt = value; PropChanged(nameof(AllowExt)); } }
        [JsonIgnore]
        public List<string> JobApps { get => _jobApps; set { _jobApps = value; PropChanged(nameof(JobApps)); } }


        public bool IsValid()
        {
            if (!Enum.IsDefined(typeof(ELangCode), _activeLang))
                _activeLang = ELangCode.EN;
            
            if (!Enum.IsDefined(typeof(EModeIHM), _ihmMode))
                _ihmMode = EModeIHM.Client;

            return true;


        }

        public bool ContainsJobApp(string appName)
        {
            if (_jobApps == null || _jobApps.Count == 0)
                return false;

            foreach (var item in _jobApps)
            {
                if (Path.GetFileName(item).Split('.')[0] == appName)
                    return true;
            }

            return false;
        }


        public void IncrementPriorityFile()=>  this._priorityFilerunningnumber++;
       
        public void DecrementPriorityFile()=>  this._priorityFilerunningnumber--;
        
        public bool IsPriorityFileRunning() => this._priorityFilerunningnumber > 0;      
    }




}
