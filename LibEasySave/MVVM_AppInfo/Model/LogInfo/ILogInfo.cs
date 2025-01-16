using System.ComponentModel;

namespace LibEasySave.AppInfo
{
    public interface ILogInfo : INotifyPropertyChanged
    {
        ESavingFormat SavingFormat { get; set; }
        string StateLogPath { get;  set; }
        string DailyLogPath { get;  set; }

        bool IsValid();
    }




}
