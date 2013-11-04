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
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        public ObservableCollection<NodeViewModel> Nodes { get; set; }
        public ObservableCollection<EdgeViewModel> Edges { get; set; }

        //private Point moveNodePoint; // No longer in use
        private double posX;
        private double posY;

        private string edgeType;

        private PointCollection points = new PointCollection();

        private bool isAddingEdge = false;
        private bool isRemovingNode = false;
        private bool isMovingNode = false;
        private NodeViewModel firstSelectedEdgeEnd;

        //public ICommand AddNodeCommand { get; private set; }
        //public ICommand AddEdgeCommand { get; private set; }

        public ICommand RemoveNodeCommand { get; private set; }
        public ICommand RemoveEdgeCommand { get; private set; }

        public ICommand MouseDownNodeCommand { get; private set; }
        public ICommand MouseUpNodeCommand { get; private set; }
        public ICommand MouseMoveNodeCommand { get; private set; }

        public ICommand OpenCreateDialogCommand { get; private set; }

        public ICommand ExpandResizeCommand { get; private set; }
        public ICommand CancelActionCommand { get; private set; }

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



        // public ICommand TestCommand { get; private set; }

        #region Constructor
        public MainViewModel()
        {
            Nodes = new ObservableCollection<NodeViewModel>()
            {
            };

            AddNode(new NodeViewModel());

            Edges = new ObservableCollection<EdgeViewModel>()
            {
            };

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

            CancelActionCommand = new RelayCommand(CancelAction);

            //TestCommand = new RelayCommand(test);

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
        #endregion

        private void CancelAction()
        {
            foreach (NodeViewModel vm in Nodes)
            {
                vm.IsSelected = false;
            }
            isAddingEdge = false;
            isRemovingNode = false;
            isMovingNode = false;
            firstSelectedEdgeEnd = null;
        }

        /// <summary>
        /// A dummy command implemantation which allows us to debug and test methods on button press.
        /// </summary>
        public void test()
        {
        }

        /// <summary>
        /// Given a NodeViewModel it calls a methods in undoRedoController to add it to the ObservableCollection managing the NodeViewModels.
        /// Effectively adding a NodeUserControl to the canvas.
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(NodeViewModel node)
        {
            undoRedoController.AddAndExecute(new AddNodeCommand(Nodes, node));
        }

        /// <summary>
        /// Sets bools used in MouseUpNode to determine if the clicked node should be removed.
        /// </summary>
        public void RemoveNode()
        {
            isAddingEdge = false;
            isRemovingNode = true;
            NodeViewModel nodeToRemove = null;
            foreach (NodeViewModel vm in Nodes)
            {
                if (vm.IsSelected)
                {
                    nodeToRemove = vm;
        }

            }
            if (nodeToRemove != null)
            {
                RemoveNodeCommand m = new RemoveNodeCommand(Nodes, Edges, nodeToRemove);
                undoRedoController.AddAndExecute(m);
            }
            isRemovingNode = false;
        }

        //public bool CanRemove()
        //{
        //    foreach (NodeViewModel vm in Nodes)
        //        if (vm.IsSelected)
        //            return true;
        //    return false;
        //}

        /// <summary>
        /// Catches an SizeChangedEvent. Used to get the height and width of a NodeUserControl.
        /// </summary>
        /// <param name="e"></param>
        public void ExpandResize(SizeChangedEventArgs e)
        {
            FrameworkElement rect = (FrameworkElement)e.Source;
            NodeViewModel node = (NodeViewModel)rect.DataContext;

            node.Height = e.NewSize.Height;
            node.Width = e.NewSize.Width;
            CalculateAnchor(node);
        }

        #region Edge Adding
        /// <summary>
        /// Sets bools used in MouseUpNode to determine if the clicked node should have an edge added.
        /// </summary>
        public void AddEdge()
        {
            isRemovingNode = false;
            isAddingEdge = true;
        }

        /// <summary>
        /// Invoked by a command, through a binding, corresponding to the edge type (Generalization) to be added.
        /// </summary>
        public void AddGen() { AddEdge(); edgeType = "GEN"; }

        /// <summary>
        /// Invoked by a command, through a binding, corresponding to the edge type (Association) to be added.
        /// </summary>
        public void AddAss() { AddEdge(); edgeType = "ASS"; }

        /// <summary>
        /// Invoked by a command, through a binding, corresponding to the edge type (Aggregation) to be added.
        /// </summary>
        public void AddAgg() { AddEdge(); edgeType = "AGG"; }

        /// <summary>
        /// Invoked by a command, through a binding, corresponding to the edge type (Dependency) to be added.
        /// </summary>
        public void AddDep() { AddEdge(); edgeType = "DEP"; }

        /// <summary>
        /// Invoked by a command, through a binding, corresponding to the edge type (Composition) to be added.
        /// </summary>
        public void AddCom() { AddEdge(); edgeType = "COM"; }
        #endregion

        #region Mouse UP DOWN MOVE
        /// <summary>
        /// MouseDownNode handles the implementation used when a MouseDown is triggered through an EventToCommand.
        /// </summary>
        /// <param name="e"></param>


        Point offsetPosition;
        private double oldPosX;
        private double oldPosY;
        public void MouseDownNode(MouseButtonEventArgs e)
        {
            isMovingNode = false;
            if (!isAddingEdge && !isRemovingNode)
            {
                e.MouseDevice.Target.CaptureMouse();

                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel movingNode = (NodeViewModel)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);

                offsetPosition = Mouse.GetPosition(canvas);
                oldPosX = movingNode.X;
                oldPosY = movingNode.Y;

            }
        }

        /// <summary>
        /// MouseMoveNode handles the implementation used when a MouseMove is triggered through an EventToCommand.
        /// </summary>
        /// <param name="e"></param>
        public void MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null && !isAddingEdge && !isRemovingNode)
            {
                isMovingNode = true;
                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel movingNode = (NodeViewModel)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);

                Point mousePosition = Mouse.GetPosition(canvas);

                mousePosition.X -= offsetPosition.X;
                mousePosition.Y -= offsetPosition.Y;

                movingNode.X = oldPosX + mousePosition.X;
                movingNode.Y = oldPosY + mousePosition.Y;

                posX = movingNode.X = movingNode.X >= 0 ? movingNode.X : 0;
                posY = movingNode.Y = movingNode.Y >= 0 ? movingNode.Y : 0;

                CalculateAnchor(movingNode);

            }
        }

        /// <summary>
        /// MouseUpNode handles the implementation used when a MouseUp is triggered through an EventToCommand.
        /// </summary>
        /// <param name="e"></param>
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
                //FrameworkElement rectNode = (FrameworkElement)e.MouseDevice.Target;
                //NodeViewModel NodeToRemove = (NodeViewModel)rectNode.DataContext;
                //RemoveNodeCommand m = new RemoveNodeCommand(Nodes, Edges, NodeToRemove);
                //undoRedoController.AddAndExecute(m);
                //isRemovingNode = false;
            }
            else if (isMovingNode)
            {
                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel movingNode = (NodeViewModel)movingRect.DataContext;
                //Canvas canvas = FindParentOfType<Canvas>(movingRect);
                //Point mousePosition = Mouse.GetPosition(canvas);

                MoveNodeCommand m = new MoveNodeCommand(movingNode, posX, posY, oldPosX, oldPosY);
                undoRedoController.AddAndExecute(m);

                //moveNodePoint = new Point();

                isMovingNode = false;
            }
            else
            {
                FrameworkElement rect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel rectNode = (NodeViewModel)rect.DataContext;

                foreach (NodeViewModel vm in Nodes)
                    vm.IsSelected = false;
                rectNode.IsSelected = true;
            }

            if (Mouse.Captured != null)
                e.MouseDevice.Target.ReleaseMouseCapture();
        }
        #endregion

        /// <summary>
        /// Calculates anchor points for a given node. (The points on the node that edges snap)
        /// </summary>
        /// <param name="nodeVM"></param>
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

        /// <summary>
        /// Creates a CreateNodeViewModel, opens a CreateNodeWindow (dialog) and sets the datacontext.
        /// If the dialog returns true a the NodeViewModel given to the CreateNodeViewModel should be added.
        /// </summary>
        public void OpenCreateClassDialog()
        {
            NodeViewModel newNode = new NodeViewModel();

            var dialog = new CreateNodeWindow();
            //newnode gives med som reference, derfor kan vi bare redigere den direkte i vores dialog
            CreateNodeViewModel dialogViewModel = new CreateNodeViewModel(newNode, dialog);
            dialog.DataContext = dialogViewModel;
            if (dialog.ShowDialog() == true)
                AddNode(newNode);
        }

        #region Undo/Redo Command Implementation
        /*
         * Instead of calling:
         * UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
         * RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);
         * we do:
         * UndoCommand = new RelayCommand(undo, canUndo);
         * RedoCommand = new RelayCommand(redo, canRedo);
         * which allows us to call CalculateAnchor right after a Node has been added to canvas.
         */
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

        /// <summary>
        /// Finds parent of an element recursively.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
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