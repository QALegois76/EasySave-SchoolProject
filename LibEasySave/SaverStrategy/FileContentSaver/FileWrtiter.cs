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
                sw.WriteLine(text);
            }
        }


        public static void Append(string text, string fileName)
        {
            using (StreamWriter sw = File.AppendText(fileName))
            {     
                sw.WriteLine(text);
            }
        }
    }

    public static class FileReader
    { 

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







