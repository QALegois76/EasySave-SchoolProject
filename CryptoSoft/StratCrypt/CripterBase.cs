using System.Diagnostics;
using System.IO;

namespace CryptoSoft
{
    public abstract class CripterBase
    {
        protected const int NB_BIT_BYTE = 8;
        protected const int NB_BIT_INT = 32;
        protected const int NB_BIT_LONG = 64;

        protected byte[] _contentSrc;
        protected byte[] _contentDest;
        protected CryptFileInfo _cryptFileInfo;

        protected Stopwatch _watch = new Stopwatch();


        public CripterBase(CryptFileInfo cryptFileInfo)
        {
            _cryptFileInfo = cryptFileInfo;
            _contentSrc = File.ReadAllBytes(_cryptFileInfo.FileSrc);
            _contentDest = new byte[_contentSrc.Length];
            _watch.Reset();
        }

        public abstract long Crypt();
        public abstract long DeCrypt();
    }
}
