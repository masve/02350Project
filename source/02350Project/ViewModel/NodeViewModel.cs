using _02350Project.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _02350Project.ViewModel
{
    class NodeViewModel : ViewModelBase
    {
        private Node node;

        private int x;
        private int y;
        private int height;
        private int width;

        private Point north;
        private Point south;
        private Point east;
        private Point west;

        private string name;
        private bool noneFlag;
        private bool abstractFlag;
        private bool interfaceFlag;
        private string nodeSubText;
        private List<string> attributes;
        private List<string> methods;

        private bool nodeCollapsed;
        private bool attCollapsed;
        private bool metCollapsed;
        private bool isSelected;

        public NodeViewModel(Node _node)
        {
            node = _node;
        }

        public double X { get { return node.X; } set { node.X = value; RaisePropertyChanged("X"); } }
        public double Y { get { return node.Y; } set { node.Y = value; RaisePropertyChanged()} }
        //public int Height { get { return node.Height; } set { node.Height = value; } }
        //public int Width { get { return node.Width; } set { node.Width = value; } }

        public string Name { get { return node.Name; } set { node.Name = value; } }
        public string NodeSubText { get { return node.NodeSubText; } set { node.NodeSubText =  } }
        public string NodeSubText { get {  } }






    }
}
