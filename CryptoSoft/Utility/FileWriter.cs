using System.IO;

namespace CryptoSoft
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
        
        public static void Write(byte[] content, string fileName)=>  File.WriteAllBytes(fileName, content);
        


        public static void Append(string text, string fileName)
        {
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine(text);
            }
        }
    }
}
