using _02350Project.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _02350Project.Command
{
    class RemoveNodeCommand : IUndoRedoCommand
    {
        private readonly ObservableCollection<NodeViewModel> _nodes;
        private readonly ObservableCollection<EdgeViewModel> _edges;
        private readonly NodeViewModel _removeNode;
        readonly List<EdgeViewModel> _removeEdges = new List<EdgeViewModel>();

        public RemoveNodeCommand(ObservableCollection<NodeViewModel> nodes, ObservableCollection<EdgeViewModel> edges, NodeViewModel nodeToRemove)
        {
            _nodes = nodes;
            _edges = edges;
            _removeNode = nodeToRemove;

            /* Find all edges who are connected to the node-to-be-removed */
            foreach (EdgeViewModel e in _edges)
                if (e.VMEndA.Equals(_removeNode))
                    _removeEdges.Add(e);
                else if (e.VMEndB.Equals(_removeNode))
                    _removeEdges.Add(e);
        }

        public void Execute()
        {
            foreach (EdgeViewModel e in _removeEdges)
                _edges.Remove(e);
            _nodes.Remove(_removeNode);
        }



        public void UnExecute()
        {
            _nodes.Add(_removeNode);
            foreach (EdgeViewModel e in _removeEdges)
            {
                if (e.EndA == null) e.VMEndA = _removeNode;
                if (e.EndB == null) e.VMEndB = _removeNode;
                _edges.Add(e);
            }
        }
    }
}
