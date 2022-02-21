using LibEasySave.TranslaterSystem;
using Newtonsoft.Json;
using System;

namespace LibEasySave.AppInfo
{

    [Serializable]
    public class AppInfo : ObservableObject, IAppInfo
    {
        public event EventHandler OnLangUpdate;

        #region private members
        [JsonProperty]
        private ELangCode _activeLang = ELangCode.EN;

        [JsonProperty]
        private EModeIHM _ihmMode = EModeIHM.Client;

        [JsonProperty]
        private string[] _priorityExt;

        [JsonProperty]
        private string[] _allowSaveExt;
        #endregion

        [JsonIgnore]
        public ELangCode ActivLang { get => _activeLang; set { _activeLang = value; OnLangUpdate?.Invoke(this, EventArgs.Empty);  PropChanged(nameof(ActivLang)); } }
        [JsonIgnore]
        public EModeIHM ModeIHM { get => _ihmMode; set { _ihmMode = value; PropChanged(nameof(ModeIHM)); } }
        [JsonIgnore]
        public string[] PriorityExt { get => _priorityExt; set { _priorityExt = value; PropChanged(nameof(PriorityExt)); } }
        [JsonIgnore]
        public  string[] AllowExt{ get => _allowSaveExt; set { _allowSaveExt = value; PropChanged(nameof(AllowExt)); } }


        public bool IsValid()
        {
            if (!Enum.IsDefined(typeof(ELangCode), _activeLang))
                _activeLang = ELangCode.EN;
            
            if (!Enum.IsDefined(typeof(EModeIHM), _ihmMode))
                _ihmMode = EModeIHM.Client;

            return true;


        }

    }




}
