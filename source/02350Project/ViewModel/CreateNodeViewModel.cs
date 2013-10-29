using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _02350Project.ViewModel
{
    public class CreateNodeViewModel : ViewModelBase
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

        private NodeViewModel node;


        public ICommand AddAttributeCommand { get; private set; }
        public ICommand AddMethodCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        public ICommand CreateNodeCommand { get; private set; }
        public ICommand CancelNodeCommand { get; private set; }

        private string nodeType;
        private ObservableCollection<string> nodeTypes;

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

        public string ActualAttribute { get { return attribute; } set { attribute = value; RaisePropertyChanged("ActualAttribute"); } }
        public string ActualMethod { get { return method; } set { method = value; RaisePropertyChanged("ActualMethod"); } }
        public ObservableCollection<string> Attributes { get { return attributes; } set { attributes = value; RaisePropertyChanged("Attributes"); } }
        public ObservableCollection<string> Methods { get { return methods; } set { methods = value; RaisePropertyChanged("Methods"); } }
        public string SelectedItem { get { return selectedAttribute; } set { selectedAttribute = value; } }
        public string NodeName { get { return nodeName; } set { nodeName = value; RaisePropertyChanged("NodeName"); } }
        public bool NoneCheck { get { return noneCheck; } set { noneCheck = value; RaisePropertyChanged("NoneCheck"); } }
        public bool AbstractCheck { get { return abstractCheck; } set { abstractCheck = value; RaisePropertyChanged("AbstractCheck"); } }
        public bool InterfaceCheck { get { return interfaceCheck; } set { interfaceCheck = value; RaisePropertyChanged("InterfaceCheck"); } }


        public string SelectedChoice { get { return nodeType; } set { nodeType = value; RaisePropertyChanged("selectedradio"); } }
        public ObservableCollection<string> RadioChoices { get { return nodeTypes; } set { nodeTypes = value; RaisePropertyChanged("Choices"); } }

        public void addAttribute()
        {
            // If empty don't add
            if (ActualAttribute == "") return;
            // If already in collection don't add
            if (Attributes.Contains(ActualAttribute)) return;

            addEntry(ActualAttribute, entryType.ATTRIBUTE);

            ActualAttribute = "";
        }

        public void addMethod()
        {
            // If empty don't add
            if (ActualMethod == "") return;
            // If already in collection don't add
            if (Methods.Contains(ActualMethod)) return;

            addEntry(ActualMethod, entryType.METHOD);

            ActualMethod = "";
        }

        /*
         * Appends entry with default visibility if not mentioned.
         * Public(+) for attributes.
         * Private(-) for methods.
         */
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

        /*
         * Used to convert from the new solution from the gui back to the model.
         */
        public void listboxToBoolRadioConverter()
        {
            if (SelectedChoice.Equals("Default"))
            {
                NoneCheck = true;
                AbstractCheck = false;
                interfaceCheck = false;
            }
            else if (SelectedChoice.Equals("Abstract"))
            {
                NoneCheck = false;
                AbstractCheck = true;
                interfaceCheck = false;
            }
            else if (SelectedChoice.Equals("Interface"))
            {
                NoneCheck = false;
                AbstractCheck = false;
                interfaceCheck = true;
            }
            else
            {
                NoneCheck = false;
                AbstractCheck = false;
                interfaceCheck = false;
            }
        }

        // Better implementation
        public void removeItem()
        {
            if (SelectedItem == "") return;
            Attributes.Remove(SelectedItem);
            Methods.Remove(SelectedItem);
        }

        public void cancel()
        {
            //resetDialog();
            //MessengerInstance.Send<int>(1000, "CloseNodeDialog");
            window.DialogResult = false;
        }

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

            //MessengerInstance.Send<Node>(new Node()
            //{

            //    Name = NodeName,
            //    NoneFlag = NoneCheck,
            //    AbstractFlag = AbstractCheck,
            //    InterfaceFlag = InterfaceCheck,
            //    Attributes = this.Attributes.ToList<string>(),
            //    Methods = this.Methods.ToList<string>()
            //}, "key1");

            //resetDialog();

            //MessengerInstance.Send<int>(1000, "CloseNodeDialog");
        }

        public void resetDialog()
        {
            NoneCheck = true;
            SelectedChoice = "Default";
            Methods.Clear();
            Attributes.Clear();
            ActualAttribute = "";
            ActualMethod = "";
            NodeName = "";
        }
    }
}
