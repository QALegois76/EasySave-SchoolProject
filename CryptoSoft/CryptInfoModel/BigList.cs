using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSoft.CryptInfoModel
{
    public class BigList<T>
    {
        private const long MAX_INDEX = int.MaxValue - 56;

        private long _count = 0;

        private List<List<T>> _data;


        public long Count => GetCount();

        public T this[long idx]
        {
            get => GetItemAt(idx);
            set
            {
                int first = (int)(idx / MAX_INDEX);
                int second = (int)(idx % ((long)MAX_INDEX));

                _data[first][second] = value;
            }
        }

        public List<T>[] Array() => _data.ToArray();


        public BigList(long size = 0)
        {
            long Nlist = size / (long)MAX_INDEX;
            long NSubList = size % (long)MAX_INDEX;

            _data = (new List<T>[Nlist]).ToList();
            for(int nExt = 0; nExt< Nlist; nExt ++)
            {
                _data[nExt] = (new T[MAX_INDEX]).ToList();
            }

            if (NSubList > 0)
                _data.Add((new T[NSubList]).ToList());

            //_data = new List<List<T>>();
            _count = 0;
        }

        public void Add(T obj)
        {
            if (_data.Count == 0 || _data.Last().Count == MAX_INDEX)
                _data.Add(new List<T>());

            _data.Last().Add(obj);
            _count++;
        }

        public void AddRange(T[] obj) => AddRange(obj.ToList());

        public void AddRange(List<T> obj)
        {
            if (_data.Count == 0)
                _data.Add(new List<T>());

            if ((long)_data.Last().Count + (long)obj.Count >= (long)MAX_INDEX)
            {
                int range = _data.Last().Count + obj.Count - (int)MAX_INDEX;
                _data.Last().AddRange(obj.GetRange(0, range));
                _data.Add(new List<T>());
                _data.Last().AddRange(obj.GetRange(range, obj.Count - range));
            }
            else
            {
                _data.Last().AddRange(obj);
            }
        }

        public void RemoveAt(long idx)
        {
            int first = (int)(idx / MAX_INDEX);
            int second = (int)(idx % (long)MAX_INDEX);

            _data[first].RemoveAt(second);
            _count--;
        }

        public void Remove(T obj)
        {
            for (int i = 0; i < _data.Count; i++)
            {
                for (int y = 0; y < _data[i].Count; y++)
                {
                    if (_data[i][y].Equals( obj))
                    {
                        _data[i].RemoveAt(y);
                        _count--;
                        return;
                    }
                }
            }
        }

        private T GetItemAt(long  idx)
        {
            int first = (int)(idx / MAX_INDEX);
            int second = (int)(idx % ((long)MAX_INDEX));

            return _data[first][second];
        }

        private long  GetCount()
        {
            long f = (long)_data.Count-1;
            long s = (long)_data.Last().Count;
            return f * (long)MAX_INDEX + s;
        }

        private T[][] GetArrays()
        {
            long f = (long)_data.Count;
            List<T[]> output = new List<T[]>();
            foreach (var item in _data)
            {
                output.Add(item.ToArray());
            }
            return output.ToArray();
        }


    }
}
