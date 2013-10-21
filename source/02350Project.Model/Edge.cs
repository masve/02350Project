using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _02350Project.Model
{
    public class Edge : NotifyBase
    {
        private Node endA;
        private Node endB;
        private bool dash;
        private enum ANCHOR { NORTH, SOUTH, WEST, EAST };
        //private ANCHOR anchorA;
        //private ANCHOR anchorB;

        private int aX, aY, bX, bY;

        public Node EndA { get { return endA; } set { endA = value; NotifyPropertyChanged("EndA"); } }
        public Node EndB { get { return endB; } set { endB = value; NotifyPropertyChanged("EndB"); } }

        public bool Dash { get { return dash; } set { dash = value; NotifyPropertyChanged("Dash"); } }

        //public ANCHOR AnchorA { get { return anchorA; } set { anchorA = value; NotifyPropertyChanged("AnchorA"); NotifyPropertyChanged("AX"); NotifyPropertyChanged("AY"); } }
        //public ANCHOR AnchorB { get { return anchorB; } set { anchorB = value; NotifyPropertyChanged("AnchorB"); NotifyPropertyChanged("BX"); NotifyPropertyChanged("BY"); } }

        public int AX { get { return aX; } set { aX = value; NotifyPropertyChanged("AX"); } }
        public int AY { get { return aY; } set { aY = value; NotifyPropertyChanged("AY"); } }
        public int BX { get { return bX; } set { bX = value; NotifyPropertyChanged("BX"); } }
        public int BY { get { return bY; } set { bY = value; NotifyPropertyChanged("BY"); } }

        public Edge()
        {
            Dash = true;
        }
    }
}
