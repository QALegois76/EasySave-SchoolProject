﻿using LibEasySave.AppInfo;
using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibEasySave
{
    public class DifferentialSaver : BaseJobSaver
    {
        public DifferentialSaver(IJob job) :base(job)
        {

        }

        protected override void SearchFile(string path, string destinationPath , bool isCheck)
        {
            {
                if (!isCheck && (!Directory.Exists(path) || !Directory.Exists(destinationPath)))
                //if (!Directory.Exists(path)  /*|| !Directory.Exists(destinationPath)*/)
                {
                    return;
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
                        var temp = new DataFile(src, dest, size);
                        _jobInfo.NFiles++;
                        _jobInfo.TotalSize += size;
                        if (_job.IsEncrypt && DataModel.Instance.CryptInfo.IsCryptedExt(fi.Extension))
                        {
                            _fileToSaveEncrypt.Add(temp);
                            _jobInfo.NFileCrypt++;

                        } else
                            _fileToSave.Add(temp);

                        _totalSize += size;
                    }

                    // Now find all the subdirectories under this directory.
                    subDirs = directoryInfo.GetDirectories();

                    foreach (DirectoryInfo repos in subDirs)
                    {
                        string newDestPath = Path.Combine(destinationPath, repos.Name);
                        _jobInfo.NFolders++;
                        if (!_folderToCreate.Contains(newDestPath))
                            _folderToCreate.Add(newDestPath);
                        // if path does't exist in destination we create it
                        //if (!Directory.Exists(newDestPath))
                        //    Directory.CreateDirectory(newDestPath);

                        // Resursive call for each subdirectory.
                        SearchFile(repos.FullName, newDestPath,true);
                    }
                }
            }
        }
    }
}
