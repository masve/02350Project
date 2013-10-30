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

        public enum typeEnum { DEPENDENCY, INHERITANCE };
        private typeEnum type = typeEnum.DEPENDENCY;

        public Node EndA { get { return endA; } set { endA = value; } }
        public Node EndB { get { return endB; } set { endB = value; } }

        public typeEnum Type { get { return type; } set { type = value; } }

    }
}
