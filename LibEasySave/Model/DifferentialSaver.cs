using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LibEasySave.Model
{
    public class DifferentialSaver : BaseJobSaver
    {
        public DifferentialSaver(IJob job):base(job)
        {

        }

        public override void CopyFile()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            foreach (DataFile item in _fileToSave)
            {

                File.Copy(item.SrcFile, item.DestFile);
            }
            watch.Stop();
        }
    }
}
