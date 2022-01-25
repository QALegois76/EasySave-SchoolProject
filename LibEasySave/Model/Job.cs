using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace LibEasySave
{
    public interface IJob
    {
        string Name { get; set; }
        string DestinationFolder { get; set; }
        string SourceFolder { get; set; }
        ESavingMode SavingMode { get; set; }

    }

    public class Job : IJob , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected string _name = null;
        protected string _repSrc = null;
        protected string _repDest = null;
        protected ESavingMode _savingMode = ESavingMode.Full;

        public string Name { get => _name; set => _name = value; }
        public string DestinationFolder { get => _repDest; set { _repDest = value;  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DestinationFolder)));  } }
        public string SourceFolder { get => _repSrc; set { _repSrc = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceFolder))); } }
        public ESavingMode SavingMode { get => _savingMode; set { _savingMode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SavingMode))); } }


        //constructor
        public Job(string name , string repSrc = null, string repDest =null, ESavingMode savingMode = ESavingMode.Full )
        {
            _name = name;
            _repDest = repDest;
            _repSrc = repSrc;
            _savingMode = savingMode;
        }

    }

    public enum ESavingMode
    {
        Full,
        Diferential
    }
}
