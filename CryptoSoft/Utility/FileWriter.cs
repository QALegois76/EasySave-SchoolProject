using CryptoSoft.CryptInfoModel;
using System;
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
            MemoryMappedFile mmf = null;
            try
            {
                using (mmf = MemoryMappedFile.CreateFromFile(fileName, FileMode.OpenOrCreate, "CryptoSoft_outputFile", fileContent.Count, MemoryMappedFileAccess.ReadWrite))
                {
                    using (var accessor = mmf.CreateViewAccessor(0, fileContent.Count, MemoryMappedFileAccess.Write))
                    {
                        for (int i = 0; i < fileContent.CountArray; i++)
                        {
                            accessor.WriteArray<byte>(i * BigList<byte>.MAX_INDEX, fileContent.GetArray(i), 0, fileContent.GetArray(i).Length);
                        }
                    }

                    //for (long idx = 0; idx < fileContent.Count; idx += fileContent.Count / 512)
                    //{
                    //    idxSave = idx;
                    //    var accessor = mmf.CreateViewAccessor(idx, fileContent.Count / 512, MemoryMappedFileAccess.ReadWrite);
                    //    WriteBLThreadData writeBL = new WriteBLThreadData(idx, fileContent, accessor);
                    //    ThreadPool.QueueUserWorkItem(new WaitCallback(WriteData), writeBL);
                    //}

                }
            }
            catch (Exception ex)
            {
                ;
            }
        }


        private static void WriteData(object obj)
        {
            WriteBLThreadData data = (WriteBLThreadData)obj;
            //for (long idx = 0; idx < data.Accessor.Capacity; idx++)
            //{
            //    data.Accessor.Write(idx , data.Data[idx + data.Offset]);
            //}

            for (long idx = 0; idx < data.Accessor.Capacity; idx += 8)
            {

                if (data.Accessor.Capacity - idx < 8)
                {
                    for (long i = 0; i < data.Accessor.Capacity - idx; i++)
                    {
                        data.Accessor.Write(idx, data.Data[idx]);
                    }
                }
                else
                {
                    byte[] temp = new byte[8];
                    for (int i = 0; i < 8; i++)
                    {
                        temp[i] = data.Data[idx + i];
                    }
                    var v = BitConverter.ToInt64(temp);
                    data.Accessor.Write(idx, v);

                }
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
