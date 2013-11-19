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
    class AddNodeCommand : IUndoRedoCommand
    {
        // Use this one!
        // test

        private ObservableCollection<NodeViewModel> nodes;
        private NodeViewModel node;
        private NodeViewModel createNode;

        public AddNodeCommand(ObservableCollection<NodeViewModel> _nodes, NodeViewModel _createNode)
        {
            nodes = _nodes;
            createNode = _createNode;
        }

        public void Execute()
        {
            nodes.Add(node = createNode);
        }

        public void UnExecute()
        {
            nodes.Remove(node);
        }
    }
}
