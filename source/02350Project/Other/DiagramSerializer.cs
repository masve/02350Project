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
using System.Windows;

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
            serializer.UnknownNode += new XmlNodeEventHandler(UnknowNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(UnknowAttribute);
            serializer.UnknownElement += new XmlElementEventHandler(UnknowElement);
            FileStream reader = new FileStream(path, FileMode.Open);
            try
            {
                diagram = (Diagram)serializer.Deserialize(reader);
            }
            catch (InvalidOperationException e)
            {
                DiagramSerializer.error("Invalid Operation Exception");
            }
            finally
            {
                reader.Close();
            }
            return diagram;
        }

        protected static void UnknowNode(Object sender, XmlNodeEventArgs e)
        {
            Other.ConsolePrinter.Write("Node Error: " + e.Name);
            DiagramSerializer.error(e.Name);
        }

        private static void UnknowAttribute(Object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Other.ConsolePrinter.Write("Attribute error: " + attr.Name);
            DiagramSerializer.error(attr.Name);
        }
        private static void UnknowElement(Object sender, XmlElementEventArgs e)
        {
            Other.ConsolePrinter.Write("Element error: " + e.Element.Name);
            DiagramSerializer.error(e.Element.Name);
        }

        private static void error(string e)
        {
            string messageBoxText = "Invalid or corrupt XML file\nError: " + e;
            string caption = "XML error";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(messageBoxText, caption, button, icon);
        }

    }
}
