using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using _02350Project.Model;
using _02350Project.ViewModel;

namespace _02350Project.Command
{
    class EditNodeCommand : IUndoRedoCommand
    {
        private readonly NodeViewModel _node;
        private readonly ObservableCollection<NodeViewModel> _nodes;
        private readonly string _name;
        private readonly NodeType _type;
        private readonly List<string> _attributes;
        private readonly List<string> _methods;

        public EditNodeCommand(NodeViewModel node, ObservableCollection<NodeViewModel> nodes, string name, NodeType type, List<string> attributes, List<string> methods)
        {
            _node = node;
            _nodes = nodes;
            _name = name;
            _type = type;
            _attributes = new List<string>(attributes);
            _methods = new List<string>(methods);
        }

        public void Execute()
        {
            _node.Name = _name;
            _node.NodeType = _type;
            _node.Attributes = _attributes;
            _node.Methods = _methods;
        }

        public void UnExecute()
        {
            throw new NotImplementedException();
        }
    }
}
