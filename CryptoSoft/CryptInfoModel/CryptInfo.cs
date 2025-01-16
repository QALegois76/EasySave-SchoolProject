using System;
using System.Collections.Generic;
using System.IO;

namespace CryptoSoft
{
    public class CryptInfo
    {
        private enum EArg
        {
            Src = 0,
            Dest = 1,
            Key = 2,
            CryptDir = 3,
            CryptTyp = 4
        }

        #region VARIABLE

        #region const
        private const string ERROR = "ERROR";
        private const string ER_ARG_INCORRECT = "please enter 2 argurment no more and no less";
        private const string ER_FILE_NO_EXIST = "the file doesn't exist";
        private const string ER_FILE_CANT_OPEN = "the file specified can't be open";
        private const string ER_FILE_LOC_NO_EXIST = "the location specified doesn't exist";
        private const string ER_CRYP_DIR_UNKNOMWN = " the 3rd paramter is not -C ( crypt) or -D ( decrypt )";
        private const string ER_CRYP_TYPE_UNKNOWN = "the  4th parameter in unknowm";
        private const string ER_KEY = "the specified key is not correct";


        private static readonly Dictionary<string, ECryptDirection> TRANSLATER_CRYP_DIR = new Dictionary<string, ECryptDirection>()
        { {"-D",ECryptDirection.Decrypt}, {"-C",ECryptDirection.Crypt } };      
        private static readonly Dictionary<string, ECrypType> TRANSLATER_CRYP_TYPE = new Dictionary<string, ECrypType>()
        { {"XOR",ECrypType.XOR}, };
        #endregion

        #region private
        private const int NB_ARG = 4;

        private readonly CryptFileInfo _cryptFileInfo;
        private ECryptDirection _cryptDirection = ECryptDirection.Crypt;
        private ECrypType _cryptType = ECrypType.XOR;
        #endregion

        #region accessor
        public CryptFileInfo CryptFileInfo => _cryptFileInfo;
        public ECryptDirection CryptDirection { get => _cryptDirection; set => _cryptDirection = value; }
        public ECrypType CrypType { get => _cryptType; set => _cryptType = value; }


        #endregion

        #endregion

        // constructor
        public CryptInfo(string src , string dest ,long key,  ECryptDirection cryptDirection = ECryptDirection.Crypt)
        {
            _cryptFileInfo = new CryptFileInfo(src, dest,key);
            _cryptDirection = cryptDirection;
            // for now
            _cryptType = ECrypType.XOR;
        }

        #region METHOD
        public static bool TryParse(string[] args, out CryptInfo cryptInfo , out string errorMsg)
         {
            cryptInfo = null;
            string msg = null;
            errorMsg = null;
            if (!CheckArgs(args,out msg))
            {
                errorMsg = ERROR+ " : " + msg;
                return false;
            }

            cryptInfo = new CryptInfo(args[(int)EArg.Src],args[(int)EArg.Dest],long.Parse(args[(int)EArg.Key]));
            cryptInfo._cryptDirection = TRANSLATER_CRYP_DIR[args[(int)EArg.CryptDir]];
            // defined in hard way for now
            cryptInfo._cryptType = ECrypType.XOR;
            return true;
        }

        #region check method

        private static bool CheckArgs(string[] args , out string errorMsg)
        {
            if (args.Length < NB_ARG)
            {
                errorMsg = ER_ARG_INCORRECT;
                return false;
            }

            if (!CheckFileSrc(args[(int)EArg.Src], out errorMsg))
                return false;
            
            if (!CheckPathDest(args[(int)EArg.Dest], out errorMsg))
                return false;
                        
            if (!CheckCryptDirection(args[(int)EArg.CryptDir], out errorMsg))
                return false;

            return true;
                  
            // not used for now
            if (!CheckCryptType(args[4], out errorMsg))
                return false;
                                    


        }

        private static  bool CheckFileSrc(string arg ,out string errorMsg)
        {
            errorMsg = null;
            if(!File.Exists(arg))
            {
                errorMsg = ER_FILE_NO_EXIST;
                return false;
            }

            return true;

        }

        private static  bool CheckPathDest(string arg , out string errorMsg)
        {
            errorMsg = null;
            string directory =  Path.GetDirectoryName(arg);
            
            if (!Directory.Exists(directory))
            {
                errorMsg = ER_FILE_LOC_NO_EXIST;
                return false;
            }

            return true;

        }

        private static bool CheckCryptDirection(string arg, out string errorMsg)
        {
            errorMsg = null;
            if (!TRANSLATER_CRYP_DIR.ContainsKey(arg.Trim().ToUpper()))
            {
                errorMsg = ER_CRYP_DIR_UNKNOMWN;
                return false;
            }
            return true;
        }

        private static bool CheckCryptType(string arg, out string errorMsg)
        {
            errorMsg = null;
            if (!TRANSLATER_CRYP_TYPE.ContainsKey(arg.Trim().ToUpper()))
            {
                errorMsg = ER_CRYP_TYPE_UNKNOWN;
                return false;
            }
            return true;
        }

        private static bool CheckKeyType(string arg, out string errorMsg)
        {
            errorMsg = null;
            long l;
            if (!long.TryParse(arg,out l))
            {
                return false;
            }

            return true;
        }
        #endregion

        #endregion
    }
}
