using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02350Project.ViewModel;
using System.Windows;

namespace _02350Project.Command
{
    class MoveNodeCommand1 : IUndoRedoCommand
    {

        private List<NodeViewModel> nodes;
        private List<Point> oldPoints;
        private List<Point> newPoints;

        private Vector offset;
        private double y;
        private double newX;
        private double newY;

        public MoveNodeCommand1(List<NodeViewModel> _nodes, List<Point> _oldPoints, List<Point> _newPoints)
        {
            nodes = new List<NodeViewModel>(_nodes);
            newPoints = new List<Point>(_newPoints);
            oldPoints = new List<Point>(_oldPoints);
        }

        public void Execute()
        {
            for (int i = 0; i < nodes.Count; ++i)
            {
                nodes[i].X = newPoints[i].X;
                nodes[i].Y = newPoints[i].Y;
            }

        }

        public void UnExecute()
        {
            for (int i = 0; i < nodes.Count; ++i)
            {
                nodes[i].X = oldPoints[i].X;
                nodes[i].Y = oldPoints[i].Y;
            }
        }

        //public NodeViewModel Node { get { return node; } }

    }
}
