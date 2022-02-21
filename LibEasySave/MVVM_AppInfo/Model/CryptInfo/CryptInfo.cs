using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LibEasySave.AppInfo
{


    [Serializable]
    public class CryptInfo : ObservableObject, ICryptInfo
    {
        // private members
        [JsonProperty]
        private List<string> _allowEtx = new List<string>();
        [JsonProperty]
        private ECryptMode cryptMode = ECryptMode.XOR;
        [JsonProperty]
        private string _key = "681257479207131073";


        // public attributes
        [JsonIgnore]
        public List<string> AllowEtx { get => _allowEtx; set { _allowEtx = value; PropChanged(nameof(AllowEtx)); } }
        [JsonIgnore]
        public ECryptMode CryptMode { get => cryptMode; set { cryptMode = value; PropChanged(nameof(CryptMode)); } }
        [JsonIgnore]
        public string Key { get => _key; set { _key = value; PropChanged(nameof(Key)); } }



        // method
        public bool IsCryptedExt(string ext)
        {
            foreach (string allowEtx in _allowEtx)
            {
                if (ext == allowEtx)
                    return true;
            }
            return false;
        }

        public bool IsValid()
        {
            if (!Enum.IsDefined(typeof(ECryptMode), cryptMode))
                cryptMode = ECryptMode.XOR;

            return true;
        }
    }




}
