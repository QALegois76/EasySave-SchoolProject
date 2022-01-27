using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Text;

namespace LibEasySave
{

    public interface ILog
    {
        DateTime Time { get; }

        string JobName { get; }

        string PathFileSource { get; }
        string PathFileDestination { get; }
        string SizeFile { get; }
        EDisplayMode DisplayMode { get; set; }
        int TimeSaving { get; set; }
    }

    [Serializable]
    public class Log : ILog
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty]
        protected DateTime _time;
        [JsonProperty]
        protected string _jobName = null;
        [JsonProperty]
        protected string _pathFileSrc = null;
        [JsonProperty]
        protected string _pathFileDest = null;
        [JsonProperty]
        protected string _sizeFile = null;
        [JsonProperty]
        protected EDisplayMode _displayMode = EDisplayMode.JSON;
        [JsonProperty]
        protected int _timeSaving = -1;

        [JsonIgnore]
        public DateTime Time  => _time;
        
        [JsonIgnore]
        public string JobName => _jobName;
        
        [JsonIgnore]
        public string PathFileSource => _pathFileSrc;
        
        [JsonIgnore]
        public string PathFileDestination =>_pathFileDest;
        
        [JsonIgnore]
        public string SizeFile => _sizeFile;

        [JsonIgnore]
        public EDisplayMode DisplayMode { get => _displayMode; set { _displayMode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayMode))); } }

        [JsonIgnore]
        public int TimeSaving { get => _timeSaving; set => _timeSaving = value; }


        // constuctor
        public Log(string jobName, string pathFileSrc, string pathFileDest, string sizeFile , EDisplayMode displayMode = EDisplayMode.JSON , int timeSaving = -1)
        {
            _time = DateTime.Now;
            _jobName = jobName;
            _pathFileSrc = pathFileSrc;
            _pathFileDest = pathFileDest;
            _sizeFile = sizeFile;
            _timeSaving = timeSaving;
            _displayMode = displayMode;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder("");
            output.Append(_time.ToString());
            output.Append(" : ");
            output.Append(_pathFileSrc);
            output.Append(" save  to ");
            output.Append(_pathFileDest);
            output.Append("  | size : " + _sizeFile);
            output.Append("  displayMode : " + _displayMode);
            output.Append((_timeSaving == -1) ? "error" : ("  (" + _timeSaving + "ms)"));
            return output.ToString();
        }

    }

    public enum EDisplayMode
    {
        JSON,
        XML
    }
}
