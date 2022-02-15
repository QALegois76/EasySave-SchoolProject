using CryptoSoft.CryptInfoModel;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace CryptoSoft
{
    public static class FileWriter
    {

        public static void Write(string text, string fileName)
        {

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine(text);
            }
        }

        public static void WriteBL(BigList<byte> fileContent, string fileName)
        {
            using (var mmf = MemoryMappedFile.CreateFromFile(fileName, FileMode.OpenOrCreate))
            {
                int count = 0;
                for (long idx = 0; idx < fileContent.Count; idx += fileContent.Count/1000)
                {
                    if (ThreadPool.PendingWorkItemCount <= 1000)
                    {
                        count++;
                        if (WriteBLThreadData.RANGE > fileContent.Count - idx)
                            ThreadPool.QueueUserWorkItem(new WaitCallback(WriteData), new WriteBLThreadData(idx, fileContent, mmf.CreateViewAccessor(idx, fileContent.Count - idx)));
                        else
                            ThreadPool.QueueUserWorkItem(new WaitCallback(WriteData), new WriteBLThreadData(idx, fileContent, mmf.CreateViewAccessor(idx, fileContent.Count / 1000)));
                    }
                    else
                        Thread.Sleep(300);

                }
            }
        }


        private static void WriteData(object obj)
        {
            WriteBLThreadData data = (WriteBLThreadData)obj;
            for (long idx = 0; idx < data.Accessor.Capacity; idx++)
            {
                data.Accessor.Write(idx , data.Data[idx + data.Offset]);
            }
            data.Accessor.Dispose();
        }

        private class WriteBLThreadData
        {
            public const long RANGE = 4194304;

            // private 
            private long _offset;

            private BigList<byte> _data;

            private MemoryMappedViewAccessor _accessor;


            // public 
            public long Offset => _offset; 
            public BigList<byte> Data => _data;
            public MemoryMappedViewAccessor Accessor  => _accessor;


            // constructor
            public WriteBLThreadData(long offset, BigList<byte> data, MemoryMappedViewAccessor accessor)
            {
                _offset = offset;
                _data = data;
                _accessor = accessor;
            }
        }
        
        public static void Write(byte[] content, string fileName)=>  File.WriteAllBytes(fileName, content);
        


        public static void Append(string text, string fileName)
        {
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine(text);
            }
        }
    }
}
