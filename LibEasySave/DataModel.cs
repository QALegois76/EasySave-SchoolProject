using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave
{
    /// <summary>
    /// DataModel is singleton.
    /// It contains the lists of  extension of  files to encrypt (_listEncryptExtend) and files to make priority (_listPriorityExtend).
    /// 
    /// </summary>
    public class DataModel
    {

        List<String> _listEncryptExtend = new List<string>();
        List<String> _listPriorityExtend = new List<String>();
        /// <summary>
        /// Number of priority files which run
        /// </summary>
        private int _priorityFilerunningnumber;

        private static DataModel cripting_instance = null;

        private DataModel() {
            _listEncryptExtend.Add(".txt");
            _listPriorityExtend.Add(".txt");
            this._priorityFilerunningnumber = 0;
        }

        /// <summary>
        /// Initialisation of DataModel
        /// </summary>
        public static DataModel Instance
        {
            get
            {

                if (cripting_instance == null)
                    cripting_instance = new DataModel();

                return cripting_instance;
            }
        }

        public bool IsEncript(string extend)
        {
            if (_listEncryptExtend.Contains(extend))
                return true;

            return false;
        }

        public List<String> ListPriorityExtend
        {
            get
            {
                return _listPriorityExtend;
            }
        }

        public void IncrementPriorityFile()
        {
            this._priorityFilerunningnumber++;
        }

        public void DecrementPriorityFile()
        {
            this._priorityFilerunningnumber--;
        }
        public bool IsPriorityFileRunning()
        {
            return this._priorityFilerunningnumber > 0;
        }

        public List<String> ListEncryptExtend
        {
            get
            {
                return _listEncryptExtend;
            }
        }


    }
}
