using LibEasySave.TranslaterSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LibEasySave.AppInfo
{

    [Serializable]
    public class AppInfo : ObservableObject, IAppInfo
    {
        public event EventHandler OnLangUpdate;

        [JsonIgnore]
        private int _priorityFilerunningnumber = 0;

        #region private members
        [JsonProperty]
        private ELangCode _activeLang = ELangCode.EN;

        [JsonProperty]
        private EModeIHM _ihmMode = EModeIHM.Client;

        [JsonProperty]
        private List<string> _priorityExt = new List<string>();

        [JsonProperty]
        private List<string> _allowSaveExt = new List<string>();
        #endregion

        [JsonIgnore]
        public ELangCode ActivLang { get => _activeLang; set { _activeLang = value;  PropChanged(nameof(ActivLang)); } }
        [JsonIgnore]
        public EModeIHM ModeIHM { get => _ihmMode; set { _ihmMode = value; PropChanged(nameof(ModeIHM)); } }
        [JsonIgnore]
        public List<string> PriorityExt { get => _priorityExt; set { _priorityExt = value; PropChanged(nameof(PriorityExt)); } }
        [JsonIgnore]
        public  List<string> AllowExt{ get => _allowSaveExt; set { _allowSaveExt = value; PropChanged(nameof(AllowExt)); } }


        public bool IsValid()
        {
            if (!Enum.IsDefined(typeof(ELangCode), _activeLang))
                _activeLang = ELangCode.EN;
            
            if (!Enum.IsDefined(typeof(EModeIHM), _ihmMode))
                _ihmMode = EModeIHM.Client;

            return true;


        }

        public void IncrementPriorityFile()=>  this._priorityFilerunningnumber++;
       
        public void DecrementPriorityFile()=>  this._priorityFilerunningnumber--;
        
        public bool IsPriorityFileRunning() => this._priorityFilerunningnumber > 0;      
    }




}
