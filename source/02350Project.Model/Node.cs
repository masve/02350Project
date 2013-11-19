using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace _02350Project.Model
{
    public class Node /*: NotifyBase*/
    {
        [XmlAttribute]
        public int Id { get; set; }

        /*
         * Coordinates
         */
        public int X { get; set; }
        public int Y { get; set; }

        /*
         * Node Content
         */
        public string Name { get; set; }
        public NodeType NodeType { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Methods { get; set; }
    }
}
