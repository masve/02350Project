using _02350Project.Command;
using _02350Project.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;

// Test 1 + 2 = 3

namespace _02350Project.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Node> Nodes { get; set; }
        public ObservableCollection<Edge> Edges { get; set; }

        private Point moveNodePoint;

        private bool isAddingEdge = false;
        private Node firstSelectedEdgeEnd;

        public ICommand AddNodeCommand { get; private set; }
        public ICommand AddEdgeCommand { get; private set; }

        public ICommand MouseDownNodeCommand { get; private set; }
        public ICommand MouseUpNodeCommand { get; private set; }
        public ICommand MouseMoveNodeCommand { get; private set; }

        public MainViewModel()
        {
            Nodes = new ObservableCollection<Node>()
            {
                 new Node() {X = 30, Y = 30, Width = 90, Height = 120},
                 new Node() {X = 250, Y = 250, Width = 90, Height = 120}
            };

            Edges = new ObservableCollection<Edge>()
            {
                new Edge() { EndA = Nodes.ElementAt(0), EndB = Nodes.ElementAt(1) }
            };

            AddNodeCommand = new RelayCommand(AddNode);
            AddEdgeCommand = new RelayCommand(AddEdge);

            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);

        }

        public void AddNode()
        {
            AddNodeCommand m = new AddNodeCommand(Nodes);
            m.Execute();
        }

        public void AddEdge()
        {
            isAddingEdge = true;
        }

        public void MouseDownNode(MouseButtonEventArgs e)
        {
            if (!isAddingEdge)
                e.MouseDevice.Target.CaptureMouse();
        }

        public void MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null && !isAddingEdge)
            {
                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                Node movingNode = (Node)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);

                Point mousePosition = Mouse.GetPosition(canvas);
                if (moveNodePoint == default(Point))
                    moveNodePoint = mousePosition;

                movingNode.X = (int)mousePosition.X;
                movingNode.Y = (int)mousePosition.Y;
            }
        }

        public void MouseUpNode(MouseButtonEventArgs e)
        {
            if (isAddingEdge)
            {
                FrameworkElement rectEnd = (FrameworkElement)e.MouseDevice.Target;
                Node rectNode = (Node)rectEnd.DataContext;

                if (firstSelectedEdgeEnd == null)
                {
                    firstSelectedEdgeEnd = rectNode;
                }
                else if (firstSelectedEdgeEnd != rectNode)
                {
                    AddEdgeCommand m = new AddEdgeCommand(Edges, firstSelectedEdgeEnd, rectNode);
                    m.Execute();
                    isAddingEdge = false;
                    firstSelectedEdgeEnd = null;
                }
            }
            else
            {
                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                Node movingNode = (Node)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);
                Point mousePosition = Mouse.GetPosition(canvas);

                MoveNodeCommand m = new MoveNodeCommand(movingNode, (int)mousePosition.X, (int)mousePosition.Y, (int)moveNodePoint.X, (int)moveNodePoint.Y);
                m.Execute();

                moveNodePoint = new Point();
                e.MouseDevice.Target.ReleaseMouseCapture();
            }
        }

        // Finds parent of element
        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            if (parent.GetType().IsAssignableFrom(typeof(T)))
                return parent;
            else
                return FindParentOfType<T>(parent);
        }
    }
}