using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LibEasySave.Model
{
    class StateSaverStrategy
    {
        public static bool Save(IState state)
        {
            if (state == null)
            {
                Debug.Fail("Veuillez entrer un state");
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
