using Newtonsoft.Json;
using System;
using System.Text;

namespace LibEasySave
{
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
        protected long _timeSaving = -1;
        
        [JsonProperty]
        protected long _timeCrypted = -1;

        [JsonProperty]
        protected long _sizeFile;



        [JsonIgnore]
        public DateTime Time  => _time;
        
        [JsonIgnore]
        public string JobName => _jobName;
        
        [JsonIgnore]
        public string PathFileSource => _pathFileSrc;
        
        [JsonIgnore]
        public string PathFileDestination =>_pathFileDest;

        [JsonIgnore]
        public long TimeSaving { get => _timeSaving; }
        
        [JsonIgnore]
        public long TimeCrypted { get => _timeCrypted; }

        [JsonIgnore]
        public long SizeFile =>_sizeFile;


        // constuctor
        public DailyLog(string jobName, string pathFileSrc, string pathFileDest, long sizeFile , long timeSaving = -1, bool isCrypted = false)
        {
            _time = DateTime.Now;
            _jobName = jobName;
            _pathFileSrc = pathFileSrc;
            _pathFileDest = pathFileDest;
            _sizeFile = sizeFile;
            _timeSaving = timeSaving;
            _timeCrypted = (isCrypted)? timeSaving : -1;
            
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
            output.Append((_timeSaving == -1) ? "error" : ("  (" + _timeSaving + "ms)"));
            return output.ToString();
        }

    }
}
