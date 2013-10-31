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
        /*
         * Start and End Point 
         */
        private Node endA;
        private Node endB;

        public Node EndA { get { return endA; } set { endA = value; } }
        public Node EndB { get { return endB; } set { endB = value; } }

        /*
         * Type Definition
         */
        public enum typeEnum { DEP, GEN, AGG, ASS, COM };
        private typeEnum type;

        public typeEnum Type { get { return type; } set { type = value; } }
    }
}
