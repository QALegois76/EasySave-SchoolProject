using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.Network
{

    [Serializable]
    public class NetworkInfo
    {
        [JsonProperty]
        private object _parameter;
        [JsonProperty]
        private ENetorkCommand _command = ENetorkCommand.Unknown;


        public NetworkInfo(ENetorkCommand command, object parameter)
        {
            _command = command;
            _parameter = parameter;
        }

        [JsonIgnore]
        public ENetorkCommand Command { get => _command; set => _command = value; }
        [JsonIgnore]
        public object Parameter { get => _parameter; set => _parameter = value; }
    }

    public enum ENetorkCommand
    {
        Unknown,

        AddJob,
        RemoveJob,
        UpdateJobData,
        UpdateJobList,
        UpdateJobProgress,
        RunJobs,
        UpdateDataModel,
        LockUIClient,
    }
}


