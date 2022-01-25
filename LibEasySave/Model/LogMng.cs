using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave.Model
{
    public class LogMng
    {

        private static LogMng instance;
        private static readonly object lockobj = new object();

        private LogMng()
        {

        }
        public static LogMng GetInstance
        {
            get
            {

                if (instance == null)
                {
                    lock (lockobj)
                    {
                        if (instance == null)
                        {
                            instance = new LogMng();
                        }
                    }
                }
                return instance;
            }
        }


    }
}
