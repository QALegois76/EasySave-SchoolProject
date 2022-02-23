using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace LibEasySave.ProcessMng
{
    public static class InstanceIsRunning
    {
        public static bool IsRunning(string nomProcess)
        {
            Process[] process = Process.GetProcesses();
            foreach (Process leProcess in process)
            {
                if (leProcess.ProcessName == nomProcess)
                {
                    Process[] listProcessByName = Process.GetProcessesByName(nomProcess);
                    int nbProcess = 0;
                    foreach (Process processByName in listProcessByName)
                    {
                        nbProcess++;
                        if (nbProcess > 1) { return true; }
                    }
                    
                }
            }
            return false;
        }
    }
}
