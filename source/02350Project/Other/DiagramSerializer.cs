using _02350Project.Model;
using _02350Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace _02350Project.Other
{
    public class DiagramSerializer
    {
        //private static Data instance;

        //private Data() { }

        //public static Data Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new Data();
        //        }
        //        return instance;
        //    }
        //}

        public class Diagram
        {
            public List<Node> Nodes = new List<Node>();
            public List<Edge> Edges = new List<Edge>();
        }

        /// <summary>
        /// Saves the program state to a file.
        /// </summary>
        /// <param name="nodeVMs"></param>
        /// <param name="edgeVMs"></param>
        /// <param name="path"></param>
        public static void Save(List<NodeViewModel> nodeVMs, List<EdgeViewModel> edgeVMs, string path)
        {
            Diagram diagram = new Diagram();

            foreach (NodeViewModel n in nodeVMs)
                diagram.Nodes.Add(n.getNode());
            foreach (EdgeViewModel e in edgeVMs)
                diagram.Edges.Add(e.getEdge());

            XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
            TextWriter writer = new StreamWriter(path);

            serializer.Serialize(writer, diagram);
            writer.Close();
        }

        /// <summary>
        /// Loads the program state from a file.
        /// </summary>
        /// <param name="path"></param>
        public static void Load(string path)
        {
            Diagram diagram = new Diagram();
            XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
            TextReader reader = new StreamReader(path);
            diagram = (Diagram)serializer.Deserialize(reader);
            reader.Close();

        }
    }
}
