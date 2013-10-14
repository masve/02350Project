using _02350Project.Model;
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
using System.Windows.Input;

namespace _02350Project.ViewModel
{
    public class CreateClassViewModel : ViewModelBase
    {
        public ObservableCollection<String> attributes;
        public ObservableCollection<String> methods;
        public string nodeName;
        public string attribute;
        public string method;
        public string selectedAttribute;
        //public enum nodeFlag { NONE, ABSTRACT, INTERFACE };
        public bool noneCheck;
        public bool abstractCheck;
        public bool interfaceCheck;
       // private nodeFlag selectedFlag;
        

        public ICommand AddAttributeCommand { get; private set; }
        public ICommand AddMethodCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        public ICommand CreateNodeCommand { get; private set; }
        public ICommand CancelNodeCommand { get; private set; }

        public CreateClassViewModel()
        {
            attributes = new ObservableCollection<string>();
            methods = new ObservableCollection<string>();

            NoneCheck = true;

            AddAttributeCommand = new RelayCommand(addAttribute);
            AddMethodCommand = new RelayCommand(addMethod);
            RemoveItemCommand = new RelayCommand(removeItem);
            CreateNodeCommand = new RelayCommand(createNode);
            CancelNodeCommand = new RelayCommand(cancel);
        }

        // Needs NotifyPropertyChanged (RaisePropertyChanged?)
        public string ActualAttribute { get { return attribute; } set { attribute = value; RaisePropertyChanged("ActualAttribute"); } }
        public string ActualMethod { get { return method; } set { method = value; RaisePropertyChanged("ActualMethod"); } }
        public ObservableCollection<string> Attributes { get { return attributes; } set { attributes = value; RaisePropertyChanged("Attributes"); } }
        public ObservableCollection<string> Methods { get { return methods; } set { methods = value; RaisePropertyChanged("Methods"); } }
        public string SelectedItem { get { return selectedAttribute; } set { selectedAttribute = value; } }
        public string NodeName { get { return nodeName; } set { nodeName = value; RaisePropertyChanged("NodeName"); } }
       // public nodeFlag SelectedFlag { get { return selectedFlag; } set { selectedFlag = value; RaisePropertyChanged("SelectedFlag"); } }
        public bool NoneCheck { get { return noneCheck; } set { noneCheck = value; RaisePropertyChanged("NoneCheck"); } }
        public bool AbstractCheck { get { return abstractCheck; } set { abstractCheck = value; RaisePropertyChanged("AbstractCheck"); } }
        public bool InterfaceCheck { get { return interfaceCheck; } set { interfaceCheck = value; RaisePropertyChanged("InterfaceCheck"); } }


        public void addAttribute()
        {
            // If empty don't add
            if (ActualAttribute == "") return;
            // If already in collection don't add
            if (Attributes.Contains(ActualAttribute)) return;

            Attributes.Add(ActualAttribute);

            ActualAttribute = "";

            //Other.ConsolePrinter.WriteToConsole("none: " + NoneCheck + ", abstract: " + AbstractCheck + ", interface: " + InterfaceCheck);
        }

        public void addMethod()
        {
            // If empty don't add
            if (ActualMethod == "") return;
            // If already in collection don't add
            if (Methods.Contains(ActualMethod)) return;

            Methods.Add(ActualMethod);

            ActualMethod = "";
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
            resetDialog();
            MessengerInstance.Send<int>(1000, "CloseClassDialogView");
        }

        public void createNode()
        {
            MessengerInstance.Send<Node>(new Node(){Name = NodeName, NoneFlag = NoneCheck, AbstractFlag = AbstractCheck, 
                                                    InterfaceFlag = InterfaceCheck, Attributes = this.Attributes.ToList<string>(), 
                                                    Methods = this.Methods.ToList<string>()}, "key1");

            resetDialog();

            MessengerInstance.Send<int>(1000, "CloseClassDialogView");
            //MessengerInstance.Send(new GenericMessage<object>("Hello viewmodel"), "key3");
            //MessengerInstance.Send<string>(ClassName, "key2");
            //MessengerInstance.Send(new DialogMessage())
        }

        public void resetDialog()
        {
            Methods.Clear();
            Attributes.Clear();
            NodeName = "";
            NoneCheck = true;
        }
    }
}
