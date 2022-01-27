using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.Model
{
    public class LogMng
    {

        private static LogMng _instance;
        // used for thread safe lock
        private static readonly object _lockobj = new object();

        // private constructor
        private LogMng()
        {

        }
        public static LogMng Instance
        {
            get
            {

                if (_instance == null)
                {
                    lock (_lockobj)
                    {
                        if (_instance == null)
                        {
                            _instance = new LogMng();
                        }
                    }
                }
                return _instance;
            }
        }

        


    }
}
