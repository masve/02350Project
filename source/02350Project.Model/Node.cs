using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace _02350Project.Model
{
    public class Node /*: NotifyBase*/
    {
        private int id;

        [XmlAttribute]
        public int Id { get { return id; } set { id = value; } }

        /*
         * Coordinates
         */
        private int x;
        private int y;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        /*
         * Node Content
         */
        private string name;
        private string nodeSubText;
        private List<string> attributes;
        private List<string> methods;

        public string Name { get { return name; } set { name = value; } }
        public string NodeSubText { get { return nodeSubText; } set { nodeSubText = value; } }
        public List<string> Attributes { get { return attributes; } set { attributes = value; } }
        public List<string> Methods { get { return methods; } set { methods = value; } }
    }
}
