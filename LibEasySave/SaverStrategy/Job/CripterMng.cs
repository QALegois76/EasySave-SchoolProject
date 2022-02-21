using LibEasySave.AppInfo;
using System;

namespace LibEasySave
{
    public class CripterMng
    {
        private static CripterMng _instance = new CripterMng();
        public static CripterMng Instance => _instance;


        private ICyptPlugin _activCripter = null;
        public  ICyptPlugin ActivCripter => _activCripter;


        private CripterMng()
        {
            UpdateCryptPlugin();
        }



        private void UpdateCryptPlugin()
        {
            switch (DataModel.Instance.CryptInfo.CryptMode)
            {
                case ECryptMode.XOR:
                    _activCripter = new CryptosoftPlugin();
                    break;
                default:
                    throw new Exception("CryptMode no found");
                    break;
            }
        }


    }
}
