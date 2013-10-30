using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _02350Project.ViewModel
{
    public class CreateNodeViewModel : ViewModelBase, IDataErrorInfo
    {
        private Window window;

        private ObservableCollection<String> attributes;
        private ObservableCollection<String> methods;
        
        private string nodeName;
        private string attribute;
        private string method;
        private string selectedAttribute;
        private bool noneCheck;
        private bool abstractCheck;
        private bool interfaceCheck;
        private enum entryType { ATTRIBUTE, METHOD };
        private string nodeType;
        private ObservableCollection<string> nodeTypes;
        private bool canCreate;
        private bool canAddAttribute;
        private bool canAddMethod;

        private NodeViewModel node;

        public ICommand AddAttributeCommand { get; private set; }
        public ICommand AddMethodCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        public ICommand CreateNodeCommand { get; private set; }
        public ICommand CancelNodeCommand { get; private set; }

        #region Constructor
        public CreateNodeViewModel(NodeViewModel _node, Window _window)
        {
            Other.ConsolePrinter.Write("hello");
            window = _window;
            node = _node;

            attributes = new ObservableCollection<string>();
            methods = new ObservableCollection<string>();
            nodeTypes = new ObservableCollection<string>();

            NoneCheck = true;

            RadioChoices.Add("Default");
            RadioChoices.Add("Interface");
            RadioChoices.Add("Abstract");
            SelectedChoice = "Default";

            AddAttributeCommand = new RelayCommand(addAttribute);
            AddMethodCommand = new RelayCommand(addMethod);
            RemoveItemCommand = new RelayCommand(removeItem);
            CreateNodeCommand = new RelayCommand(createNode);
            CancelNodeCommand = new RelayCommand(cancel);
        }
        #endregion

        #region Properties
        public string ActualAttribute 
        { 
            get { return attribute; } 
            set 
            { 
                attribute = value; 
                if (String.IsNullOrWhiteSpace(ActualAttribute))
                    CanAddAttribute = false; 
                else 
                    CanAddAttribute = true;
                RaisePropertyChanged("ActualAttribute"); 
            } 
        }
        public string ActualMethod 
        {
            get { return method; } 
            set 
            { 
                method = value;
                if (String.IsNullOrWhiteSpace(ActualMethod))
                    CanAddMethod = false;
                else
                    CanAddMethod = true;
                RaisePropertyChanged("ActualMethod"); 
            } 
        }
        public ObservableCollection<string> Attributes { get { return attributes; } set { attributes = value; RaisePropertyChanged("Attributes"); } }
        public ObservableCollection<string> Methods { get { return methods; } set { methods = value; RaisePropertyChanged("Methods"); } }
        public string SelectedItem { get { return selectedAttribute; } set { selectedAttribute = value; } }
        public string NodeName { get { return nodeName; } set { nodeName = value; RaisePropertyChanged("NodeName"); } }
        public bool NoneCheck { get { return noneCheck; } set { noneCheck = value; RaisePropertyChanged("NoneCheck"); } }
        public bool AbstractCheck { get { return abstractCheck; } set { abstractCheck = value; RaisePropertyChanged("AbstractCheck"); } }
        public bool InterfaceCheck { get { return interfaceCheck; } set { interfaceCheck = value; RaisePropertyChanged("InterfaceCheck"); } }
        public string SelectedChoice { get { return nodeType; } set { nodeType = value; RaisePropertyChanged("selectedradio"); } }
        public ObservableCollection<string> RadioChoices { get { return nodeTypes; } set { nodeTypes = value; RaisePropertyChanged("Choices"); } }
        public bool CanCreate { get { return canCreate; } set { canCreate = value; RaisePropertyChanged("CanCreate"); } }
        public bool CanAddAttribute { get { return canAddAttribute; } set { canAddAttribute = value; RaisePropertyChanged("CanAddAttribute"); } }
        public bool CanAddMethod { get { return canAddMethod; } set { canAddMethod = value; RaisePropertyChanged("CanAddMethod"); } }
        #endregion
        
        /// <summary>
        /// Adds the content of ActualAttribute to Attributes
        /// </summary>
        public void addAttribute()
        {
            if (!String.IsNullOrWhiteSpace(ActualAttribute))
                addEntry(ActualAttribute, entryType.ATTRIBUTE);
            ActualAttribute = "";
        }

        /// <summary>
        /// Adds the content of ActualMethod to Methods
        /// </summary>
        public void addMethod()
        {
            if (!String.IsNullOrWhiteSpace(ActualMethod))
                addEntry(ActualMethod, entryType.METHOD);
            ActualMethod = "";
        }

        /// <summary>
        /// Appends entry with default visibility if not mentioned.
        /// Public(+) for attributes.
        /// Private(-) for methods.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="type"></param>
        private void addEntry(string entry, entryType type)
        {
            switch (type)
            {
                case entryType.ATTRIBUTE:
                    if (entry[0].Equals('-')) Attributes.Add(entry);
                    else if (entry[0].Equals('+')) Attributes.Add(entry);
                    else Attributes.Add("- " + entry);
                    break;
                case entryType.METHOD:
                    if (entry[0].Equals('-')) Methods.Add(entry);
                    else if (entry[0].Equals('+')) Methods.Add(entry);
                    else Methods.Add("+ " + entry);
                    break;
            }
        }

        /// <summary>
        /// Converts the Selected Listbox Choice to the bool properties.
        /// </summary>
        public void listboxToBoolRadioConverter()
        {
            if (SelectedChoice.Equals("Default"))
            {
                NoneCheck = true;
                AbstractCheck = false;
                InterfaceCheck = false;
            }
            else if (SelectedChoice.Equals("Abstract"))
            {
                NoneCheck = false;
                AbstractCheck = true;
                InterfaceCheck = false;
            }
            else if (SelectedChoice.Equals("Interface"))
            {
                NoneCheck = false;
                AbstractCheck = false;
                InterfaceCheck = true;
            }
            else
            {
                NoneCheck = false;
                AbstractCheck = false;
                interfaceCheck = false;
            }
        }

        /// <summary>
        /// Removes the selected item in either the attribute listbox or method listbox
        /// </summary>
        public void removeItem()
        {
            if (SelectedItem == "") return;
            Attributes.Remove(SelectedItem);
            Methods.Remove(SelectedItem);
        }

        /// <summary>
        /// Closes the window and returns false to the dialog invoker.
        /// </summary>
        public void cancel()
        {
            window.DialogResult = false;
        }

        /// <summary>
        /// Sets the node properties and returns true to the dialog invoker.
        /// </summary>
        public void createNode()
        {
            listboxToBoolRadioConverter();

            node.Name = NodeName;
            node.NoneFlag = NoneCheck;
            node.AbstractFlag = AbstractCheck;
            node.InterfaceFlag = InterfaceCheck;
            node.Methods = this.Methods.ToList<string>();
            node.Attributes = this.Attributes.ToList<string>();

            window.DialogResult = true;
        }

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

         //if (columnName == "ActualAttribute")
         //       {
         //           if (String.IsNullOrWhiteSpace(ActualAttribute))
         //           {
         //               Error = "Attribute field cannot be empty.";
         //               CanAddAttribute = false;
         //           }
         //           else 
         //           {
         //               Error = null;
         //               Can
         //           }
         //       }
    }
}
