using _02350Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace _02350Project.Commands
{
    class AddNodeCommand
    {
        private ObservableCollection<Node> nodes;
        private Node node;

        public AddNodeCommand(ObservableCollection<Node> _nodes)
        {
            nodes = _nodes;
        }

        public void Execute()
        {
            nodes.Add(node = new Node());
        }

        public void UnExecute()
        {
            nodes.Remove(node);
        }
    }
}
