using System;
using System.Collections.Generic;
using _02350Project.Command;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using _02350Project.Model;
using _02350Project.View;
using _02350Project.Other;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Threading;

// Test F# er godt

namespace _02350Project.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly UndoRedoController _undoRedoController = UndoRedoController.GetInstance();

        public ObservableCollection<NodeViewModel> Nodes { get; set; }
        public ObservableCollection<EdgeViewModel> Edges { get; set; }

        //private Point moveNodePoint; // No longer in use
        private double _posX;
        private double _posY;

        private string _edgeType;
        private string _path;

        #region Can Bools
        private bool _canRemove;
        private bool _canCancel;
        #endregion

        private int _nodeIdCounter;

        private bool _isAddingEdge;
        private bool _isRemovingNode;
        private bool _isMovingNode;
        private NodeViewModel _firstSelectedEdgeEnd;

        public ICommand RemoveElementsCommand { get; private set; }
        public ICommand RemoveEdgeCommand { get; private set; }

        public ICommand MouseDownNodeCommand { get; private set; }
        public ICommand MouseUpNodeCommand { get; private set; }
        public ICommand MouseMoveNodeCommand { get; private set; }

        public ICommand CreateNodeCommand { get; private set; }
        public ICommand EditNodeCommand { get; private set; }

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

        #region Save / Save As / Open / New
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand NewCommand { get; private set; }
        #endregion



        public ICommand TestCommand { get; private set; }

        #region Constructor
        public MainViewModel()
        {
            _nodeIdCounter = 0;

            Nodes = new ObservableCollection<NodeViewModel>();

            Edges = new ObservableCollection<EdgeViewModel>();

            #region Test Data
            NodeViewModel testNode = new NodeViewModel(_nodeIdCounter++, new Node())
            {
                Name = "Calculator",
                NodeType = NodeType.INTERFACE,
                Attributes = new List<string> {"+ a : int", "+ b : int", "+ sum : int"},
                Methods = new List<string> { "- add ( val1 : int, val2 : int )", "- sub ( val1 : int, val2 : int )" }
            };

            AddNode(testNode);
            #endregion

            RemoveElementsCommand = new RelayCommand(RemoveElements, CanRemove);

            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);

            CreateNodeCommand = new RelayCommand(CreateNode);
            EditNodeCommand = new RelayCommand(EditNode);
            //ExpandResizeCommand = new RelayCommand<SizeChangedEventArgs>(ExpandResize);

            UndoCommand = new RelayCommand(Undo, CanUndo);
            RedoCommand = new RelayCommand(Redo, CanRedo);

            #region Line RelayCommands
            AddAGGCommand = new RelayCommand(AddAgg);
            AddASSCommand = new RelayCommand(AddAss);
            AddDEPCommand = new RelayCommand(AddDep);
            AddCOMCommand = new RelayCommand(AddCom);
            AddGENCommand = new RelayCommand(AddGen);
            #endregion

            CancelActionCommand = new RelayCommand(CancelAction, CanCancel);

            TestCommand = new RelayCommand(Test);

            #region Save / Save As / Open / New RelayCommands
            SaveCommand = new RelayCommand(Save);
            SaveAsCommand = new RelayCommand(SaveAs);
            OpenCommand = new RelayCommand(Open);
            NewCommand = new RelayCommand(New);
            #endregion

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
            foreach (EdgeViewModel vm in Edges)
            {
                vm.IsSelected = false;
            }
            _isAddingEdge = false;
            _isRemovingNode = false;
            _isMovingNode = false;
            _firstSelectedEdgeEnd = null;
            _canRemove = false;
            _canCancel = false;
        }

        

        /// <summary>
        /// A dummy command implemantation which allows us to debug and test methods on button press.
        /// </summary>
        public void Test()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
                ConsolePrinter.Write("Printing...");
        }

        public void New()
        {
            ClearDiagram();
            _path = null;            
        }

        private bool ClearDiagram()
        {
            if (Nodes.Any())
            {
                string message = "Do you want to save changes to save.xml?";
                MessageBoxButton b = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBoxResult result = MessageBox.Show(message, "placeholder", b, icon);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ConsolePrinter.Write("Yes");
                        Save();
                        Nodes.Clear();
                        Edges.Clear();
                        return true;
                    case MessageBoxResult.No:
                        ConsolePrinter.Write("No");
                        Nodes.Clear();
                        Edges.Clear();
                        return true;
                    default:
                        ConsolePrinter.Write("Cancel");
                        return false;
                }

            }
            return true;
        }


        /// <summary>
        /// Opens a Save File Dialog and calls the save function in Data with the returned path.
        /// </summary>
        public void SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "save",
                DefaultExt = ".xml",
                Filter = "Extensible Markup Language (*.xml)|*.xml|All Files (*.*)|*.*"
            };

            if (sfd.ShowDialog() == true)
            {
                _path = sfd.FileName;
                //Task t = new Task(DiagramSerializer.j);
                //;
                //new Thread(() =>
                //{
                //    DiagramSerializer.Save(Nodes.ToList(), Edges.ToList(), _path);
                //}).Start();
                DiagramSerializer.Save(Nodes.ToList(), Edges.ToList(), _path);
            }
        }

        /// <summary>
        /// Saves the current program state to the default path.
        /// </summary>
        public void Save()
        {
            if (String.IsNullOrWhiteSpace(_path))
            {
                SaveAs();
            }
            else
                DiagramSerializer.Save(Nodes.ToList(), Edges.ToList(), _path);
        }

        public void Open()
        {
            
            OpenFileDialog ofd = new OpenFileDialog();


            if (ofd.ShowDialog() == false)
            {
                
                ConsolePrinter.Write("path : " + _path);
                return;
            }
            if (!ClearDiagram())
                return;
            _path = ofd.FileName;
            DiagramSerializer.Diagram diagram = DiagramSerializer.Load(_path);
            RestoreDiagram(diagram);

        }

        private void RestoreDiagram(DiagramSerializer.Diagram diagram)
        {
            foreach (Node n in diagram.Nodes)
            {
                NodeViewModel nvm = new NodeViewModel(n.Id, n);
                Nodes.Add(nvm);
            }
        }

        /// <summary>
        /// Given a NodeViewModel it calls a methods in undoRedoController to add it to the ObservableCollection managing the NodeViewModels.
        /// Effectively adding a NodeUserControl to the canvas.
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(NodeViewModel node)
        {
            _undoRedoController.AddAndExecute(new AddNodeCommand(Nodes, node));
        }

        /// <summary>
        /// Sets bools used in MouseUpNode to determine if the clicked node should be removed.
        /// </summary>
        public void RemoveElements()
        {
            _isAddingEdge = false;
            _isRemovingNode = true;

            List<RemoveEdgeCommand> edgeCommands = new List<RemoveEdgeCommand>();
            List<RemoveNodeCommand> nodeCommands = new List<RemoveNodeCommand>();

            NodeViewModel nodeToRemove = null;
            EdgeViewModel edgeToRemove = null;

            foreach (NodeViewModel vm in Nodes)
            {
                if (vm.IsSelected)
                {
                    nodeCommands.Add(new RemoveNodeCommand(Nodes, Edges, vm));
                }

            }
            foreach (EdgeViewModel vm in Edges) 
            {
                if (vm.IsSelected)
                {
                    edgeCommands.Add(new RemoveEdgeCommand(Edges, vm));
                }
            }                     

            if (edgeCommands.Count != 0 || nodeCommands.Count != 0)
            {
                RemoveElementsCommand m = new RemoveElementsCommand(edgeCommands, nodeCommands);
                _undoRedoController.AddAndExecute(m);
            }

            _isRemovingNode = false;
            _canRemove = false;
            _canCancel = false;
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
        //public void ExpandResize(SizeChangedEventArgs e)
        //{
        //    FrameworkElement rect = (FrameworkElement)e.Source;
        //    NodeViewModel node = (NodeViewModel)rect.DataContext;

        //    node.Height = e.NewSize.Height;
        //    node.Width = e.NewSize.Width;
        //    CalculateAnchor(node);
        //}

        #region Edge Adding
        /// <summary>
        /// Sets bools used in MouseUpNode to determine if the clicked node should have an edge added.
        /// </summary>
        public void AddEdge()
        {
            _isRemovingNode = false;
            _isAddingEdge = true;
            _canCancel = true;
        }

        /// <summary>
        /// Invoked by a command, through a binding, corresponding to the edge type (Generalization) to be added.
        /// </summary>
        public void AddGen() { AddEdge(); _edgeType = "GEN"; }

        /// <summary>
        /// Invoked by a command, through a binding, corresponding to the edge type (Association) to be added.
        /// </summary>
        public void AddAss() { AddEdge(); _edgeType = "ASS"; }

        /// <summary>
        /// Invoked by a command, through a binding, corresponding to the edge type (Aggregation) to be added.
        /// </summary>
        public void AddAgg() { AddEdge(); _edgeType = "AGG"; }

        /// <summary>
        /// Invoked by a command, through a binding, corresponding to the edge type (Dependency) to be added.
        /// </summary>
        public void AddDep() { AddEdge(); _edgeType = "DEP"; }

        /// <summary>
        /// Invoked by a command, through a binding, corresponding to the edge type (Composition) to be added.
        /// </summary>
        public void AddCom() { AddEdge(); _edgeType = "COM"; }
        #endregion

        #region Mouse UP DOWN MOVE
        private Point _offsetPosition;
        private double _oldPosX;
        private double _oldPosY;

        /// <summary>
        /// MouseDownNode handles the implementation used when a MouseDown is triggered through an EventToCommand.
        /// </summary>
        /// <param name="e"></param>
        public void MouseDownNode(MouseButtonEventArgs e)
        {
            _isMovingNode = false;
            if (!_isAddingEdge && !_isRemovingNode)
            {
                e.MouseDevice.Target.CaptureMouse();

                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel movingNode = (NodeViewModel)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);

                _offsetPosition = Mouse.GetPosition(canvas);
                _oldPosX = movingNode.X;
                _oldPosY = movingNode.Y;

            }
        }

        /// <summary>
        /// MouseMoveNode handles the implementation used when a MouseMove is triggered through an EventToCommand.
        /// </summary>
        /// <param name="e"></param>
        public void MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null && !_isAddingEdge && !_isRemovingNode)
            {
                _isMovingNode = true;
                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel movingNode = (NodeViewModel)movingRect.DataContext;
                Canvas canvas = FindParentOfType<Canvas>(movingRect);

                Point mousePosition = Mouse.GetPosition(canvas);

                mousePosition.X -= _offsetPosition.X;
                mousePosition.Y -= _offsetPosition.Y;
                
                movingNode.X = _oldPosX + mousePosition.X;
                movingNode.Y = _oldPosY + mousePosition.Y;
                
                _posX = movingNode.X = movingNode.X >= 0 ? movingNode.X : 0;
                _posY = movingNode.Y = movingNode.Y >= 0 ? movingNode.Y : 0;

            }
        }
        
        /// <summary>
        /// MouseUpNode handles the implementation used when a MouseUp is triggered through an EventToCommand.
        /// </summary>
        /// <param name="e"></param>
        public void MouseUpNode(MouseButtonEventArgs e)
        {
            if (_isAddingEdge)
            {
                FrameworkElement rectEnd = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel rectNode = (NodeViewModel)rectEnd.DataContext;

                if (_firstSelectedEdgeEnd == null)
                {
                    _firstSelectedEdgeEnd = rectNode;
                    ConsolePrinter.Write("first edge");
                }
                else if (_firstSelectedEdgeEnd != rectNode)
                {
                    AddEdgeCommand m = new AddEdgeCommand(Edges, _firstSelectedEdgeEnd, rectNode, _edgeType);
                    _undoRedoController.AddAndExecute(m);

                    CalculateAnchor(rectNode);
                    _isAddingEdge = false;
                    _firstSelectedEdgeEnd = null;
                }
            }
            else if (_isRemovingNode)
            {
                //FrameworkElement rectNode = (FrameworkElement)e.MouseDevice.Target;
                //NodeViewModel NodeToRemove = (NodeViewModel)rectNode.DataContext;
                //RemoveNodeCommand m = new RemoveNodeCommand(Nodes, Edges, NodeToRemove);
                //undoRedoController.AddAndExecute(m);
                //isRemovingNode = false;
            }
            else if (_isMovingNode)
            {
                FrameworkElement movingRect = (FrameworkElement)e.MouseDevice.Target;
                NodeViewModel movingNode = (NodeViewModel)movingRect.DataContext;
                //Canvas canvas = FindParentOfType<Canvas>(movingRect);
                //Point mousePosition = Mouse.GetPosition(canvas);

                MoveNodeCommand m = new MoveNodeCommand(movingNode, _posX, _posY, _oldPosX, _oldPosY);
                _undoRedoController.AddAndExecute(m);

                //moveNodePoint = new Point();

                _isMovingNode = false;
            }
            else
            {
                FrameworkElement rect = (FrameworkElement)e.MouseDevice.Target;

                if (Keyboard.Modifiers != ModifierKeys.Control)
                {
                    foreach (NodeViewModel vm in Nodes)
                        vm.IsSelected = false;
                    foreach (EdgeViewModel evm in Edges)
                        evm.IsSelected = false;
                    _canRemove = false;
                }

                if (rect.DataContext is NodeViewModel)
                {
                    NodeViewModel rectNode = (NodeViewModel)rect.DataContext;
                    rectNode.IsSelected = true;
                    _canRemove = true;
                }

                if (rect.DataContext is EdgeViewModel)
                {
                    EdgeViewModel rectEdge = (EdgeViewModel)rect.DataContext;
                    rectEdge.IsSelected = true;
                    _canRemove = true;
                }

                _canCancel = true;
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
        public void CreateNode()
        {
            NodeViewModel newNodeVM = new NodeViewModel(_nodeIdCounter++, new Node());

            var dialog = new CreateNodeWindow();
            //newnode gives med som reference, derfor kan vi bare redigere den direkte i vores dialog
            CreateNodeViewModel dialogViewModel = new CreateNodeViewModel(newNodeVM, dialog);
            dialog.DataContext = dialogViewModel;
            if (dialog.ShowDialog() == true)
                AddNode(newNodeVM);
        }

        public void EditNode()
        {
            string name = null;
            NodeType type = NodeType.ABSTRACT;
            ObservableCollection<string> attributes = null;
            ObservableCollection<string> methods = null;
            NodeViewModel node = null;
            foreach (NodeViewModel n in Nodes)
            {
                if (n.IsSelected)
                {
                    node = n;
                    name = n.Name;
                    type = n.NodeType;
                    attributes = new ObservableCollection<string>(n.Attributes);
                    methods = new ObservableCollection<string>(n.Methods);
                    break;
                }
            }
            var dialog = new CreateNodeWindow();
            CreateNodeViewModel dialogViewModel = new CreateNodeViewModel(ref name, ref type, ref attributes, ref methods, dialog);
            dialog.DataContext = dialogViewModel;
            if (dialog.ShowDialog() == true)
            {
                _undoRedoController.AddAndExecute(new EditNodeCommand(node, Nodes, name, type, attributes.ToList(), methods.ToList()));
            }
                
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
        public void Undo()
        {
            IUndoRedoCommand command = _undoRedoController.getHeadOfUndo();

            _undoRedoController.Undo();
            if (command is MoveNodeCommand)
            {
                CalculateAnchor(((MoveNodeCommand)command).Node);
            }

        }
        public bool CanUndo()
        {
            return _undoRedoController.CanUndo();
        }

        public void Redo()
        {
            IUndoRedoCommand command = _undoRedoController.getHeadOfRedo();

            _undoRedoController.Redo();
            if (command is MoveNodeCommand)
            {
                CalculateAnchor(((MoveNodeCommand)command).Node);
            }
        }

        public bool CanRedo()
        {
            return _undoRedoController.CanRedo();
        }
        #endregion

        public bool CanRemove()
        {
            return _canRemove; 
            
        }

        public bool CanCancel()
        {
            return _canCancel;
        }

        private double _showGrid;
        private bool _gridCheck;

        public double ShowGrid { get
        {
            if (GridCheck) 
                _showGrid = 1.0;
            else 
                _showGrid = 0.0;
            return _showGrid;
        }}
        
        public bool GridCheck { get { return _gridCheck; } set { _gridCheck = value; RaisePropertyChanged("ShowGrid"); } }

        /// <summary>
        /// Finds parent of an element recursively.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        private static T FindParentOfType<T>(DependencyObject o)
            where T : class
        {
            DependencyObject parent = VisualTreeHelper.GetParent(o);
            if (typeof(T).IsAssignableFrom(parent.GetType()))
                return parent is T ? parent as T : null;
            else
                return FindParentOfType<T>(parent);
        }
    }
}