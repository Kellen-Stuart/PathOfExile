using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace PathOfExile
{
    public class Program
    {
        const UInt32 WM_KEYDOWN = 0x0100;
        const int VK_M = 0x4D;

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [STAThread]
        static void Main()
        {
            while (true)
            {
                Process[] processes = Process.GetProcessesByName("notepad");

                foreach (Process proc in processes)
                    PostMessage(proc.MainWindowHandle, WM_KEYDOWN, VK_M, 0);

                Thread.Sleep(5000);
            }
        }
    }
}