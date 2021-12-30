using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//using System.Management;

namespace MFD_Thingy.API
{
    // Tried using this as something that might be faster than Syste.Diagnostic, but in this case it's the same. This also only works on Windows and not Linux.
    public partial class MT_SimKey
    {
        //const string G_ProcessQuery = "SELECT * FROM Win32_PerfFormattedData_PerfProc_Process WHERE Name <> '_Total' AND Name <> 'Idle'";

        //public bool _CanRunWMI()
        //{

        //    IntPtr L_ActivatedHandle = GetForegroundWindow();
        //    if (L_ActivatedHandle == IntPtr.Zero)
        //    {
        //        return false;       // No window is currently activated
        //    }

        //    List<cl_Process> L_cl_Processes = _GetAll_PR();

        //    cl_Process L_cl_Process = L_cl_Processes.Where(p => p.Name.Equals("notepad") || p.Name.Equals("starcitizen")).FirstOrDefault();

        //    if (L_cl_Process != null)
        //    {
        //        return _CanRun_CheckProcessActive(L_cl_Process.ProcessId, L_ActivatedHandle);
        //    }

        //    return false;
        //}

        

        //private List<cl_Process> _GetAll_PR()
        //{
        //    List<cl_Process> L_Info = new List<cl_Process>();

        //    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", G_ProcessQuery);

        //    foreach (ManagementObject queryObj in searcher.Get())
        //    {

        //        //StreamWriter SW;
        //        //SW = File.CreateText("c:\\MyTextFile.txt");//I can't get the StreamWriter to work

        //        //SW.WriteLine("Name: ", queryObj["Name"]);//This fails in that it does not write to the file and I don't understand why
        //        //Console.WriteLine("ProcessID: {0}", queryObj["IDProcess"]);
        //        //Console.WriteLine("Handles: {0}", queryObj["HandleCount"]);
        //        //Console.WriteLine("Threads: {0}", queryObj["ThreadCount"]);
        //        //Console.WriteLine("Memory: {0}", queryObj["WorkingSetPrivate"]);
        //        //Console.WriteLine("CPU%: {0}", queryObj["PercentProcessorTime"]);
        //        //Console.Read();
        //        //SW.Close();

        //        L_Info.Add(new cl_Process()
        //        {
        //            Name = queryObj["Name"].ToString(),
        //            ProcessId = Convert.ToInt32(queryObj["IDProcess"])
        //        });

        //    }

        //    return L_Info;
        //}

        //public class cl_Process
        //{
        //    public int ProcessId { get; set; }
        //    public string Name { get; set; }
        //}
    }
}
