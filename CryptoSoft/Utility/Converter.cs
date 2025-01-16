using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoSoft
{
    public static class Converter
    {
        public static long ToDecimal(this List<bool> boolean)
        {
            if (boolean == null || boolean.Count == 0)
                return -1;

            int result = 0;
            for (int i = 0; i < boolean.Count; i++)
            {
                result += (int)Math.Pow(2, i) * ((boolean[i]) ? 1 : 0);
            }
            return result;
        }

        public static long ToDecimal(this bool[] boolean) => boolean.ToList().ToDecimal();

        public static List<bool> ToBool(this List<int> list, int nBit = 0)
        {
            List<bool> output = new List<bool>();


            while (list.First() == 0 && list.Count > nBit)
            {
                list.RemoveAt(0);
            }
            foreach (int i in list)
            {
                if (i == 0)
                    output.Add(false);
                else
                    output.Add(true);
            }
            return output;
        }

        public static List<bool> ToBool(this long value, int nbBit = 0)
        {
            // NBA : corrigé
            List<bool> output = new List<bool>();
            int p = 1;
            for (int i = 0; i < nbBit; i++)
            {
                if ((value & p) == p)
                {
                    output.Add(true);
                }
                else
                    output.Add(false);

                p *= 2;
            }
            return output;
        }




        public static List<bool> ToBool(this byte value, int nbBit = 0)
        {
            // NBA : corrigé
            List<bool> output = new List<bool>();
            int p = 1;
            for (int i = 0; i < nbBit; i++)
            {
                if ((value & p) == p)
                {
                    output.Add(true);
                }
                else
                    output.Add(false);

                p *= 2;
            }
            return output;
        }

        public static byte[] ToByte(this long l)
        {
            return BitConverter.GetBytes(l);
        }
    }
}
