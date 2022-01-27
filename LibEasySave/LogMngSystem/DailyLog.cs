using System;
using System.Text;

namespace LibEasySave
{

    public class DailyLog : IDailyLog
    {

        protected DateTime _time;
        protected string _jobName = null;
        protected string _pathFileSrc = null;
        protected string _pathFileDest = null;
        protected string _sizeFile = null;
        protected int _timeSaving = -1;


        public DateTime Time  => _time;

        public string JobName => _jobName;

        public string PathFileSource => _pathFileSrc;

        public string PathFileDestination =>_pathFileDest;

        public string SizeFile => _sizeFile;

        public int TimeSaving { get => _timeSaving; set => _timeSaving = value; }


        // constuctor
        public DailyLog(string jobName, string pathFileSrc, string pathFileDest, string sizeFile , int timeSaving = -1)
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
