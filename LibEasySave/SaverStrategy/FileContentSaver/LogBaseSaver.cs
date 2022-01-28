using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave
{
    public abstract class LogBaseSaver
    {
        public LogBaseSaver()
        {
        }

        public abstract string GetFormatingText(object dailyLog);


    }
}
