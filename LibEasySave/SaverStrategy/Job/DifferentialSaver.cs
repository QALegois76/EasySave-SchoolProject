using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibEasySave.Model
{
    public class DifferentialSaver : BaseJobSaver
    {
        public DifferentialSaver(IJob job):base(job)
        {

        }

        protected override void SearchFile(string path, string destinationPath)
        {
            {
                if (!Directory.Exists(path) || !Directory.Exists(destinationPath))
                {
                    throw new Exception("Error path");
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                FileInfo[] files = directoryInfo.GetFiles();
                DirectoryInfo[] subDirs = null;

                if (files != null)
                {

                    // add all files found in the list
                    foreach (FileInfo fi in files)
                    {
                        string src = fi.FullName;
                        string dest = Path.Combine(destinationPath, fi.Name);

                        if (File.Exists(dest))
                            continue;

                        long size = fi.Length;
                        _fileToSave.Add(new DataFile(src, dest, size));
                    }

                    // Now find all the subdirectories under this directory.
                    subDirs = directoryInfo.GetDirectories();

                    foreach (DirectoryInfo repos in subDirs)
                    {
                        string newDestPath = Path.Combine(destinationPath, repos.Name);

                        // if path does't exist in destination we create it
                        if (!Directory.Exists(newDestPath))
                            Directory.CreateDirectory(newDestPath);

                        // Resursive call for each subdirectory.
                        SearchFile(repos.FullName, newDestPath);
                    }
                }
            }
        }
    }
}
