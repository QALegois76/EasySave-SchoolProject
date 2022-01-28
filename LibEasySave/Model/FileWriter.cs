using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LibEasySave
{
    public static class FileWriter
    {
        public static void Write(string text, string fileName)
        {

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(text);
            }
        }
        public static void Append(string text, string fileName)
        {
            string fileReaded;
            fileReaded = Read(fileName);
            text = fileReaded +" "+ text;
            Write(text, fileName);
        }
        private static string Read(string fileName)
        {
            string text;
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                string textFinal = "";
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    textFinal += "\n" + line;
                }
                return textFinal;
            }
        }
    }
}







