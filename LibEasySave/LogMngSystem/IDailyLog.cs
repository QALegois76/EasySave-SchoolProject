using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave
{
    interface IDailyLog
    {
        public string JobName
        {
            get { return JobName; }
            set { JobName = value; }
        }
        public DateTime Time
        {
            get { return Time; }
            set { Time = value; }
        }
        public string PathFileSrc
        {
            get { return PathFileSrc; }
            set {; }
        }
        public string PathFileDest
        {
            get { return PathFileDest; }
            set {; }
        }
        public string SizeFile
        {
            get { return SizeFile; }
            set {; }
        }
        public long TimeElapsed
        {
            get { return TimeElapsed; }
            set {; }
        }
    }
}
