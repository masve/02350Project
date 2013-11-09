using _02350Project.Model;
using _02350Project.ViewModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Not yet implemented.
namespace _02350Project.Command
{
    class RemoveEdgeCommand : IUndoRedoCommand
    {
        private ObservableCollection<EdgeViewModel> edges;
        private EdgeViewModel edge;

        public RemoveEdgeCommand(ObservableCollection<EdgeViewModel> _edges, EdgeViewModel _edge)
        {
            edges = _edges;
            edge = _edge;
        }

        public void Execute()
        {

            edge.IsSelected = false;
            edges.Remove(edge);           
            
        }

        public void UnExecute()
        {
            edges.Add(edge);
        }

    }
}
