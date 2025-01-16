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
        [JsonIgnore]
        private static DataModel _instanceActivClient = new DataModel();

        [JsonIgnore]
        private bool _isClientLock = false;

        [JsonProperty]
        private ICryptInfo _cryptInfo = new CryptInfo();

        [JsonProperty]
        private IAppInfo _appInfo = new AppInfo();

        [JsonProperty]
        private ILogInfo _logInfo = new LogInfo();


 
        // public accessor
        [JsonIgnore]
        public bool IsClientLock { get => _isClientLock; set { _isClientLock = value; PropChanged(nameof(IsClientLock)); } }
        [JsonIgnore]
        public ICryptInfo CryptInfo => _cryptInfo;
        [JsonIgnore]
        public IAppInfo AppInfo => _appInfo;
        [JsonIgnore]
        public ILogInfo LogInfo => _logInfo;

        [JsonIgnore]
        public static IDataModel Instance => _instance;
        public static IDataModel InstanceActivClient => _instanceActivClient;


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
                if (!File.Exists(APP_INFO_FULL_NAME))
                    SaveAppInfo();

                var v = JsonConvert.DeserializeObject<DataModel>(File.ReadAllText(APP_INFO_FULL_NAME));
                Copy(v);
            }
            catch(Exception)
            {

            }

        }

        // use after deserialisation
        internal void Copy(DataModel src)
        {
            if (_instance.AppInfo.ModeIHM == EModeIHM.Client)
            {
                _instance._cryptInfo = src._cryptInfo;
                _instance.PropChanged(nameof(CryptInfo));
                _instance._appInfo = src._appInfo;
                _instance.PropChanged(nameof(AppInfo));
                _instance._logInfo = src._logInfo;
                _instance.PropChanged(nameof(LogInfo));
            }
            else
            {
                _instanceActivClient._cryptInfo = src._cryptInfo;
                _instanceActivClient.PropChanged(nameof(CryptInfo));
                _instanceActivClient._appInfo = src._appInfo;
                _instanceActivClient.PropChanged(nameof(AppInfo));
                _instanceActivClient._logInfo = src._logInfo;
                _instanceActivClient.PropChanged(nameof(LogInfo));
            }
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
        bool IsClientLock { get; set; }

        IAppInfo AppInfo { get; }
        ICryptInfo CryptInfo { get; }
        ILogInfo LogInfo { get; }

        public void Init();

        public void SaveAppInfo();

        public bool IsValid();
    }




}
