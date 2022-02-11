using System;
using System.IO;

namespace CryptoSoft
{
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
