using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MFD_Thingy.API
{
    public partial class MT_SimKey
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        public bool _CanRun()
        {

            IntPtr L_ActivatedHandle = GetForegroundWindow();
            if (L_ActivatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }


            Process[] processCollection = Process.GetProcesses();
            for (int p = 0; p < processCollection.Count(); p++)
            {
                if (_CanRun_CheckProcess(processCollection[p], L_ActivatedHandle) == true)
                {
                    return true;
                }
            }

            return false;
        }

        private bool _CanRun_CheckProcess(Process P_Process, IntPtr P_ActivatedHandle)
        {
            if (_CanRun_CheckProcessModule(P_Process) == true)
            {
                switch (P_Process.MainModule.ModuleName)
                {
                    case "notepad.exe":
                        return _CanRun_CheckProcessActive(P_Process.Id, P_ActivatedHandle);

                    case "starcitizen.exe":
                        return _CanRun_CheckProcessActive(P_Process.Id, P_ActivatedHandle);

                }
            }

            return false;
        }

        private bool _CanRun_CheckProcessActive(int P_ProcessId, IntPtr P_ActivatedHandle)
        {
            int activeProcId;
            GetWindowThreadProcessId(P_ActivatedHandle, out activeProcId);

            return activeProcId == P_ProcessId;
        }

        private bool _CanRun_CheckProcessModule(Process P_Process)
        {
            try
            {
                if (P_Process.MainModule == null)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
