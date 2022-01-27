using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.Model
{
    public abstract class LogBaseSaver
    {
        IJob _job;
        public LogBaseSaver(IJob job)
        {
            _job = job;
        }
    }
}
