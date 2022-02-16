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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            long time = CrypterStrategy.Execute(cryptInfo);
            stopwatch.Stop();

            time = stopwatch.ElapsedMilliseconds;


            int sec =(int)(time /1000)%60;
            int min =(int) (time /( 60*1000));
            int hours = (int)(time /(1000* 3600));

#if DEBUG
            Console.WriteLine("==> " + hours + " hours "+min+" min "+sec+" sec "+time%1000+" ms");
#endif
            Console.WriteLine(time);
        }



    }
}
