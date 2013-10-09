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
    /// Interaction logic for RibbonUserControl.xaml
    /// </summary>
    public partial class RibbonUserControl : UserControl
    {
        public RibbonUserControl()
        {
            InitializeComponent();
        }
#if true
        private void RibbonButton_Click_1(object sender, RoutedEventArgs e)
        {
            View.CreateClassWindow rt = new View.CreateClassWindow();
            rt.Show();
        }
#endif
    }
}
