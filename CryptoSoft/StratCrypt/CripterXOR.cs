using System;
using System.Collections.Generic;
using System.Threading;
using CryptoSoft.CryptInfoModel;
using Newtonsoft.Json;

namespace CryptoSoft
{
    public class CripterXOR : CripterBase
    {
        private byte[,] _translater;

        

        public CripterXOR(CryptFileInfo cryptFileInfo) : base(cryptFileInfo)
        {
            _translater =  JsonConvert.DeserializeObject<byte[,]>(global::CryptoSoft.Resource1.Data);
        }

        //public override long Crypt()
        //{
        //    _watch.Start();
        //    for (int iByte = 0; iByte < _contentSrc.Length; iByte++)
        //    {

        //        List<bool> byteSrc = _contentSrc[iByte].ToBool(NB_BIT_BYTE);
        //        List<bool> byteDest = new List<bool>();
        //        for (int iBit = 0; iBit < NB_BIT_BYTE; iBit++)
        //            byteDest.Add(byteSrc[iBit] != _cryptFileInfo.Key.ToBool(NB_BIT_LONG)[(iByte * NB_BIT_BYTE + iBit) % NB_BIT_LONG]);

        //        _contentDest[iByte] = (byte)byteDest.ToDecimal();
        //    }

        //    FileWriter.Write(_contentDest, _cryptFileInfo.FileDest);

        //    _watch.Stop();
        //    return _watch.ElapsedMilliseconds;
        //}
        
        
        
        public override long Crypt()
        {
            _watch.Start();
            byte[] key = _cryptFileInfo.Key.ToByte();

            for (long iByte = 0; iByte <= _contentFile.Count; iByte += ThreadCryptData.RANGE)
            {
                ThreadCryptData threadCryptData = new ThreadCryptData(iByte,key,_translater,_contentFile);
                ThreadPool.QueueUserWorkItem(new WaitCallback(CripterXOR.CryptThread), threadCryptData);
            }

            while (ThreadPool.PendingWorkItemCount > 0)
                Thread.Sleep(300);

            FileWriter.WriteBL(_contentFile, _cryptFileInfo.FileDest);

            _watch.Stop();
            return _watch.ElapsedMilliseconds;
        }


        public static void CryptThread(object obj)
        {
            ThreadCryptData data = (ThreadCryptData)obj;
            for (long i = data.Offset; i < data.Offset+ ThreadCryptData.RANGE; i++)
            {
                //if (i == 2965536)
                //    ;
                if (i < data.InputFile.Count)
                    data.InputFile[i] =  data.Translater[data.InputFile[i], data.Key[i % data.Key.LongLength]];
            }

        }



        public override long DeCrypt() => Crypt();

    }


    class ThreadCryptData
    {
        public const long RANGE = 4194304;

        private long _offset;
        private byte[] _key;
        private byte[,] _translater;

        private List<byte> _ouptut;
        private BigList<byte> _inputFile;



        public long Offset => _offset;
        public byte[] Key => _key;
        public byte[,] Translater => _translater;
        public List<byte> Ouptut => _ouptut;
        public BigList<byte> InputFile  => _inputFile; 


        // constructor
        public ThreadCryptData(long offset, byte[] key, byte[,] translater, BigList<byte> inputFile)
        {
            _ouptut = new List<byte>();
            _offset = offset;
            _offset = offset;
            _key = key;
            _translater = translater;
            _inputFile = inputFile;
        }



    }
}
