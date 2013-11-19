using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02350Project.ViewModel;
using System.Windows;

namespace _02350Project.Command
{
    class MoveNodeCommand : IUndoRedoCommand
    {
    
        private List<NodeViewModel> nodes;
        private Point offset;

        public MoveNodeCommand(List<NodeViewModel> _nodes, Point _offset)
        {
            nodes = new List<NodeViewModel>(_nodes);
            offset = _offset;
        }

        public void Execute()
        {
            for (int i = 0; i < nodes.Count; ++i )
                {
                    nodes[i].X += offset.X;
                    nodes[i].Y += offset.Y;
                }

        }

        public void UnExecute()
        {
            for (int i = 0; i < nodes.Count; ++i)
            {
                nodes[i].X -= offset.X;
                nodes[i].Y -= offset.Y;
            }
        }
    }
}
