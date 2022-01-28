namespace LibEasySave
{
    public abstract partial class BaseJobSaver 
    {
        public struct DataFile
        {
            private string _src;
            private string _dest;
            private long _size;

            public string SrcFile => _src;
            public string DestFile => _dest;
            public long SizeFile => _size;

            public DataFile(string src, string dest, long size)
            {
                _src = src;
                _dest = dest;
                _size = size;
            }

        }
    }
}
