using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Threading;
using CryptoSoft.CryptInfoModel;
using Newtonsoft.Json;

namespace CryptoSoft
{
    partial class Program
    {

        static void Main(string[] args)
        {

            //FillFileList();
            //return;

            string msg;
            CryptInfo cryptInfo;
            if (!CryptInfo.TryParse(args, out cryptInfo,out msg))
            {
                Console.WriteLine(msg);
            }

            if (cryptInfo == null)
                return;

            long time = CrypterStrategy.Execute(cryptInfo);

            Console.WriteLine("time to crypt / decrypt = " + time);

        }

        private static void FillFileList()
        {
            byte[,] data = new byte[byte.MaxValue+1, byte.MaxValue+1];

            for (int file = 0; file <= byte.MaxValue; file++)
            {
                for (int key = 0; key <= byte.MaxValue; key++)
                {
                    List<bool> fileBools = new List<bool>();
                    List<bool> keyBools = new List<bool>();

                    fileBools = ((long)file).ToBool(8);
                    keyBools = ((long)key).ToBool(8);

                    List<bool> result = new List<bool>(8);
                    for (int i = 0; i < 8; i++)
                    {
                        result.Add(fileBools[i] != keyBools[i]);
                    }

                    data[file, key] = (byte)result.ToDecimal();
                }

            }

            string fileContent = JsonConvert.SerializeObject(data, Formatting.Indented);
            FileWriter.Write(fileContent, "Data.json");


        }



        
    }
}
