using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350Project.Model
{
    public class Edge : NotifyBase
    {
        private Node endA;
        private Node endB;

        public Node EndA { get { return endA; } set { endA = value; NotifyPropertyChanged("EndA"); } }
        public Node EndB { get { return endB; } set { endB = value; NotifyPropertyChanged("EndB"); } }

        //public Edge(Node _endA, Node _endB)
        //{
        //    EndA = _endA;
        //    EndB = _endB;
        //}
    }
}
