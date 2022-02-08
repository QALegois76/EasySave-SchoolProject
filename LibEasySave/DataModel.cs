using System;
using System.Collections.Generic;
using System.Text;

namespace LibEasySave
{
    public class DataModel
    {

        List<String> _listExtend = new List<string>();

        private static DataModel cripting_instance = null;

        private DataModel() {
            _listExtend.Add(".text");
            
        }

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
            if (_listExtend.Contains(extend))
                return true;

            return false;
        }

        
    }
}
