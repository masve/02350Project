using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _02350Project.Other
{
    class ConsolePrinter
    {
        public static void Write(string message)
        {
            AttachConsole(-1);
            Console.WriteLine(message);
        }
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);
    }
}
