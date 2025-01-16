using Newtonsoft.Json;
using System;

namespace LibEasySave
{
    public abstract partial class BaseJobSaver 
    {
        [Serializable]
        public struct DataFile
        {
            [JsonProperty]
            private string _src;
            [JsonProperty]
            private string _dest;
            [JsonProperty]
            private long _size;

            [JsonIgnore]
            public string SrcFile => _src;
            [JsonIgnore]
            public string DestFile => _dest;
            [JsonIgnore]
            public long SizeFile => _size;

            public DataFile(string src, string dest, long size)
            {
                _src = src;
                _dest = dest;
                _size = size;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is DataFile))
                    return false;

                return _src == ((DataFile)obj)._src;
            }

            public static bool operator ==(DataFile A, DataFile B) => A.Equals(B);
            public static bool operator !=(DataFile A, DataFile B) => !A.Equals(B);
        }
    }
}
