using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02350Project.ViewModel;

namespace _02350Project.Command
{
    class MoveNodeCommand : IUndoRedoCommand
    {
        private NodeViewModel node;
        private double x;
        private double y;
        private double newX;
        private double newY;

        public MoveNodeCommand(NodeViewModel _node, double _newX, double _newY, double _x, double _y)
        {
            node = _node;
            x = _x;
            y = _y;
            newX = _newX;
            newY = _newY;
        }

        public void Execute()
        {
            node.X = newX;
            node.Y = newY;
        }

        public void UnExecute()
        {
            node.X = x;
            node.Y = y;
        }

        public NodeViewModel Node { get { return node; } }

    }
}
