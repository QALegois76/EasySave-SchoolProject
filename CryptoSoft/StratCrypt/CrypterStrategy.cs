using System;

namespace CryptoSoft
{
    public static class CrypterStrategy
    {

        public static long Execute(CryptInfo cryptInfo)
        {
            CripterBase criptBase;
            switch (cryptInfo.CrypType)
            {
                case ECrypType.XOR:
                    criptBase = new CripterXOR(cryptInfo.CryptFileInfo);
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }


            
            if (cryptInfo.CryptDirection == ECryptDirection.Crypt)
                return criptBase.Crypt();
            else
                return criptBase.DeCrypt();
        }
    }
}
