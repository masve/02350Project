using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using _02350Project.ViewModel;

namespace _02350Project.Command
{
    class AddEdgeCommand : IUndoRedoCommand
    {
        private EdgeViewModel edge;
        private ObservableCollection<EdgeViewModel> edges;

        public AddEdgeCommand(ObservableCollection<EdgeViewModel> _edges, NodeViewModel _endA, NodeViewModel _endB)
        {
            edges = _edges;
            edge = _endB.newEdge(_endA);
        }

        public void Execute()
        {
            edges.Add(edge);
        }

        public void UnExecute()
        {
            edges.Remove(edge);
        }
    }
}
