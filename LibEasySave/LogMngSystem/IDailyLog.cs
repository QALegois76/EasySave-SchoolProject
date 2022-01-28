using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave
{
    public interface IDailyLog
    {
        DateTime Time { get; }

        string JobName { get; }

        string PathFileSource { get; }
        string PathFileDestination { get; }
        string SizeFile { get; }
        int TimeSaving { get; set; }
    }

    [Serializable]
    public class DailyLog : IDailyLog
    {

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
        protected int _timeSaving = -1;

        [JsonIgnore]
        public DateTime Time => _time;

        [JsonIgnore]
        public string JobName => _jobName;

        [JsonIgnore]
        public string PathFileSource => _pathFileSrc;

        [JsonIgnore]
        public string PathFileDestination => _pathFileDest;

        [JsonIgnore]
        public string SizeFile => _sizeFile;

        [JsonIgnore]
        public int TimeSaving { get => _timeSaving; set => _timeSaving = value; }


        // constuctor
        public DailyLog(string jobName, string pathFileSrc, string pathFileDest, string sizeFile, int timeSaving = -1)
        {
            _time = DateTime.Now;
            _jobName = jobName;
            _pathFileSrc = pathFileSrc;
            _pathFileDest = pathFileDest;
            _sizeFile = sizeFile;
            _timeSaving = timeSaving;
        }

    }
}
