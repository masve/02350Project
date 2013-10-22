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
        private int x;
        private int y;
        private int height;
        private int width;

        public int X { get { return x; } set { x = value; NotifyPropertyChanged("X"); NotifyPropertyChanged("CanvasCenterX"); NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West"); } }
        public int Y { get { return y; } set { y = value; NotifyPropertyChanged("Y"); NotifyPropertyChanged("CanvasCenterY"); NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West"); } }
        public int Height { get { return height; } set { height = value; NotifyPropertyChanged("Height"); NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West"); } }
        public int Width { get { return width; } set { width = value; NotifyPropertyChanged("Width"); NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West"); } }

        public int CanvasCenterX { get { return X + Width / 2; } set { X = value - Width / 2; NotifyPropertyChanged("X"); NotifyPropertyChanged("Width"); } }
        public int CanvasCenterY { get { return Y + Height / 2; } set { Y = value - Height / 2; NotifyPropertyChanged("Y"); NotifyPropertyChanged("Height"); } }

        /*
         * Edge anchor points
         */
        private Point north;
        private Point south;
        private Point east;
        private Point west;

        public Point North { get { north.X = X + Width / 2; north.Y = Y; return north; } set { north.X = X + Width / 2; north.Y = Y; } }
        public Point South { get { south.X = X + Width / 2; south.Y = Y + Height; return south; } set { south.X = X + Width / 2; south.Y = Y + Height; } }
        public Point East { get { east.X = X + Width; east.Y = Y + Height / 2; return east; } set { east.X = X + Width; east.Y = Y + Height / 2; } }
        public Point West { get { west.X = X; west.Y = Y + Height / 2; return west; } set { west.X = X; west.Y = Y + Height / 2; } }

        /*
         * Usercontrol contents
         */
        private string name;
        private bool noneFlag;
        private bool abstractFlag;
        private bool interfaceFlag;
        private string nodeSubText;
        private List<string> attributes;
        private List<string> methods;

        public string Name { get { return name; } set { name = value; NotifyPropertyChanged("Name"); NotifyPropertyChanged("NodeSubText"); } }
        public bool NoneFlag { get { return noneFlag; } set { noneFlag = value; NotifyPropertyChanged("NoneFlag"); NotifyPropertyChanged("NodeSubText"); } }
        public bool AbstractFlag { get { return abstractFlag; } set { abstractFlag = value; NotifyPropertyChanged("AbstractFlag"); NotifyPropertyChanged("NodeSubText"); } }
        public bool InterfaceFlag { get { return interfaceFlag; } set { interfaceFlag = value; NotifyPropertyChanged("InterfaceFlag"); NotifyPropertyChanged("NodeSubText"); } }
        public string NodeSubText { get { if (AbstractFlag == true) nodeSubText = "Abstract class"; else if (InterfaceFlag == true) nodeSubText = "Interface"; else nodeSubText = "Class"; return nodeSubText; } }
        public List<string> Attributes { get { return attributes; } set { attributes = value; NotifyPropertyChanged("Attributes"); } }
        public List<string> Methods { get { return methods; } set { methods = value; NotifyPropertyChanged("Methods"); } }

        /*
         * Usercontrol visual cues
         */
        private bool nodeCollapsed;
        private bool attCollapsed;
        private bool metCollapsed;
        private bool isSelected;

        public bool NodeCollapsed { get { return nodeCollapsed; } set { nodeCollapsed = value; NotifyPropertyChanged("NodeCollapsed"); } }
        public bool AttCollapsed { get { return attCollapsed; } set { attCollapsed = value; NotifyPropertyChanged("AttCollapsed"); } }
        public bool MetCollapsed { get { return metCollapsed; } set { metCollapsed = value; NotifyPropertyChanged("MetCollapsed"); } }
        public bool IsSelected { get { return isSelected; } set { isSelected = value; NotifyPropertyChanged("IsSelected"); } }

        public Node()
        {
            NodeCollapsed = true;
            AttCollapsed = true;
            MetCollapsed = true;
            X = Y = 50;
            Height = 100;
            Width = 100;
            Name = "test";
        }

        public static void WriteToConsole(string message)
        {
            AttachConsole(-1);
            Console.WriteLine(message);
        }
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);
    }
}
