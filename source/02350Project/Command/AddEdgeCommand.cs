using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using _02350Project.Model;
using System.Collections.ObjectModel;

namespace _02350Project.Command
{ //git test
    class AddEdgeCommand
    {
        private Edge edge;
        private ObservableCollection<Edge> edges;

        public AddEdgeCommand(ObservableCollection<Edge> _edges, Node _endA, Node _endB)
        {
            edges = _edges;
            edge = new Edge() { EndA = _endA, EndB = _endB };
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
