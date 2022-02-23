using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LibEasySave.Model.LogMng.Interface
{
    public interface IStateLog : INotifyPropertyChanged
    {
        string JobName { get; }
        DateTime Time { get; }
        Guid Guid { get; }
        EJobState JobState { get; }
    }
}

