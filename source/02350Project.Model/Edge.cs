using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace _02350Project.Model
{
    public class Edge : NotifyBase
    {
        private int nodeIdA;
        private int nodeIdB;

        public int NodeIdA { get { return nodeIdA; } set { nodeIdA = value; } }
        public int NodeIdB { get { return nodeIdB; } set { nodeIdB = value; } }

        /*
         * Start and End Point 
         */
        private Node endA;
        private Node endB;

        [XmlIgnore]
        public Node EndA { get { return endA; } set { endA = value; NodeIdA = value.Id; } }
        [XmlIgnore]
        public Node EndB { get { return endB; } set { endB = value; NodeIdB = value.Id; } }

        /*
         * Type Definition
         */
        private EdgeType type;

        public EdgeType Type { get { return type; } set { type = value; } }
    }
}
