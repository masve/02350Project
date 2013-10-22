using _02350Project.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350Project.Command
{
    class RemoveNodeCommand : IUndoRedoCommand
    {
        private ObservableCollection<Node> nodes;
        private ObservableCollection<Edge> edges;
        private Node removeNode;
        List<Edge> removeEdges = new List<Edge>();

        public RemoveNodeCommand(ObservableCollection<Node> _nodes, ObservableCollection<Edge> _edges, Node _nodeToRemove)
        {
            nodes = _nodes;
            edges = _edges;
            removeNode = _nodeToRemove;

            /* Find all edges who are connected to the node-to-be-removed */
            foreach (Edge e in edges)
                if (e.EndA.Equals(removeNode))
                    removeEdges.Add(e);
                else if (e.EndB.Equals(removeNode))
                    removeEdges.Add(e);
        }

        public void Execute()
        {
            foreach (Edge e in removeEdges)
                edges.Remove(e);
            nodes.Remove(removeNode);
        }



        public void UnExecute()
        {
            nodes.Add(removeNode);
            foreach (Edge e in removeEdges)
            {
                if (e.EndA == null) e.EndA = removeNode;
                if (e.EndB == null) e.EndB = removeNode;
                edges.Add(e);
            }
        }
    }
}
