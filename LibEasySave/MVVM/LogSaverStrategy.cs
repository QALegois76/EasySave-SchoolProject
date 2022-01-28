using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LibEasySave.Model
{
    public class LogSaverStrategy
    {
        public static bool Save(ILog log)
        {
            if (log == null)
            {
                Debug.Fail("Veuillez entrer un job");
                return false;
            }

            LogBaseSaver temp;

            try
            {
                switch (log.DisplayMode)
                {

                    case EDisplayMode.JSON:
                        temp = new JSONText();
                        break;

                    default:
                    case EDisplayMode.XML:
                        temp = new XMLText();
                        break;
                }

                temp.GetSavedLogText(log);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool Save(IState state)
        {
            if (state == null)
            {
                Debug.Fail("Veuillez entrer un job");
                return false;
            }

            LogBaseSaver temp;

            try
            {
                switch (state.DisplayMode)
                {

                    case EDisplayMode.JSON:
                        temp = new JSONText();
                        break;

                    default:
                    case EDisplayMode.XML:
                        temp = new XMLText();
                        break;
                }

                temp.GetSavedStateText(state);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

    }
}
