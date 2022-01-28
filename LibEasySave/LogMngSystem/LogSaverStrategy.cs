using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LibEasySave.Model
{
    public class LogSaverStrategy
    {
        public static string Save(ILog log, EDisplayMode mode)
        {
            if (log == null)
            {
                Debug.Fail("Veuillez entrer un job");
                return null;
            }

            string text;
            try
            {

                LogBaseSaver temp;
                switch (mode)
                {

                    case EDisplayMode.JSON:
                        temp = new JSONText();
                        break;

                    default:
                    case EDisplayMode.XML:
                        temp = new XMLText();
                        break;
                }

                text = temp.GetSavedLogText(log);
            }
            catch (Exception ex)
            {
                return null;
            }
            return text;
        }

        public static string Save(IDailyLog dailyLog, EDisplayMode mode)
        {
            if (dailyLog == null)
            {
                Debug.Fail("Veuillez entrer un dailyLog");
                return null;
            }

            string text;
            try
            {

                LogBaseSaver temp;
                switch (mode)
                {

                    case EDisplayMode.JSON:
                        temp = new JSONText();
                        break;

                    default:
                    case EDisplayMode.XML:
                        temp = new XMLText();
                        break;
                }

                text = temp.GetSavedLogText(dailyLog);
            }
            catch (Exception ex)
            {
                return null;
            }
            return text;
        }

        public static string Save(IStateLog stateLog, EDisplayMode mode)
        {
            if (stateLog == null)
            {
                Debug.Fail("Veuillez entrer un job");
                return null;
            }

            string text;
            try
            {

                LogBaseSaver temp;
                switch (mode)
                {

                    case EDisplayMode.JSON:
                        temp = new JSONText();
                        break;

                    default:
                    case EDisplayMode.XML:
                        temp = new XMLText();
                        break;
                }

                text = temp.GetSavedStateText(stateLog);
            }
            catch (Exception ex)
            {
                return null;
            }
            return text;
        }
    }
}
