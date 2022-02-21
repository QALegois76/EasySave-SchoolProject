using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace LibEasySave
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void PropChanged(/*[System.Runtime.CompilerServices.CallerMemberName]*/ string propName = "")
        {
            PropertyChanged?.Invoke(this ,new PropertyChangedEventArgs(propName));
        }
    }
}
