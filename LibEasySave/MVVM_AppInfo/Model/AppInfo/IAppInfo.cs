using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LibEasySave.AppInfo
{
    public interface IAppInfo : INotifyPropertyChanged
    {
         string FilterFileDialog { get; set; }
         string EasySaveFileExt { get; set; }
        ELangCode ActivLang { get;  set; }
        EModeIHM ModeIHM { get;  set; }
        List<string> PriorityExt { get;  set;}
        List<string> AllowExt { get;  set;}

        bool IsValid();

        void IncrementPriorityFile();
        void DecrementPriorityFile();
        bool IsPriorityFileRunning();
    }
}
