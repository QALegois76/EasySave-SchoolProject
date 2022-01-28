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
        long SizeFile { get; }
        long TimeSaving { get; }
    }
}
