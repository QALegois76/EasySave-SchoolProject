using CryptoSoft.CryptInfoModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using static CryptoSoft.Program;

namespace CryptoSoft
{
    public static class FileReader
    {

        public static string Read(string fileName)
        {
            string text;
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                string textFinal = "";
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    textFinal += "\n" + line;
                }
                return textFinal;
            }
        }

        public static BigList<byte> ReadBL(string filename)
        {
            BigList<byte> output = new BigList<byte>();

            using (FileStream sr = new FileStream(filename, FileMode.Open))
            {
                long pos = 0;
                Span<byte> span = new Span<byte>();
                while (pos < sr.Length)
                {
                    sr.Read(span);
                    output.Add((byte)sr.ReadByte());
                    pos++;
                }
            }
            return output;
        }





        public static BigList<byte> GetByteBL(string path)
        {

            //long offset = 0x10000000; // 256 megabytes
            // long length = 0x20000000; // 512 megabytes
            // long length = 0x00100000; // 64 megabytes
            long length = 0x00400000; // 4 megabytes  (4194304)

            long fileSize = (new FileInfo(path)).Length;
            BigList<byte> bigList = new BigList<byte>(fileSize);

            using (var mmf = MemoryMappedFile.CreateFromFile(path, FileMode.Open))
            {

                using (var accessor = mmf.CreateViewStream(0,fileSize))
                //using (var accessor = mmf.CreateViewAccessor(0,fileSize))
                {
                    int nbFull = (int)(fileSize / BigList<byte>.MAX_INDEX);
                    for (int i = 0; i < nbFull; i++)
                    {
                        byte[] temp = new byte[BigList<byte>.MAX_INDEX];
                        accessor.Read(temp, 0, (int)BigList<byte>.MAX_INDEX);
                        bigList.SetArray(i, temp);
                        temp = null;
                        GC.Collect();
                    }

                    int byteRemain = (int)(fileSize % BigList<byte>.MAX_INDEX);
                    if (byteRemain > 0)
                    {
                        var tempLast = new byte[byteRemain];
                        accessor.Read(tempLast, 0, byteRemain);
                        bigList.SetArray(nbFull , tempLast);
                        tempLast = null;
                        GC.Collect();
                    }
                }

                //List<ThreadData> data = new List<ThreadData>();

                //for (long i = 0; i < fileSize; i += length)
                //{

                //    Thread t = new(new ParameterizedThreadStart(ThreadMethod));
                //    MemoryMappedViewAccessor temp;

                //    if (fileSize - i < length)
                //    {
                //        temp = mmf.CreateViewAccessor(i, fileSize - i, MemoryMappedFileAccess.Read);
                //    }
                //    else
                //    {
                //        temp = mmf.CreateViewAccessor(i, length, MemoryMappedFileAccess.Read);
                //    }
                //    var d = new ThreadData(i, bigList, temp);
                //   // data.Add(d);
                //    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadMethod), d);
                //}

                //while (ThreadPool.PendingWorkItemCount > 0)
                //    Thread.Sleep(300);
            }
            return bigList;


        }


        private static void ThreadMethod(object obj)
        {
            ThreadData data = (ThreadData)obj;
            for (long idx = 0; idx < data.Accessor.Capacity; idx += 8)
            {
                if (idx == 4194304)
                    ;

                if (data.Accessor.Capacity - idx < 8)
                {
                    for (long i = idx; i < data.Accessor.Capacity; i++)
                    {
                        data.Data[data.Offset + idx] = data.Accessor.ReadByte(idx);

                        //output.Add(data.Accessor.ReadByte(i));
                    }
                }
                else
                {
                    var temp = BitConverter.GetBytes(data.Accessor.ReadUInt64(idx));
                    for (int i = 0; i < temp.Length; i++)
                        data.Data[data.Offset + idx + i] = temp[i];
                }


            }
            data.Accessor.Dispose();
        }




    }
}
