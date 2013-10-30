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
         */
        private double height;
        private double width;

        public double X { get { return node.X; } set { node.X = (int)value; RaisePropertyChanged("X"); RaisePropertyChanged("CanvasCenterX"); RaisePropertyChanged("North"); RaisePropertyChanged("South"); RaisePropertyChanged("East"); RaisePropertyChanged("West"); } }
        public double Y { get { return node.Y; } set { node.Y = (int)value; RaisePropertyChanged("Y"); RaisePropertyChanged("CanvasCenterY"); RaisePropertyChanged("North"); RaisePropertyChanged("South"); RaisePropertyChanged("East"); RaisePropertyChanged("West"); } }
        public double Height { get { return height; } set { height = value; RaisePropertyChanged("Height"); RaisePropertyChanged("North"); RaisePropertyChanged("South"); RaisePropertyChanged("East"); RaisePropertyChanged("West"); } }
        public double Width { get { return width; } set { width = value; RaisePropertyChanged("Width"); RaisePropertyChanged("North"); RaisePropertyChanged("South"); RaisePropertyChanged("East"); RaisePropertyChanged("West"); } }

        public double CanvasCenterX { get { return node.X + Width / 2; } set { node.X = (int)value - (int)Width / 2; RaisePropertyChanged("X"); RaisePropertyChanged("Width"); RaisePropertyChanged("North"); RaisePropertyChanged("South"); RaisePropertyChanged("East"); RaisePropertyChanged("West"); } }
        public double CanvasCenterY { get { return node.Y + Height / 2; } set { node.Y = (int)value - (int)Height / 2; RaisePropertyChanged("Y"); RaisePropertyChanged("Height"); RaisePropertyChanged("North"); RaisePropertyChanged("South"); RaisePropertyChanged("East"); RaisePropertyChanged("West"); } }

        /*
         * Edge anchor points
         */
        private Point north;
        private Point south;
        private Point east;
        private Point west;

        public Point North { get { north.X = node.X + Width / 2; north.Y = node.Y; return north; } set { north.X = node.X + Width / 2; north.Y = node.Y; RaisePropertyChanged("North"); } }
        public Point South { get { south.X = node.X + Width / 2; south.Y = node.Y + Height; return south; } set { south.X = node.X + Width / 2; south.Y = node.Y + Height; RaisePropertyChanged("South"); } }
        public Point East { get { east.X = node.X + Width; east.Y = node.Y + Height / 2; return east; } set { east.X = node.X + Width; east.Y = node.Y + Height / 2; RaisePropertyChanged("East"); } }
        public Point West { get { west.X = node.X; west.Y = node.Y + Height / 2; return west; } set { west.X = node.X; west.Y = node.Y + Height / 2; RaisePropertyChanged("West"); } }

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
            get
            {
                if (AbstractFlag == true)
                    node.NodeSubText = "Abstract class";
                else if (InterfaceFlag == true)
                    node.NodeSubText = "Interface";
                else
                    node.NodeSubText = "Class";
                return node.NodeSubText;
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

        public EdgeViewModel newEdge(NodeViewModel fromNode, string type)
        {
            EdgeViewModel newEdge = new EdgeViewModel(fromNode.node, this.node, fromNode, this, type);
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

        }

        public void Resize(double h, double w)
        {
            Height = h;
            Width = w;
        }

        #region Dynamic Anchorpoint Calculations
        public enum ANCHOR { NORTH, SOUTH, EAST, WEST };
        private double northEast = -1.0 * Math.PI / 4.0;
        private double northWest = -3.0 * Math.PI / 4.0;
        private double southEast = Math.PI / 4.0;
        private double southWest = 3.0 * Math.PI / 4.0;

        //public void CalculateAnchor(NodeViewModel node)
        //{
        //    foreach (EdgeViewModel e in Edges)
        //    {
        //        if (e.VMEndA.Equals(node))
        //        {
        //            setEnds(e);

        //        }
        //        else if (e.VMEndB.Equals(node))
        //        {
        //            setEnds(e);
        //        }
        //    }
        //}

        public void setEnds(EdgeViewModel e)
        {
            ANCHOR A = findAnchor(e.VMEndA.CanvasCenterX, e.VMEndA.CanvasCenterY, e.VMEndB.CanvasCenterX, e.VMEndB.CanvasCenterY);
            ANCHOR B = findAnchor(e.VMEndB.CanvasCenterX, e.VMEndB.CanvasCenterY, e.VMEndA.CanvasCenterX, e.VMEndA.CanvasCenterY);
            switch (A)
            {
                case ANCHOR.NORTH:
                    e.AX = e.VMEndA.North.X;
                    e.AY = e.VMEndA.North.Y;
                    break;
                case ANCHOR.EAST:
                    e.AX = e.VMEndA.East.X;
                    e.AY = e.VMEndA.East.Y;

                    break;
                case ANCHOR.SOUTH:
                    e.AX = e.VMEndA.South.X;
                    e.AY = e.VMEndA.South.Y;
                    break;
                case ANCHOR.WEST:
                    e.AX = e.VMEndA.West.X;
                    e.AY = e.VMEndA.West.Y;
                    break;
            }
            switch (B)
            {
                case ANCHOR.NORTH:
                    e.BX = e.VMEndB.North.X;
                    e.BY = e.VMEndB.North.Y;
                    break;
                case ANCHOR.EAST:
                    e.BX = e.VMEndB.East.X;
                    e.BY = e.VMEndB.East.Y;

                    break;
                case ANCHOR.SOUTH:
                    e.BX = e.VMEndB.South.X;
                    e.BY = e.VMEndB.South.Y;

                    break;
                case ANCHOR.WEST:
                    e.BX = e.VMEndB.West.X;
                    e.BY = e.VMEndB.West.Y;
                    break;
            }
        }

        private ANCHOR findAnchor(double x1, double y1, double x2, double y2)
        {
            double deltaX = x2 - x1;
            double deltaY = y2 - y1;


            double angle = Math.Atan2(deltaY, deltaX);


            if (northEast > angle && angle >= northWest)
            {
                return ANCHOR.NORTH;

            }
            else if (southWest > angle && angle >= southEast)
            {
                return ANCHOR.SOUTH;

            }
            else if (southEast > angle && angle >= northEast)
            {
                return ANCHOR.EAST;

            }
            else if (northWest > angle || angle >= southWest)
            {
                return ANCHOR.WEST;

            }
            return ANCHOR.NORTH;
        }

        #endregion

    }
}
