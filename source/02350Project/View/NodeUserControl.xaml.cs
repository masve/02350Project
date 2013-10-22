using _02350Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _02350Project.View
{
    /// <summary>
    /// Interaction logic for NodeUserControl.xaml
    /// </summary>
    public partial class NodeUserControl : UserControl
    {
        public NodeUserControl()
        {
            InitializeComponent();
        }

        private void Node_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //SizeChangedEvent();
            Other.ConsolePrinter.Write("helloinit");
        }
    }
}
