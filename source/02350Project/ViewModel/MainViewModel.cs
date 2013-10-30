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

        private string edgeType;

        private PointCollection points = new PointCollection();

        private bool isAddingEdge = false;
        private bool isRemovingNode = false;
        private bool isMovingNode = false;
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

        #region Edge type commands
        public ICommand AddAGGCommand { get; private set; }
        public ICommand AddDEPCommand { get; private set; }
        public ICommand AddCOMCommand { get; private set; }
        public ICommand AddASSCommand { get; private set; }
        public ICommand AddGENCommand { get; private set; }
        #endregion

        public ICommand TestCommand { get; private set; }

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
            //AddEdgeCommand = new RelayCommand(AddEdge);

            RemoveNodeCommand = new RelayCommand(RemoveNode);

            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);

            OpenCreateDialogCommand = new RelayCommand(OpenCreateClassDialog);
            ExpandResizeCommand = new RelayCommand<SizeChangedEventArgs>(ExpandResize);

            UndoCommand = new RelayCommand(undo, canUndo);
            RedoCommand = new RelayCommand(redo, canRedo);

            AddAGGCommand = new RelayCommand(AddAgg);
            AddASSCommand = new RelayCommand(AddAss);
            AddDEPCommand = new RelayCommand(AddDep);
            AddCOMCommand = new RelayCommand(AddCom);
            AddGENCommand = new RelayCommand(AddGen);

            TestCommand = new RelayCommand(test);

            #region About Messaging
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
            #endregion

        }

        public void test()
        {
            //Point p = new Point(2.0, 3.0);
            //PointCollection pc = new PointCollection();
            //pc.Add(new Point(4.0, 1.0));
            //pc.Add(new Point(4.0, 5.0));
            //double angle = 90.0 * (Math.PI / 180.0);
            //Other.ConsolePrinter.Write(rotatePoint(p, pc, angle).ToString());
        }



        #region Undo/Redo Command Implementation
        public void undo()
        {
            IUndoRedoCommand command = undoRedoController.getHeadOfUndo();

            undoRedoController.Undo();
            if (command is MoveNodeCommand)
            {
                CalculateAnchor(((MoveNodeCommand)command).Node);
            }

        }
        public bool canUndo()
        {
            return undoRedoController.CanUndo();
        }

        public void redo()
        {
            IUndoRedoCommand command = undoRedoController.getHeadOfRedo();

            undoRedoController.Redo();
            if (command is MoveNodeCommand)
            {
                CalculateAnchor(((MoveNodeCommand)command).Node);
            }
        }

        public bool canRedo()
        {
            return undoRedoController.CanRedo();
        }
        #endregion


        public void AddNode(NodeViewModel node)
        {
            undoRedoController.AddAndExecute(new AddNodeCommand(Nodes, node));
        }

        #region Add edge
        public void AddEdge()
        {
            isRemovingNode = false;
            isAddingEdge = true;
        }
        public void AddGen()
        {
            AddEdge();
            edgeType = "GEN";
        }
        public void AddAss()
        {
            AddEdge();
            edgeType = "ASS";
        }
        public void AddAgg()
        {
            AddEdge();
            edgeType = "AGG";
        }
        public void AddDep()
        {
            AddEdge();
            edgeType = "DEP";
        }
        public void AddCom()
        {
            AddEdge();
            edgeType = "COM";
        }
        #endregion

        public void RemoveNode()
        {
            isAddingEdge = false;
            isRemovingNode = true;
        }

        public void ExpandResize(SizeChangedEventArgs e)
        {
            FrameworkElement rect = (FrameworkElement)e.Source;
            NodeViewModel node = (NodeViewModel)rect.DataContext;

            node.Resize(e.NewSize.Height, e.NewSize.Width);
            CalculateAnchor(node);
        }

        #region Mouse UP DOWN MOVE

        public void MouseDownNode(MouseButtonEventArgs e)
        {
            isMovingNode = false;
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
                isMovingNode = true;
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
                    AddEdgeCommand m = new AddEdgeCommand(Edges, firstSelectedEdgeEnd, rectNode, edgeType);
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
            else if (isMovingNode)
            {
                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel movingNode = (NodeViewModel)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);
                Point mousePosition = Mouse.GetPosition(canvas);


                MoveNodeCommand m = new MoveNodeCommand(movingNode, posX, posY, (int)moveNodePoint.X, (int)moveNodePoint.Y);
                undoRedoController.AddAndExecute(m);

                moveNodePoint = new Point();

                isMovingNode = false;
            }

            if (Mouse.Captured != null)
                e.MouseDevice.Target.ReleaseMouseCapture();
        }
        #endregion

        public void CalculateAnchor(NodeViewModel nodeVM)
        {
            foreach (EdgeViewModel e in Edges)
            {
                if (e.VMEndA.Equals(nodeVM))
                {
                    nodeVM.setEnds(e);
                    e.ArrowControl();

                }
                else if (e.VMEndB.Equals(nodeVM))
                {
                    nodeVM.setEnds(e);
                    e.ArrowControl();
                }
            }
        }

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