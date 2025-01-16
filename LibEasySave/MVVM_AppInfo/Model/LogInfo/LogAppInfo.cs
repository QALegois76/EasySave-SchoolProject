using Newtonsoft.Json;
using System;
using System.IO;

namespace LibEasySave.AppInfo
{

    [Serializable]
    public class LogInfo: ObservableObject ,ILogInfo
    {
        [JsonProperty]
        private string _stateLogPath = string.Empty;
        [JsonProperty]
        private string _dailyLogInfo = string.Empty;
        [JsonProperty]
        private ESavingFormat _logSavingFormat = ESavingFormat.JSON;


        [JsonIgnore]
        public ESavingFormat SavingFormat { get => _logSavingFormat; set { _logSavingFormat = value; PropChanged(nameof(SavingFormat)); } }
        [JsonIgnore]
        public string StateLogPath { get => _stateLogPath; set { _stateLogPath = value; PropChanged(nameof(StateLogPath)); } }
        [JsonIgnore]
        public string DailyLogPath { get => _dailyLogInfo; set { _dailyLogInfo = value; PropChanged(nameof(DailyLogPath)); } }

        public bool IsValid()
        {
            if (!Directory.Exists(_stateLogPath))
                return false;

            if (!Directory.Exists(_dailyLogInfo))
                return false;

            if (!Enum.IsDefined(typeof(ESavingFormat), _logSavingFormat))
                _logSavingFormat = ESavingFormat.JSON;

            return true;
        }
    }




}
