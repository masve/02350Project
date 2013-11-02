using _02350Project.ViewModel;
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
        private ObservableCollection<NodeViewModel> nodes;
        private ObservableCollection<EdgeViewModel> edges;
        private NodeViewModel removeNode;
        List<EdgeViewModel> removeEdges = new List<EdgeViewModel>();

        public RemoveNodeCommand(ObservableCollection<NodeViewModel> _nodes, ObservableCollection<EdgeViewModel> _edges, NodeViewModel _nodeToRemove)
        {
            nodes = _nodes;
            edges = _edges;
            removeNode = _nodeToRemove;

            /* Find all edges who are connected to the node-to-be-removed */
            foreach (EdgeViewModel e in edges)
                if (e.VMEndA.Equals(removeNode))
                    removeEdges.Add(e);
                else if (e.VMEndB.Equals(removeNode))
                    removeEdges.Add(e);
        }

        public void Execute()
        {
            foreach (EdgeViewModel e in removeEdges)
                edges.Remove(e);
            nodes.Remove(removeNode);
        }



        public void UnExecute()
        {
            nodes.Add(removeNode);
            foreach (EdgeViewModel e in removeEdges)
            {
                if (e.EndA == null) e.VMEndA = removeNode;
                if (e.EndB == null) e.VMEndB = removeNode;
                edges.Add(e);
            }
        }
    }
}
