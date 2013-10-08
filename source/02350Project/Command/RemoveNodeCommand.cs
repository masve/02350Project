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
    class RemoveNodeCommand
    {
        private ObservableCollection<Node> nodes;
        private ObservableCollection<Edge> edges;
        private Node nodeToRemove;
        List<Edge> edgesToRemove = new List<Edge>();

        public RemoveNodeCommand(ObservableCollection<Node> _nodes, ObservableCollection<Edge> _edges, Node _nodeToRemove)
        {
            nodes = _nodes;
            edges = _edges;
            nodeToRemove = _nodeToRemove;

            /* Find all edges who are connected to the node-to-be-removed */
            foreach (Edge e in edges)
                if (e.EndA.Equals(nodeToRemove))
                    edgesToRemove.Add(e);
                else if (e.EndB.Equals(nodeToRemove))
                    edgesToRemove.Add(e);
        }

        public void Execute()
        {
            foreach (Edge e in edgesToRemove)
                edges.Remove(e);
            nodes.Remove(nodeToRemove);
        }

    }
}
