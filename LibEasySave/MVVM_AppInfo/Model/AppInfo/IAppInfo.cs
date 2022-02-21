using LibEasySave.TranslaterSystem;
using System;
using System.ComponentModel;

namespace LibEasySave.AppInfo
{
    public interface IAppInfo : INotifyPropertyChanged
    {
        event EventHandler OnLangUpdate;

        ELangCode ActivLang { get;  set; }
        EModeIHM ModeIHM { get;  set; }
        string[] PriorityExt { get;  set;}
        string[] AllowExt { get;  set;}

        bool IsValid();
    }
}
