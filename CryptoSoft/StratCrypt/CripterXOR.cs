using System;
using System.Collections.Generic;

namespace CryptoSoft
{
    public class CripterXOR : CripterBase
    {
        public CripterXOR(CryptFileInfo cryptFileInfo) : base(cryptFileInfo)
        {
        }

        public override long Crypt()
        {
            _watch.Start();
            for (int iByte = 0; iByte < _contentSrc.Length; iByte++)
            {

                List<bool> byteSrc = _contentSrc[iByte].ToBool(NB_BIT_BYTE);
                List<bool> byteDest = new List<bool>();
                for (int iBit = 0; iBit < NB_BIT_BYTE; iBit++)
                    byteDest.Add(byteSrc[iBit] != _cryptFileInfo.Key.ToBool(NB_BIT_LONG)[(iByte * NB_BIT_BYTE + iBit) % NB_BIT_LONG]);

                _contentDest[iByte] = (byte)byteDest.ToDecimal();
            }

            FileWriter.Write(_contentDest, _cryptFileInfo.FileDest);

            _watch.Stop();
            return _watch.ElapsedMilliseconds;
        }

        public override long DeCrypt() => Crypt();

    }
}
