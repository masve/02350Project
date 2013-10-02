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
    class AddNodeCommand
    {
        // Use this one!
        // test

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
