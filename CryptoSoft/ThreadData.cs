using CryptoSoft.CryptInfoModel;
using System.IO.MemoryMappedFiles;

namespace CryptoSoft
{
    partial class Program
    {
        public class ThreadData
        {
            // private
            private long _offset;
            private BigList<byte> _data;

            private MemoryMappedViewAccessor _accessor;


            // public accessor
            public long Offset => _offset;
            public BigList<byte> Data => _data;
            public MemoryMappedViewAccessor Accessor => _accessor;


            // constructor
            public ThreadData(long offset, BigList<byte> data, MemoryMappedViewAccessor accessor)
            {
                _offset = offset;
                _accessor = accessor;
                _data = data;
            }
            

        }

        



    }
}
