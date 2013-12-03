using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Ioc;
using _02350Project.Model;

namespace _02350Project.ViewModel
{
    public class CreateNodeViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Private Fields
        private readonly Window _window;

        private ObservableCollection<String> _attributes;
        private ObservableCollection<String> _methods;

        private string _nodeName;
        private string _attribute;
        private string _method;
        private string _selectedAttribute;
        private bool _noneCheck;
        private bool _abstractCheck;
        private bool _interfaceCheck;
        private enum EntryType { ATTRIBUTE, METHOD };
        private string _nodeType;
        private ObservableCollection<string> _nodeTypes;
        private bool _canCreate;
        private bool _canAddAttribute;
        private bool _canAddMethod;

        private readonly NodeViewModel _node;
        private enum State { CREATE, EDIT };

        private State _state;
        #endregion

        #region Public Fields
        public ICommand AddAttributeCommand { get; private set; }
        public ICommand AddMethodCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        public ICommand CreateNodeCommand { get; private set; }
        public ICommand EditNodeCommand { get; private set; }
        public ICommand CancelNodeCommand { get; private set; }
        #endregion

        #region Constructors
        [PreferredConstructor]
        public CreateNodeViewModel(NodeViewModel node, Window window)
        {
            _state = State.CREATE;
            _window = window;
            _node = node;

            _attributes = new ObservableCollection<string>();
            _methods = new ObservableCollection<string>();

            NoneCheck = true;
            ConstructorInit();
            SelectedChoice = "Class";
        }

        /// <summary>
        /// Initializes commands commmon for both constructors.
        /// </summary>
        public void ConstructorInit()
        {
            _nodeTypes = new ObservableCollection<string>();
            RadioChoices.Add("Class");
            RadioChoices.Add("Interface");
            RadioChoices.Add("Abstract");
            AddAttributeCommand = new RelayCommand(AddAttribute);
            AddMethodCommand = new RelayCommand(AddMethod);
            RemoveItemCommand = new RelayCommand(RemoveItem);
            CreateNodeCommand = new RelayCommand(CreateNode);
            EditNodeCommand = new RelayCommand(EditNode);
            CancelNodeCommand = new RelayCommand(Cancel);
        }

        private string _editNodeName;
        private NodeType _editNodeType;
        private ObservableCollection<string> _editNodeAttributes;
        private ObservableCollection<string> _editNodeMethods;

        public CreateNodeViewModel(Window window, NodeViewModel nodeVM)
        {
            _window = window;
            ConstructorInit();
            _state = State.EDIT;

            _node = nodeVM;
            NodeName = nodeVM.Name;
            EnumToListboxRadioConverter(nodeVM.NodeType);
            Attributes = new ObservableCollection<string>(nodeVM.Attributes);
            Methods = new ObservableCollection<string>(nodeVM.Methods);

            //FillFields();
        }
        //public CreateNodeViewModel(ref NodeViewModel)
        #endregion

        #region Properties
        public string ActualAttribute
        {
            get { return _attribute; }
            set
            {
                _attribute = value;
                CanAddAttribute = !String.IsNullOrWhiteSpace(ActualAttribute);
                RaisePropertyChanged("ActualAttribute");
            }
        }
        public string ActualMethod
        {
            get { return _method; }
            set
            {
                _method = value;
                CanAddMethod = !String.IsNullOrWhiteSpace(ActualMethod);
                RaisePropertyChanged("ActualMethod");
            }
        }
        public ObservableCollection<string> Attributes { get { return _attributes; } set { _attributes = value; RaisePropertyChanged("Attributes"); } }
        public ObservableCollection<string> Methods { get { return _methods; } set { _methods = value; RaisePropertyChanged("Methods"); } }
        public string SelectedItem { get { return _selectedAttribute; } set { _selectedAttribute = value; } }
        public string NodeName { get { return _nodeName; } set { _nodeName = value; RaisePropertyChanged("NodeName"); } }
        public bool NoneCheck { get { return _noneCheck; } set { _noneCheck = value; RaisePropertyChanged("NoneCheck"); } }
        public bool AbstractCheck { get { return _abstractCheck; } set { _abstractCheck = value; RaisePropertyChanged("AbstractCheck"); } }
        public bool InterfaceCheck { get { return _interfaceCheck; } set { _interfaceCheck = value; RaisePropertyChanged("InterfaceCheck"); } }
        public string SelectedChoice { get { return _nodeType; } set { _nodeType = value; RaisePropertyChanged("SelectedChoice"); } }
        public ObservableCollection<string> RadioChoices { get { return _nodeTypes; } set { _nodeTypes = value; RaisePropertyChanged("RadioChoices"); } }
        public bool CanCreate { get { return _canCreate; } set { _canCreate = value; RaisePropertyChanged("CanCreate"); } }
        public bool CanAddAttribute { get { return _canAddAttribute; } set { _canAddAttribute = value; RaisePropertyChanged("CanAddAttribute"); } }
        public bool CanAddMethod { get { return _canAddMethod; } set { _canAddMethod = value; RaisePropertyChanged("CanAddMethod"); } }
        public NodeType NodeType { get; set; }
        public Visibility CreateVisibility { get { if (_state == State.CREATE) return Visibility.Visible; return Visibility.Collapsed; } }
        public Visibility EditVisibility { get { if (_state == State.EDIT) return Visibility.Visible; return Visibility.Collapsed; } }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the content of ActualAttribute to Attributes
        /// </summary>
        public void AddAttribute()
        {
            if (!String.IsNullOrWhiteSpace(ActualAttribute))
                AddEntry(ActualAttribute, EntryType.ATTRIBUTE);
            ActualAttribute = "";
        }

        /// <summary>
        /// Adds the content of ActualMethod to Methods
        /// </summary>
        public void AddMethod()
        {
            if (!String.IsNullOrWhiteSpace(ActualMethod))
                AddEntry(ActualMethod, EntryType.METHOD);
            ActualMethod = "";
        }

        /// <summary>
        /// Appends entry with default visibility if not mentioned.
        /// Public(+) for attributes.
        /// Private(-) for methods.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="type"></param>
        private void AddEntry(string entry, EntryType type)
        {
            switch (type)
            {
                case EntryType.ATTRIBUTE:
                    if (entry[0].Equals('-')) Attributes.Add(entry);
                    else if (entry[0].Equals('+')) Attributes.Add(entry);
                    else Attributes.Add("- " + entry);
                    break;
                case EntryType.METHOD:
                    if (entry[0].Equals('-')) Methods.Add(entry);
                    else if (entry[0].Equals('+')) Methods.Add(entry);
                    else Methods.Add("+ " + entry);
                    break;
            }
        }

        /// <summary>
        /// Converts the Selected Listbox Choice to the node type enum.
        /// </summary>
        public void ListboxToEnumRadioConverter()
        {
            if (SelectedChoice.Equals("Abstract"))
                NodeType = NodeType.ABSTRACT;
            else if (SelectedChoice.Equals("Interface"))
                NodeType = NodeType.INTERFACE;
            else
                NodeType = NodeType.CLASS;
        }

        /// <summary>
        /// Converts the given enum to Selected Listbox Choice.
        /// </summary>
        public void EnumToListboxRadioConverter(NodeType type)
        {
            switch (type)
            {
                case NodeType.ABSTRACT:
                    SelectedChoice = "Abstract";
                    break;
                case NodeType.INTERFACE:
                    SelectedChoice = "Interface";
                    break;
                case NodeType.CLASS:
                    SelectedChoice = "Class";
                    break;
            }

        }

        /// <summary>
        /// Removes the selected item in either the attribute listbox or method listbox
        /// </summary>
        public void RemoveItem()
        {
            if (SelectedItem == "") return;
            Attributes.Remove(SelectedItem);
            Methods.Remove(SelectedItem);
        }

        /// <summary>
        /// Closes the window and returns false to the dialog invoker.
        /// </summary>
        public void Cancel()
        {
            _window.DialogResult = false;
        }

        /// <summary>
        /// Sets the node properties and returns true to the dialog invoker.
        /// </summary>
        public void CreateNode()
        {
            ListboxToEnumRadioConverter();

            _node.Name = NodeName;
            _node.NodeType = NodeType;
            _node.Methods = Methods.ToList();
            _node.Attributes = Attributes.ToList();

            _window.DialogResult = true;
        }

        /// <summary>
        /// Sets the node properties and returns true to the dialog invoker.
        /// </summary>
        public void EditNode()
        {
            ListboxToEnumRadioConverter();

            _node.Name = NodeName;
            _node.NodeType = NodeType;
            _node.Methods = Methods.ToList();
            _node.Attributes = Attributes.ToList();

            _window.DialogResult = true;
        }
        #endregion

        #region IDataErrorInfo Members
        public string Error { get; private set; }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "NodeName")
                {
                    if (String.IsNullOrWhiteSpace(NodeName))
                    {
                        Error = "Name cannot be empty.";
                        CanCreate = false;
                    }
                    else
                    {
                        Error = null;
                        CanCreate = true;
                    }
                }
                return Error;
            }
        }
        #endregion
    }
}
