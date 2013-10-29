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
        //private List<string> attributes;
        //private string name;

        public AddNodeCommand(ObservableCollection<NodeViewModel> _nodes, /*string _name, List<string> _attributes*/ NodeViewModel _createNode)
        {
            nodes = _nodes;
            createNode = _createNode;

            /*name = _name;
            attributes = _attributes;*/
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
