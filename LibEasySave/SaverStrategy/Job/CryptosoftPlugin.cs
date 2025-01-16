using System;
using System.Diagnostics;

namespace LibEasySave
{
    public class CryptosoftPlugin : ICyptPlugin
    {
        private ProcessStartInfo _processStartInfo = new ProcessStartInfo("CrytoSoft.exe");
        

        public void Crypt(string srcFile, string destFile, string key)
        {
            Process process = new Process();
            _processStartInfo.CreateNoWindow = true;

            process.StartInfo.Arguments ="\"" + srcFile + "\" \"" + destFile + "\" " + key + " -D";
            //process.StartInfo = _processStartInfo;
            process.StartInfo.FileName= @"..\..\..\..\LibEasySave\CryptoSoft\net5.0\CryptoSoft.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = "";
            while (!process.StandardOutput.EndOfStream)
            {
                output += process.StandardOutput.ReadLine()+"\n";
            }
        }

        public void DeCrypt(string srcFile, string destFile, string key)
        {
            throw new NotImplementedException();
        }
    }
}
