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
         * Coordinates and dimensions
         */
        private double x;
        private double y;


        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }

        private string name;
        private string nodeSubText;
        private List<string> attributes;
        private List<string> methods;

        public string Name { get { return name; } set { name = value; } }
        public string NodeSubText { get { return nodeSubText; } set { nodeSubText = value; } }
        public List<string> Attributes { get { return attributes; } set { attributes = value; } }
        public List<string> Methods { get { return methods; } set { methods = value; } }

        //public Node()
        //{
        //    NodeCollapsed = true;
        //    AttCollapsed = true;
        //    MetCollapsed = true;
        //    X = Y = 50;
        //    Height = 100;
        //    Width = 100;
        //    Name = "test";
        //}

        public static void WriteToConsole(string message)
        {
            AttachConsole(-1);
            Console.WriteLine(message);
        }
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);
    }
}
