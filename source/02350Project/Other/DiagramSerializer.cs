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
        public static async Task<bool> Save(List<NodeViewModel> nodeVMs, List<EdgeViewModel> edgeVMs, string path)
        {
            Diagram diagram = new Diagram();
            
            diagram = await GetDiagramAsync(diagram, nodeVMs, edgeVMs);
            var b = await StoreDiagramAsync(diagram, path);

            return true;
        }
        public static Task<Diagram> GetDiagramAsync(Diagram diagram, List<NodeViewModel> nodeVMs, List<EdgeViewModel> edgeVMs)
        {
            return Task.Run<Diagram>(() => GetDiagram(diagram, nodeVMs, edgeVMs));
        }

        public static Diagram GetDiagram(Diagram diagram, List<NodeViewModel> nodeVMs, List<EdgeViewModel> edgeVMs)
        {
            foreach (NodeViewModel n in nodeVMs)
                diagram.Nodes.Add(n.getNode());
            foreach (EdgeViewModel e in edgeVMs)
                diagram.Edges.Add(e.getEdge());

            return diagram;
        }

        private static Task<bool> StoreDiagramAsync(Diagram diagram, string path)
        {
            return Task.Run<bool>(() => StoreDiagram(diagram, path));
        }

        private static bool StoreDiagram(Diagram diagram, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
            TextWriter writer = new StreamWriter(path);

            //TextWriter writer = new FileStream(path,FileAccess.Write);
            //FileStream FileStream = new 
            try
            {
                serializer.Serialize(writer, diagram);
            }
            catch (IOException e)
            {
                return false;
            }
            finally
            {
                writer.Close();
            }
            return true;
            
        }

        /// <summary>
        /// Loads the program state from a file.
        /// </summary>
        /// <param name="path"></param>
        public static Diagram Load(string path)
        {
            Diagram diagram = new Diagram();
            XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
            TextReader reader = new StreamReader(path);
            diagram = (Diagram)serializer.Deserialize(reader);
            reader.Close();
            return diagram;

        }
        public static void j()
        {
            while (true)
            {

            }
        }
    }
}
