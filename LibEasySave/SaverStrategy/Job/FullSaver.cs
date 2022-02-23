using LibEasySave.AppInfo;
using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LibEasySave
{
    public class FullSaver : BaseJobSaver
    {
        public FullSaver(IJob job) : base(job)
        {
            _fileToSave = new List<DataFile>();
            _fileToSaveEncrypt = new List<DataFile>();
        }

        protected override void SearchFile(string path, string destinationPath)
        {

            if (!Directory.Exists(path) || !Directory.Exists(destinationPath))
            {
                Debug.Fail("path does't exist");
                return;
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
                    long size = fi.Length;
                    var temp = new DataFile(src, dest, size);
                    if (_job.IsEncrypt && DataModel.Instance.CryptInfo.IsCryptedExt(fi.Extension))
                    {
                        _fileToSaveEncrypt.Add(temp);

                    }
                    else
                        _fileToSave.Add(temp);

                    _totalSize += size;
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
