using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02350Project.Model;

namespace _02350Project.Command
{
    class MoveNodeCommand
    {
        private Node node;
        private int x;
        private int y;
        private int newX;
        private int newY;

        public MoveNodeCommand(Node _node, int _newX, int _newY, int _x, int _y)
        {
            node = _node;
            x = _x;
            y = _y;
            newX = _newX;
            newY = _newY;
        }

        public void Execute()
        {
            node.CanvasCenterX = newX;
            node.CanvasCenterY = newY;
        }

        public void UnExecute()
        {
            node.CanvasCenterX = x;
            node.CanvasCenterY = y;
        }

    }
}
