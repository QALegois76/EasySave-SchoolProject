using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibEasySave.MVVM_Job.Model
{
    class Tasks
    {

        private static JobMng _jobMng = null;
        private static Dictionary<Guid, Task> _tasks;


        public Tasks(JobMng jobMng)
        {
            if (jobMng == null)
            {
                Debug.Fail("JobMng is Null");
            }
            _jobMng = jobMng;
        }

        public void InitTasks()
        {
            if (_jobMng == null)
            {
                Debug.Fail("jobMng is null");
            }

            foreach (var t in _jobMng.Jobs)
            {
                _tasks.Add(t.Key, Task.Run(() => JobSaverStrategy.Save(t.Value)));
            } 
        }

        //public void BreakJob(EState state, Guid guid)
        //{
        //    if (_tasks.ContainsKey(guid))
        //    {
        //        Task task = _tasks[guid];
        //        Task.Start(() =>
        //        {
        //            switch (state)
        //            {
        //                case EState.Pause:
        //                    if (Thread.CurrentThread.ThreadState.ToString() == "Running")
        //                    {
        //                        try
        //                        {
        //                            //_playBreak.WaitOne();
        //                            //_playBreak.Set();
        //                            //task.Dispose(Timeout.Infinite).Wait();
        //                            Thread.CurrentThread.S

        //                        }
        //                        catch (ThreadInterruptedException)
        //                        {
        //                            Console.WriteLine("Thread '{0}' awoken.",
        //                                              Thread.CurrentThread.Name);
        //                        }
        //                        catch (ThreadAbortException)
        //                        {
        //                            Console.WriteLine("Thread '{0}' aborted.",
        //                                              Thread.CurrentThread.Name);
        //                        }
        //                    }
        //                    break;

        //                case EState.Stop:
        //                    if ((Thread.CurrentThread.ThreadState.ToString() == "Suspended") || (Thread.CurrentThread.ThreadState.ToString() == "Running"))
        //                    {
        //                        try
        //                        {
        //                            //Thread.ResetAbort();
        //                            Thread.CurrentThread.Abort();
        //                        }
        //                        catch (ThreadInterruptedException)
        //                        {
        //                            Console.WriteLine("Thread '{0}' awoken.",
        //                                              Thread.CurrentThread.Name);
        //                        }
        //                        catch (ThreadAbortException)
        //                        {
        //                            Console.WriteLine("Thread '{0}' aborted.",
        //                                              Thread.CurrentThread.Name);
        //                        }
        //                    }
        //                    break;
        //                case EState.Play:
        //                    if (Thread.CurrentThread.ThreadState.ToString() == "Suspended")
        //                    {
        //                        try
        //                        {
        //                            //_bigFile.WaitOne();
        //                            Thread.CurrentThread.Interrupt();
        //                        }
        //                        catch (ThreadInterruptedException)
        //                        {
        //                            Console.WriteLine("Thread '{0}' awoken.",
        //                                              Thread.CurrentThread.Name);
        //                        }
        //                        catch (ThreadAbortException)
        //                        {
        //                            Console.WriteLine("Thread '{0}' aborted.",
        //                                              Thread.CurrentThread.Name);
        //                        }
        //                    }
        //                    break;

        //                default:
        //                    break;

        //            }

        //        });
        //    }
        //}
    }


}
//if (_tasks.ContainsKey(guid))
//{
//    Task task = _tasks[guid];
//    await Task.Run(() =>
//    {

//        switch (state)
//        {
//            case EState.Break:
//                if (TaskStatus.Running == state)
//                {
//                    try
//                    {
//                        //_playBreak.WaitOne();
//                        //_playBreak.Set();
//                        Task.Delay(Timeout.Infinite).Wait();
//                    }
//                    catch (ThreadInterruptedException)
//                    {
//                        Console.WriteLine("Thread '{0}' awoken.",
//                                          task.);
//                    }
//                    catch (ThreadAbortException)
//                    {
//                        Console.WriteLine("Thread '{0}' aborted.",
//                                          Thread.CurrentThread.Name);
//                    }
//                }
//                break;

//            case EState.Stop:
//                if ((Thread.CurrentThread.ThreadState.ToString() == "Suspended") || (Thread.CurrentThread.ThreadState.ToString() == "Running"))
//                {

//                    try
//                    {
//                        using (var cts = new CancellationTokenSource())
//                        {
//                            Debug.Fail("Cancelling..");
//                            cts.Cancel();
//                            //Thread.ResetAbort();
//                        }
//                    }
//                    catch (ThreadInterruptedException)
//                    {
//                        Console.WriteLine("Thread '{0}' awoken.",
//                                          Thread.CurrentThread.Name);
//                    }
//                    catch (ThreadAbortException)
//                    {
//                        Console.WriteLine("Thread '{0}' aborted.",
//                                          Thread.CurrentThread.Name);
//                    }
//                }
//                break;
//            case EState.Play:
//                if (Thread.CurrentThread.ThreadState.ToString() == "Suspended")
//                {
//                    try
//                    {
//                        //_bigFile.WaitOne();
//                        Thread.CurrentThread.Interrupt();
//                    }
//                    catch (ThreadInterruptedException)
//                    {
//                        Console.WriteLine("Thread '{0}' awoken.",
//                                          Thread.CurrentThread.Name);
//                    }
//                    catch (ThreadAbortException)
//                    {
//                        Console.WriteLine("Thread '{0}' aborted.",
//                                          Thread.CurrentThread.Name);
//                    }
//                }
//                break;

//            default:
//                break;

//        }

//    });
//}
