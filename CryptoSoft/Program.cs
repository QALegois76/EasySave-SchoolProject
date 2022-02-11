using System;

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

            Console.WriteLine("time to crypt / decrypt = " + time);

        }



    }
}
