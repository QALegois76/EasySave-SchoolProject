﻿using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LibEasySave
{
    public static class FileSaverStrategy
    {
        public static string Save(object objToSave, string pathFile, bool isOverwrite , ESavingFormat format)
        {
            if (objToSave == null)
            {
                Debug.Fail("Veuillez entrer un dailyLog");
                return null;
            }

            string text;
            try
            {
                LogBaseSaver temp;
                switch (format)
                {

                    case ESavingFormat.JSON:
                        temp = new JSONText();
                        break;

                    default:
                    case ESavingFormat.XML:
                        temp = new XMLText();
                        break;
                }

                text = temp.GetFormatingText(objToSave);
                if (isOverwrite)
                    FileWriter.Write(text, pathFile);
                else
                    FileWriter.Append(text, pathFile);
            }
            catch (Exception ex)
            {
                return null;
            }
            return text;
        }

    }
}
