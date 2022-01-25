using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LibEasySave.Model
{
    public class JobSaverStrategy
    {
        IJob jober;

        private static List<DataFile> _fileToSave = new List<DataFile>();

        public JobSaverStrategy()
        {
            /*fileToSave.Add(new DataFile());

            foreach (var item in _fileToSave)
            {
                item.
            }*/
        }

        public static bool save(IJob job)
        {
            if (job == null)
            {
                Debug.Fail("Veuillez entrer un job");
                return false;
            }

            travel(job.SourceFolder, job.DestinationFolder);
            Console.WriteLine(_fileToSave.Count.ToString());

            return true;
        }

        private static void travel(string path, string destinationPath) 
        {
            if (!Directory.Exists(path) || !Directory.Exists(destinationPath))
            {
                return;
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
                    } catch(Exception e)
                    {
                        Debug.Fail("Veuillez vérifier le DataFile");
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
                    travel(repos.FullName,newDestPath);
                }
            }
        }

        // copy 
        private void copyfiles(DataFile targetPath)
        {
            string fileName = System.IO.Path.GetFileName(targetPath.SrcFile);
            string destFile = System.IO.Path.Combine(targetPath.DestFile, fileName);
            System.IO.File.Copy(System.IO.Path.Combine(targetPath.SrcFile, fileName), destFile, true);
        }

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
