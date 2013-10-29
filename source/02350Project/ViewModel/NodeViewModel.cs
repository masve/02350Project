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
    public class NodeViewModel : ViewModelBase
    {
        private Node node;

        /*
 * Coordinates and dimensions
         * 
         * 
 */
        private double height;
        private double width;

        //private double canvasCenterX;
        //private double canvasCenterY;


        public double X { get { return node.X; } set { node.X = value; RaisePropertyChanged("X"); RaisePropertyChanged("CanvasCenterX"); RaisePropertyChanged("North"); RaisePropertyChanged("South"); RaisePropertyChanged("East"); RaisePropertyChanged("West"); } }
        public double Y { get { return node.Y; } set { node.Y = value; RaisePropertyChanged("Y"); RaisePropertyChanged("CanvasCenterY"); RaisePropertyChanged("North"); RaisePropertyChanged("South"); RaisePropertyChanged("East"); RaisePropertyChanged("West"); } }
        public double Height { get { return height; } set { height = value; RaisePropertyChanged("Height"); RaisePropertyChanged("North"); RaisePropertyChanged("South"); RaisePropertyChanged("East"); RaisePropertyChanged("West"); } }
        public double Width { get { return width; } set { width = value; RaisePropertyChanged("Width"); RaisePropertyChanged("North"); RaisePropertyChanged("South"); RaisePropertyChanged("East"); RaisePropertyChanged("West"); } }

        public double CanvasCenterX { get { return node.X + Width / 2; } set { node.X = value - Width / 2; RaisePropertyChanged("X"); RaisePropertyChanged("Width"); } }
        public double CanvasCenterY { get { return node.Y + Height / 2; } set { node.Y = value - Height / 2; RaisePropertyChanged("Y"); RaisePropertyChanged("Height"); } }

        /*
         * Edge anchor points
         */
        private Point north;
        private Point south;
        private Point east;
        private Point west;

        public Point North { get { north.X = node.X + Width / 2; north.Y = node.Y; return north; } set { north.X = node.X + Width / 2; north.Y = node.Y; } }
        public Point South { get { south.X = node.X + Width / 2; south.Y = node.Y + Height; return south; } set { south.X = node.X + Width / 2; south.Y = node.Y + Height; } }
        public Point East { get { east.X = node.X + Width; east.Y = node.Y + Height / 2; return east; } set { east.X = node.X + Width; east.Y = node.Y + Height / 2; } }
        public Point West { get { west.X = node.X; west.Y = node.Y + Height / 2; return west; } set { west.X = node.X; west.Y = node.Y + Height / 2; } }

        /*
         * Usercontrol contents
         */

        private bool noneFlag;
        private bool abstractFlag;
        private bool interfaceFlag;


        public string Name { get { return node.Name; } set { node.Name = value; RaisePropertyChanged("Name"); RaisePropertyChanged("NodeSubText"); } }
        public bool NoneFlag { get { return noneFlag; } set { noneFlag = value; RaisePropertyChanged("NoneFlag"); RaisePropertyChanged("NodeSubText"); } }
        public bool AbstractFlag { get { return abstractFlag; } set { abstractFlag = value; RaisePropertyChanged("AbstractFlag"); RaisePropertyChanged("NodeSubText"); } }
        public bool InterfaceFlag { get { return interfaceFlag; } set { interfaceFlag = value; RaisePropertyChanged("InterfaceFlag"); RaisePropertyChanged("NodeSubText"); } }
        public string NodeSubText
        {
            get { return node.NodeSubText; }
            set
            {
                if (AbstractFlag == true)
                    node.NodeSubText = "Abstract class";
                else if (InterfaceFlag == true)
                    node.NodeSubText = "Interface";
                else
                    node.NodeSubText = "Class";
            }
        }

        public List<string> Attributes { get { return node.Attributes; } set { node.Attributes = value; RaisePropertyChanged("Attributes"); } }
        public List<string> Methods { get { return node.Methods; } set { node.Methods = value; RaisePropertyChanged("Methods"); } }

        /*
         * Usercontrol visual cues
         */
        private bool nodeCollapsed;
        private bool attCollapsed;
        private bool metCollapsed;
        private bool isSelected;

        public bool NodeCollapsed { get { return nodeCollapsed; } set { nodeCollapsed = value; RaisePropertyChanged("NodeCollapsed"); } }
        public bool AttCollapsed { get { return attCollapsed; } set { attCollapsed = value; RaisePropertyChanged("AttCollapsed"); } }
        public bool MetCollapsed { get { return metCollapsed; } set { metCollapsed = value; RaisePropertyChanged("MetCollapsed"); } }
        public bool IsSelected { get { return isSelected; } set { isSelected = value; RaisePropertyChanged("IsSelected"); } }

        public EdgeViewModel newEdge(NodeViewModel fromNode)
        {
            EdgeViewModel newEdge = new EdgeViewModel(fromNode.node, this.node, fromNode, this);
            //newEdge.VMEndA = fromNode;
            //newEdge.EndA = fromNode.node;
            //newEdge.VMEndB = this;
            //newEdge.EndB = this.node; //TODO spørg bo om dette er korrekt

            return newEdge;
        }

        public NodeViewModel()
        {
            node = new Node();
            NodeCollapsed = true;
            AttCollapsed = true;
            MetCollapsed = true;
            Name = "placeholder";
            NodeSubText = "";
        }

    }
}
