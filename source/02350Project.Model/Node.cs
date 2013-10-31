using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _02350Project.Model
{
    public class Node : NotifyBase
    {
        /*
         * Coordinates
         */
        private int x;
        private int y;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        /*
         * Node Content
         */
        private string name;
        private string nodeSubText;
        private List<string> attributes;
        private List<string> methods;

        public string Name { get { return name; } set { name = value; } }
        public string NodeSubText { get { return nodeSubText; } set { nodeSubText = value; } }
        public List<string> Attributes { get { return attributes; } set { attributes = value; } }
        public List<string> Methods { get { return methods; } set { methods = value; } }

        #region ConsoleDebugger
        public static void WriteToConsole(string message)
        {
            AttachConsole(-1);
            Console.WriteLine(message);
        }
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);
        #endregion
    }
}
