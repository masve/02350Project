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
using System;
using System.Runtime.InteropServices;

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
        private bool isRemovingNode = false;
        private Node firstSelectedEdgeEnd;

        public ICommand AddNodeCommand { get; private set; }
        public ICommand AddEdgeCommand { get; private set; }

        public ICommand RemoveNodeCommand { get; private set; }
        public ICommand RemoveEdgeCommand { get; private set; }

        public ICommand MouseDownNodeCommand { get; private set; }
        public ICommand MouseUpNodeCommand { get; private set; }
        public ICommand MouseMoveNodeCommand { get; private set; }

        public ICommand OpenCreateDialogCommand { get; private set; }

        public enum ANCHOR { NORTH, SOUTH, EAST, WEST };
        private double northEast = -1.0 * Math.PI / 4.0;
        private double northWest = -3.0 * Math.PI / 4.0;
        private double southEast = Math.PI / 4.0;
        private double southWest = 3.0 * Math.PI / 4.0;

        public MainViewModel()
        {
            List<string> Attributes = new List<string>();
            Attributes.Add("Hello");
            Attributes.Add("HelloHelloHelloHelloHelloHello");
            Attributes.Add("Hello");
            Nodes = new ObservableCollection<Node>()
            {
                     new Node() {X = 30, Y = 30, Width = 90, Height = 120, Attributes = Attributes},
                     new Node() {X = 250, Y = 250, Width = 90, Height = 120}
            };

            Edges = new ObservableCollection<Edge>()
            {
                //  new Edge() { EndA = Nodes.ElementAt(0), EndB = Nodes.ElementAt(1) }
            };

            //AddNodeCommand = new RelayCommand(AddNode);
            AddEdgeCommand = new RelayCommand(AddEdge);

            RemoveNodeCommand = new RelayCommand(RemoveNode);

            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);

            OpenCreateDialogCommand = new RelayCommand(OpenCreateClassDialog);

            /*
             * We use messages throughout the application to communicate between viewmodels.
             * And between viewmodels and views (for requesting open and close actions).
             * By doing this we can achieve a low coupling between the different parts of 
             * the application.
             * A brief introduction to messages:
             * http://www.spikie.be/blog/post/2013/04/12/10-things-you-might-have-missed-about-MVVM-Light.aspx
             * 
             */

            /*
             * We register for messages regarding creation of nodes from the CreateClassViewModel.
             * 
             * NOTE: Find better alternative to string keys. Suggestion: Some centralized place
             */ 
            MessengerInstance.Register<Node>(this, "key1", (n) => AddNode(n));
            
        }

        public void AddNode(/*string name, List<string> attributes*/ Node node)
        {
            //writetoconsole("start");
            AddNodeCommand m = new AddNodeCommand(Nodes, node);
            m.Execute();
            //writetoconsole("end");
        }

        public void AddEdge()
        {
            isRemovingNode = false;
            isAddingEdge = true;
            //Mouse.OverrideCursor = Cursors.Hand;
        }

        public void RemoveNode()
        {
            isAddingEdge = false;
            isRemovingNode = true;
        }

        public void MouseDownNode(MouseButtonEventArgs e)
        {
            if (!isAddingEdge && !isRemovingNode)
                e.MouseDevice.Target.CaptureMouse();
        }

        public void MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null && !isAddingEdge && !isRemovingNode)
            {
                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                Node movingNode = (Node)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);

                Point mousePosition = Mouse.GetPosition(canvas);
                if (moveNodePoint == default(Point))
                    moveNodePoint = mousePosition;

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

                CalculateAnchor(movingNode);

                posX = movingNode.CanvasCenterX;
                posY = movingNode.CanvasCenterY;
            }
        }

        #region Dynamic Anchorpoint Calculations
        public void CalculateAnchor(Node node)
        {
            foreach (Edge e in Edges)
            {
                if (e.EndA.Equals(node))
                {
                    setEnds(e);

                }
                else if (e.EndB.Equals(node))
                {
                    setEnds(e);
                }
            }
        }

        public void setEnds(Edge e)
        {
            ANCHOR A = findAnchor(e.EndA.CanvasCenterX, e.EndA.CanvasCenterY, e.EndB.CanvasCenterX, e.EndB.CanvasCenterY);
            ANCHOR B = findAnchor(e.EndB.CanvasCenterX, e.EndB.CanvasCenterY, e.EndA.CanvasCenterX, e.EndA.CanvasCenterY);
            switch (A)
            {
                case ANCHOR.NORTH:
                    e.AX = (int)e.EndA.North.X;
                    e.AY = (int)e.EndA.North.Y;
                    break;
                case ANCHOR.EAST:
                    e.AX = (int)e.EndA.East.X;
                    e.AY = (int)e.EndA.East.Y;

                    break;
                case ANCHOR.SOUTH:
                    e.AX = (int)e.EndA.South.X;
                    e.AY = (int)e.EndA.South.Y;
                    break;
                case ANCHOR.WEST:
                    e.AX = (int)e.EndA.West.X;
                    e.AY = (int)e.EndA.West.Y;
                    break;
            }
            switch (B)
            {
                case ANCHOR.NORTH:
                    e.BX = (int)e.EndB.North.X;
                    e.BY = (int)e.EndB.North.Y;
                    break;
                case ANCHOR.EAST:
                    e.BX = (int)e.EndB.East.X;
                    e.BY = (int)e.EndB.East.Y;

                    break;
                case ANCHOR.SOUTH:
                    e.BX = (int)e.EndB.South.X;
                    e.BY = (int)e.EndB.South.Y;

                    break;
                case ANCHOR.WEST:
                    e.BX = (int)e.EndB.West.X;
                    e.BY = (int)e.EndB.West.Y;
                    break;
            }
        }

        private ANCHOR findAnchor(int x1, int y1, int x2, int y2)
        {
            int deltaX = x2 - x1;
            int deltaY = y2 - y1;


            double angle = Math.Atan2(deltaY, deltaX);


            if (northEast > angle && angle >= northWest)
            {
                return ANCHOR.NORTH;

            }
            else if (southWest > angle && angle >= southEast)
            {
                return ANCHOR.SOUTH;

            }
            else if (southEast > angle && angle >= northEast)
            {
                return ANCHOR.EAST;

            }
            else if (northWest > angle || angle >= southWest)
            {
                return ANCHOR.WEST;

            }
            return ANCHOR.NORTH;
        }
        
        #endregion

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
                    CalculateAnchor(rectNode);
                    isAddingEdge = false;
                    //Mouse.OverrideCursor = Cursors.Arrow;
                    firstSelectedEdgeEnd = null;
                }
            }
            else if (isRemovingNode)
            {
                FrameworkElement rectNode = (FrameworkElement)e.MouseDevice.Target;
                Node NodeToRemove = (Node)rectNode.DataContext;
                RemoveNodeCommand n = new RemoveNodeCommand(Nodes, Edges, NodeToRemove);
                n.Execute();
                isRemovingNode = false;
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

        /*
         * Handling open and close for views and view models http://stackoverflow.com/questions/18435173/open-close-view-from-viewmodel 
         */

        /*
         * Opens the Create Class Dialog
         */
        public void OpenCreateClassDialog()
        {
            MessengerInstance.Send<int>(1001, "key6");
        }

        /*
         * Finds parent of element
         */
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