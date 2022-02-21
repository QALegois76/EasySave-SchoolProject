using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace LibEasySave.AppInfo
{
    [Serializable]
    public class DataModel : ObservableObject, IDataModel
    {
        private const string APP_INFO_FOLDER_NAME = "EasySaveData";
        private const string APP_INFO_FILE_NAME = "AppInfo.json";
        private readonly string APP_INFO_FULL_NAME = Path.Combine(new string[3] { Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APP_INFO_FOLDER_NAME, APP_INFO_FILE_NAME });


        // private member

        [JsonIgnore]
        private static DataModel _instance = new DataModel();


        [JsonProperty]
        private ICryptInfo _cryptInfo = new CryptInfo();

        [JsonProperty]
        private IAppInfo _appInfo = new AppInfo();

        [JsonProperty]
        private ILogInfo _logInfo = new LogInfo();


 
        // public accessor

        [JsonIgnore]
        public ICryptInfo CryptInfo => _cryptInfo;
        [JsonIgnore]
        public IAppInfo AppInfo => _appInfo;
        [JsonIgnore]
        public ILogInfo LogInfo => _logInfo;

        [JsonIgnore]
        public static IDataModel Instance => _instance;


        // constructor
        private DataModel() 
        {
        }


        public void Init()
        {
            if (!Directory.Exists(Path.GetDirectoryName(APP_INFO_FULL_NAME)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(APP_INFO_FULL_NAME));
            }
            try
            {
                var v = JsonConvert.DeserializeObject<DataModel>(File.ReadAllText(APP_INFO_FULL_NAME));
                Copy(v);
            }
            catch(Exception)
            {

            }

        }

        // use after deserialisation
        private void Copy(DataModel src)
        {
            _instance._cryptInfo = src._cryptInfo;
            _instance._appInfo = src._appInfo;
            _instance._logInfo = src._logInfo;
        }

        public void SaveAppInfo() => FileSaverStrategy.Save(_instance, APP_INFO_FULL_NAME, true, ESavingFormat.JSON);



        public bool IsValid()
        {
            if (!_cryptInfo.IsValid())
                return false;
            
            if (!_appInfo.IsValid())
                return false;
            
            if (!_logInfo.IsValid())
                return false;

            return true;
        }



        // teste for debug
        public void TestWrite()
        {
            JSONText serializer = new JSONText();
            string fileContent = serializer.GetFormatingText(_instance);
            FileWriter.Write(fileContent, "AppConfig.json");
        }


    }

    public interface IDataModel : INotifyPropertyChanged
    {
        IAppInfo AppInfo { get; }
        ICryptInfo CryptInfo { get; }
        ILogInfo LogInfo { get; }

        public void Init();

        public void SaveAppInfo();

        public bool IsValid();
    }




}
