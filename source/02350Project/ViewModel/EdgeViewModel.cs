using _02350Project.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350Project.ViewModel
{
    public class EdgeViewModel : ViewModelBase
    {
        private Edge edge;

        private NodeViewModel vMEndA;
        private NodeViewModel vMEndB;

        private string type;
        private bool dash;

        public enum ANCHOR { NORTH, SOUTH, WEST, EAST };

        private ANCHOR anchorA;
        private ANCHOR anchorB;

        private double aX, aY, bX, bY;

        public EdgeViewModel(Node fromNode, Node toNode, NodeViewModel fromVM, NodeViewModel toVM)
        {
            edge = new Edge();
            EndA = fromNode;
            EndB = toNode;
            VMEndA = fromVM;
            VMEndB = toVM;
            Other.ConsolePrinter.Write("Edge VM");
            AX = AY = 200;
            BX = BY = 300;
            Other.ConsolePrinter.Write(fromNode.Name + " + " + toNode.Name);
        }

        public NodeViewModel VMEndA { get { return vMEndA; } set { vMEndA = value; RaisePropertyChanged("VMEndA"); } }
        public NodeViewModel VMEndB { get { return vMEndB; } set { vMEndB = value; RaisePropertyChanged("VMEndB"); } }
        public Node EndA { get { return edge.EndA; } set { edge.EndA = value; RaisePropertyChanged("EndA"); } }
        public Node EndB { get { return edge.EndB; } set { edge.EndB = value; RaisePropertyChanged("EndB"); } }

        public string Type { get { return type; } set { type = value; RaisePropertyChanged("Type"); } }

        public bool Dash { get { return dash; } set { dash = value; RaisePropertyChanged("Dash"); } }

        public ANCHOR AnchorA { get { return anchorA; } set { anchorA = value; RaisePropertyChanged("AnchorA"); RaisePropertyChanged("AX"); RaisePropertyChanged("AY"); } }
        public ANCHOR AnchorB { get { return anchorB; } set { anchorB = value; RaisePropertyChanged("AnchorB"); RaisePropertyChanged("BX"); RaisePropertyChanged("BY"); } }

        public double AX { get { return aX; } set { aX = value; RaisePropertyChanged("AX"); } }
        public double AY { get { return aY; } set { aY = value; RaisePropertyChanged("AY"); } }
        public double BX { get { return bX; } set { bX = value; RaisePropertyChanged("BX"); } }
        public double BY { get { return bY; } set { bY = value; RaisePropertyChanged("BY"); } }

    }
}
