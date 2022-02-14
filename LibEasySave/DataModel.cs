using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave
{
    [Serializable]
    public class DataModel
    {
        // private member

        [JsonIgnore]
        private static DataModel _instance = new DataModel();

        [JsonProperty]
        private ICryptInfo _cryptInfo = new CryptInfo();


        // public accessor

        [JsonIgnore]
        public ICryptInfo CryptInfo => _cryptInfo;

        [JsonIgnore]
        public static DataModel Instance => _instance;

        // constructor
        private DataModel() { }


        public void Init()
        {
            var v = JsonConvert.DeserializeObject<DataModel>(global::LibEasySave.Res.Resource1.AppConfig);
            Copy(v);
        }

        private void Copy(DataModel src)
        {
            _instance._cryptInfo = src._cryptInfo;
        }

        public void TestWrite()
        {
            JSONText serializer = new JSONText();
            string fileContent = serializer.GetFormatingText(_instance);
            FileWriter.Write(fileContent, "AppConfig.json");
        }
        
    }

    public interface ICryptInfo
    {
        public string[] AllowEtx { get; }
        public ECryptMode CryptMode { get ; }

        public string Key { get; }

        public bool IsCryptedExt(string ext);

    }


    [Serializable]
    public class CryptInfo: ICryptInfo
    {
        [JsonProperty]
        private string[] _allowEtx =new string[1] {".txt"};
        [JsonProperty]
        private ECryptMode cryptMode = ECryptMode.XOR;
        [JsonProperty]
        private string _key = "681257479207131073";

        [JsonIgnore]
        public string[] AllowEtx { get => _allowEtx; set => _allowEtx = value; }
        [JsonIgnore]
        public ECryptMode CryptMode { get => cryptMode; set => cryptMode = value; }
        [JsonIgnore]
        public string Key { get => _key; set => _key = value; }

        public bool IsCryptedExt(string ext)
        {
            foreach (string allowEtx in _allowEtx)
            {
                if (ext == allowEtx)
                    return true;
            }
            return false;
        }
    }

    [Serializable]
    public enum ECryptMode
    {
        XOR,
    }
}
