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

// Test F# er godt

namespace _02350Project.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Node> Nodes { get; set; }
        public ObservableCollection<Edge> Edges { get; set; }

        private Point moveNodePoint;
        private int posX;
        private int posY;

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
                //     new Node() {X = 30, Y = 30, Width = 90, Height = 120},
                //     new Node() {X = 250, Y = 250, Width = 90, Height = 120}
            };

            Edges = new ObservableCollection<Edge>()
            {
                //  new Edge() { EndA = Nodes.ElementAt(0), EndB = Nodes.ElementAt(1) }
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

                //movingNode.CanvasCenterX = ((int)mousePosition.X > 0 ? (int)mousePosition.X : 0);
                //movingNode.CanvasCenterY = ((int)mousePosition.Y > 0 ? (int)mousePosition.Y : 0);
                if (mousePosition.X - (movingNode.Height / 2) > 0)
                {
                    movingNode.CanvasCenterX = (int)mousePosition.X;
                }
                else
                {
                    movingNode.CanvasCenterX = movingNode.Width / 2;
                }
                if (mousePosition.Y - (movingNode.Width / 2) > 0)
                {
                    movingNode.CanvasCenterY = (int)mousePosition.Y;
                }
                else
                {
                    movingNode.CanvasCenterY = movingNode.Height / 2;
                }
                posX = movingNode.CanvasCenterX;
                posY = movingNode.CanvasCenterY;
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

                MoveNodeCommand m = new MoveNodeCommand(movingNode, posX, posY, (int)moveNodePoint.X, (int)moveNodePoint.Y);
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