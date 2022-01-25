using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibEasySave.Model
{
    public abstract class BaseJobSaver 
    {
        protected IJob _job;
        protected  List<DataFile> _fileToSave = new List<DataFile>();

        public BaseJobSaver(IJob job)
        {
            this._job = job; 
            travel(job.SourceFolder, job.DestinationFolder);
        }

        private void travel(string path, string destinationPath)
        {
            if (!Directory.Exists(path) || !Directory.Exists(destinationPath))
            {
                throw new Exception("Error path");
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            System.IO.FileInfo[] files = directoryInfo.GetFiles();

            System.IO.DirectoryInfo[] subDirs = null;

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    try
                    {
                        string src = fi.FullName;
                        string dest = Path.Combine(destinationPath, fi.Name);
                        long size = fi.Length;
                        _fileToSave.Add(new DataFile(src, dest, size));
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.Fail("Veuillez vérifier le DataFile");
                    }
                }

                // Now find all the subdirectories under this directory.
                subDirs = directoryInfo.GetDirectories();

                foreach (System.IO.DirectoryInfo repos in subDirs)
                {
                    string newDestPath = Path.Combine(destinationPath, repos.Name);
                    if (!Directory.Exists(newDestPath))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(newDestPath);
                    }
                    // Resursive call for each subdirectory.
                    travel(repos.FullName, newDestPath);
                }
            }
        }

        public  abstract void copyfile();

        public struct DataFile
        {
            private string _src;
            private string _dest;
            private long _size;

            public string SrcFile => _src;
            public string DestFile => _dest;
            public long SizeFile => _size;

            public DataFile(string src, string dest, long size)
            {
                _src = src;
                _dest = dest;
                _size = size;
            }

        }

    }
}
