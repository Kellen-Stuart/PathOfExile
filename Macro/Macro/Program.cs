using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Macro
{
    static class Program
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static IntPtr _hookID = IntPtr.Zero;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static Process _pathOfExileProcess;
        private const string poeProcessName = "PathOfExile";
        private static Random _random = new Random();
        private static string[] _keys = {"{1}", "{2}", "{3}", "{5}"};
        private static int _randomWait => _random.Next(0, 100);
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var processes = Process.GetProcessesByName(poeProcessName);
            if (!processes.Any())
                throw new Exception($"{poeProcessName} must be running to execute this macro.");

            _pathOfExileProcess = processes.Single();
            
            _hookID = SetHook(_proc);
            // Application.SetHighDpiMode(HighDpiMode.SystemAware);
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }
        
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process poeProcess = Process.GetProcessesByName("pathofexile").Single())
            using (ProcessModule poeModule = poeProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(poeModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                #if DEBUG
                Console.WriteLine((Keys)vkCode);
                #endif
                if ((Keys) vkCode == Keys.D4)
                {
                    SetForegroundWindow(_pathOfExileProcess.MainWindowHandle);
                    foreach (var key in _keys)
                    {
                        SendKeys.SendWait(key);
                        Thread.Sleep(_randomWait);
                    }
                }

                if ((Keys) vkCode == Keys.D6)
                {
                    _pathOfExileProcess.Kill();
                    Application.Exit();
                }
                    
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}