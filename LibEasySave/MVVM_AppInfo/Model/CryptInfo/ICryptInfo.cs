using System.Collections.Generic;
using System.ComponentModel;

namespace LibEasySave.AppInfo
{
    public interface ICryptInfo : INotifyPropertyChanged
    {
        List<string> AllowEtx { get;  set; }
        ECryptMode CryptMode { get;  set; }

        string Key { get;  set; }

        bool IsCryptedExt(string ext);

        bool IsValid();
    }




}
