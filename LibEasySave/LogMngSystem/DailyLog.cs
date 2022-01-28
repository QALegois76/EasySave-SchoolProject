﻿using Newtonsoft.Json;
using System;
using System.Text;

namespace LibEasySave
{

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
        protected long _sizeFile;
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

        public long SizeFile => _sizeFile;

        [JsonIgnore]
        public int TimeSaving { get => _timeSaving; set => _timeSaving = value; }


        // constuctor
        public DailyLog(string jobName, string pathFileSrc, string pathFileDest, long sizeFile , int timeSaving = -1)
        {
            _time = DateTime.Now;
            _jobName = jobName;
            _pathFileSrc = pathFileSrc;
            _pathFileDest = pathFileDest;
            _sizeFile = sizeFile;
            _timeSaving = timeSaving;
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
