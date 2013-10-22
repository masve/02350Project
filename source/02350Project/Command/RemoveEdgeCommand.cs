using _02350Project.Model;
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
        /*       private ObservableCollection<Edge> edges;
               private Node node;

               public RemoveEdgeCommand(ObservableCollection<Edge> _edges, Node _node)
               {
                   edges = _edges;
                   node = _node;
               }

               public void Execute()
               {
                   foreach (Edge e in edges)
                   {
                       if (e.EndA.Equals(node))
                           edges.Remove(e);
                       else if (e.EndB.Equals(node))
                           edges.Remove(e);
                   }
            
               }*/
        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void UnExecute()
        {
            throw new NotImplementedException();
        }
    }
}
