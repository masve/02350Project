﻿using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using System.Runtime.InteropServices;

namespace _02350Project.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CreateClassWindow : Window
    {
        public CreateClassWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<int>(this, "CloseClassDialogView", a => closeView(a));
        }

        private void closeView(int a)
        {
            if (a == 1000)
            {
                this.Close();
            }
        }
    }
}
