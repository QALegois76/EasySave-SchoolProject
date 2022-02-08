namespace CryptoSoft
{
    public struct CryptFileInfo
    {
        private long _key;

        private string _fileSrc;
        private string _fileDest;

        public long Key => _key;
        public string FileDest => _fileDest;  
        public string FileSrc => _fileSrc;  

        public CryptFileInfo(string fileSrc, string fileDest, long key)
        {
            this._fileSrc = fileSrc;
            this._fileDest = fileDest;
            this._key = key;
        }


    }
}
