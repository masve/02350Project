using _02350Project.Command;
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
using _02350Project.View;

// Test F# er godt

namespace _02350Project.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Child ViewModels
        // private CreateNodeViewModel createClassViewModel;
        #endregion

        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        public ObservableCollection<NodeViewModel> Nodes { get; set; }
        public ObservableCollection<EdgeViewModel> Edges { get; set; }

        private Point moveNodePoint;
        private double posX;
        private double posY;

        private PointCollection points = new PointCollection();

        private bool isAddingEdge = false;
        private bool isRemovingNode = false;
        private NodeViewModel firstSelectedEdgeEnd;

        public ICommand AddNodeCommand { get; private set; }
        public ICommand AddEdgeCommand { get; private set; }

        public ICommand RemoveNodeCommand { get; private set; }
        public ICommand RemoveEdgeCommand { get; private set; }

        public ICommand MouseDownNodeCommand { get; private set; }
        public ICommand MouseUpNodeCommand { get; private set; }
        public ICommand MouseMoveNodeCommand { get; private set; }

        public ICommand OpenCreateDialogCommand { get; private set; }

        public ICommand ExpandResizeCommand { get; private set; }

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public ICommand UndoRedoCheckCommand { get; private set; }

        public ICommand TestCommand { get; private set; }


        public enum ANCHOR { NORTH, SOUTH, EAST, WEST };
        private double northEast = -1.0 * Math.PI / 4.0;
        private double northWest = -3.0 * Math.PI / 4.0;
        private double southEast = Math.PI / 4.0;
        private double southWest = 3.0 * Math.PI / 4.0;

        public MainViewModel()
        {
            //createClassViewModel = new CreateClassViewModel();

            #region Dummy Node
            List<string> Attributes = new List<string>();
            List<string> Methods = new List<string>();
            Attributes.Add("- a : int");
            Attributes.Add("- b : int");
            Attributes.Add("- sum : int");
            Methods.Add("+ add ( val1 : int, val2 : int )");
            Methods.Add("+ sub ( val1 : int, val2 : int )");
            Methods.Add("+ mul ( val1 : int, val2 : int )");
            Methods.Add("+ div ( val1 : int, val2 : int )");
            #endregion

            //Nodev testNode = new Node() { X = 30, Y = 30, Width = 170, Height = 200, Attributes = Attributes, Methods = Methods, Name = "Calculator" };

            Nodes = new ObservableCollection<NodeViewModel>()
            {
                //testNode,
                //new Node() {X = 250, Y = 250, Width = 90, Height = 120}
            };

            AddNode(new NodeViewModel());

            Edges = new ObservableCollection<EdgeViewModel>()
            {
                //new Edge() { EndA = Nodes.ElementAt(0), EndB = Nodes.ElementAt(1) }
            };

            //AddNodeCommand = new RelayCommand(AddNode);
            AddEdgeCommand = new RelayCommand(AddEdge);

            RemoveNodeCommand = new RelayCommand(RemoveNode);

            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);

            OpenCreateDialogCommand = new RelayCommand(OpenCreateClassDialog);
            ExpandResizeCommand = new RelayCommand<SizeChangedEventArgs>(ExpandResize);

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            TestCommand = new RelayCommand(test);


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
             * NOTE: Find better alternative to string keys. Suggestion: Some enum
             */
            // MessengerInstance.Register<Node>(this, "key1", (n) => AddNode(n));

        }

        public void test()
        {
            Other.ConsolePrinter.Write("123123");

        }

        public void AddNode(NodeViewModel node)
        {
            undoRedoController.AddAndExecute(new AddNodeCommand(Nodes, node));
        }

        public void AddEdge()
        {
            isRemovingNode = false;
            isAddingEdge = true;
            Other.ConsolePrinter.Write("addedge");
        }

        public void RemoveNode()
        {
            isAddingEdge = false;
            isRemovingNode = true;
        }

        public void ExpandResize(SizeChangedEventArgs e)
        {
            FrameworkElement rect = (FrameworkElement)e.Source;
            NodeViewModel node = (NodeViewModel)rect.DataContext;

            node.Height = e.NewSize.Height;
            node.Width = e.NewSize.Width;

            CalculateAnchor(node);
        }

        #region Mouse UP DOWN MOVE

        public void MouseDownNode(MouseButtonEventArgs e)
        {
            if (!isAddingEdge && !isRemovingNode)
            {
                e.MouseDevice.Target.CaptureMouse();

                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel movingNode = (NodeViewModel)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);

                Point mousePosition = Mouse.GetPosition(canvas);
                posX = movingNode.X;
                posY = movingNode.Y;

            }

        }

        public void MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null && !isAddingEdge && !isRemovingNode)
            {
                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel movingNode = (NodeViewModel)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);

                Point mousePosition = Mouse.GetPosition(canvas);

                if (moveNodePoint == default(Point))
                    moveNodePoint = mousePosition;

                if (mousePosition.X - (movingNode.Width / 2) > 0)
                    movingNode.CanvasCenterX = mousePosition.X;
                else
                    movingNode.CanvasCenterX = movingNode.Width / 2;
                if (mousePosition.Y - (movingNode.Height / 2) > 0)
                    movingNode.CanvasCenterY = mousePosition.Y;
                else
                    movingNode.CanvasCenterY = movingNode.Height / 2;

                CalculateAnchor(movingNode);

                posX = movingNode.CanvasCenterX;
                posY = movingNode.CanvasCenterY;
            }
        }

        public void MouseUpNode(MouseButtonEventArgs e)
        {
            if (isAddingEdge)
            {
                FrameworkElement rectEnd = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel rectNode = (NodeViewModel)rectEnd.DataContext;

                if (firstSelectedEdgeEnd == null)
                {
                    firstSelectedEdgeEnd = rectNode;
                    Other.ConsolePrinter.Write("first edge");
                }
                else if (firstSelectedEdgeEnd != rectNode)
                {
                    AddEdgeCommand m = new AddEdgeCommand(Edges, firstSelectedEdgeEnd, rectNode);
                    undoRedoController.AddAndExecute(m);

                    CalculateAnchor(rectNode);
                    isAddingEdge = false;
                    firstSelectedEdgeEnd = null;

                }

            }
            else if (isRemovingNode)
            {
                FrameworkElement rectNode = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel NodeToRemove = (NodeViewModel)rectNode.DataContext;
                RemoveNodeCommand m = new RemoveNodeCommand(Nodes, Edges, NodeToRemove);
                undoRedoController.AddAndExecute(m);
                isRemovingNode = false;
            }
            else
            {
                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel movingNode = (NodeViewModel)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);
                Point mousePosition = Mouse.GetPosition(canvas);


                MoveNodeCommand m = new MoveNodeCommand(movingNode, posX, posY, (int)moveNodePoint.X, (int)moveNodePoint.Y);
                undoRedoController.AddAndExecute(m);

                moveNodePoint = new Point();
                e.MouseDevice.Target.ReleaseMouseCapture();
            }
        }
        #endregion

        #region Dynamic Anchorpoint Calculations
        public void CalculateAnchor(NodeViewModel node)
        {
            foreach (EdgeViewModel e in Edges)
            {
                if (e.VMEndA.Equals(node))
                {
                    setEnds(e);

                }
                else if (e.VMEndB.Equals(node))
                {
                    setEnds(e);
                }
            }
        }

        public void setEnds(EdgeViewModel e)
        {
            ANCHOR A = findAnchor(e.VMEndA.CanvasCenterX, e.VMEndA.CanvasCenterY, e.VMEndB.CanvasCenterX, e.VMEndB.CanvasCenterY);
            ANCHOR B = findAnchor(e.VMEndB.CanvasCenterX, e.VMEndB.CanvasCenterY, e.VMEndA.CanvasCenterX, e.VMEndA.CanvasCenterY);
            switch (A)
            {
                case ANCHOR.NORTH:
                    e.AX = e.VMEndA.North.X;
                    e.AY = e.VMEndA.North.Y;
                    break;
                case ANCHOR.EAST:
                    e.AX = e.VMEndA.East.X;
                    e.AY = e.VMEndA.East.Y;

                    break;
                case ANCHOR.SOUTH:
                    e.AX = e.VMEndA.South.X;
                    e.AY = e.VMEndA.South.Y;
                    break;
                case ANCHOR.WEST:
                    e.AX = e.VMEndA.West.X;
                    e.AY = e.VMEndA.West.Y;
                    break;
            }
            switch (B)
            {
                case ANCHOR.NORTH:
                    e.BX = e.VMEndB.North.X;
                    e.BY = e.VMEndB.North.Y;
                    break;
                case ANCHOR.EAST:
                    e.BX = e.VMEndB.East.X;
                    e.BY = e.VMEndB.East.Y;

                    break;
                case ANCHOR.SOUTH:
                    e.BX = e.VMEndB.South.X;
                    e.BY = e.VMEndB.South.Y;

                    break;
                case ANCHOR.WEST:
                    e.BX = e.VMEndB.West.X;
                    e.BY = e.VMEndB.West.Y;
                    break;
            }
        }

        private ANCHOR findAnchor(double x1, double y1, double x2, double y2)
        {
            double deltaX = x2 - x1;
            double deltaY = y2 - y1;


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

        /*
         * Handling open and close for views and view models http://stackoverflow.com/questions/18435173/open-close-view-from-viewmodel 
         */

        /*
         * Opens the Create Class Dialog
         */
        public void OpenCreateClassDialog()
        {
            // MessengerInstance.Send<int>(1001, "key6");
            NodeViewModel newNode = new NodeViewModel();

            var dialog = new CreateNodeWindow();
            //newnode gives med som reference, derfor kan vi bare redigere den direkte i vores dialog
            CreateNodeViewModel dialogViewModel = new CreateNodeViewModel(newNode, dialog);
            dialog.DataContext = dialogViewModel;
            if (dialog.ShowDialog() == true)
                AddNode(newNode);
        }

        public bool UndoRedoCheck()
        {
            return isAddingEdge;
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