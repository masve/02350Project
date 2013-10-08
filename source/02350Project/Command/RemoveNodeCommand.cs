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
        private Node nodeToRemove;

        public RemoveNodeCommand(ObservableCollection<Node> _nodes, Node _nodeToRemove)
        {
            nodes = _nodes;
            nodeToRemove = _nodeToRemove;
        }

        public void Execute()
        {
            nodes.Remove(nodeToRemove);
        }
    }
}
