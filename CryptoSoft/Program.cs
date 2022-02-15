using System;
using System.Diagnostics;
using System.IO;

namespace CryptoSoft
{


    class Program
    {

        static void Main(string[] args)
        {
            string msg;
            CryptInfo cryptInfo;
            if (!CryptInfo.TryParse(args, out cryptInfo,out msg))
            {
                Console.WriteLine(msg);
            }

            if (cryptInfo == null)
                return;



            long time = CrypterStrategy.Execute(cryptInfo);

            //var src = File.ReadAllBytes(args[4]);
            //var dest = File.ReadAllBytes(args[1]);

            //if (src.Length != dest.Length)
            //    Console.WriteLine("Failed size");

            //for (int i = 0; i < src.Length; i++)
            //{
            //    if (src[i] != dest[i])
            //    {
            //        Console.WriteLine("Failed at index : "+i +"  => src = "+src[i] +"  !=  "+dest[i]);
            //        return;
            //    }
            //}
            //Console.WriteLine("No error detected");

            Console.WriteLine("time to crypt / decrypt = " + time);

        }



    }
}
