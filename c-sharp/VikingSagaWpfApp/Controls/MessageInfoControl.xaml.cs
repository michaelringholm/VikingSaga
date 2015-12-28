﻿using System;
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

namespace VikingSagaWpfApp.Controls
{
    /// <summary>
    /// Interaction logic for MessageInfoControl.xaml
    /// </summary>
    public partial class MessageInfoControl : UserControl
    {
        public MessageInfoControl()
        {
            InitializeComponent();
        }

        public String InfoMessage 
        {
            get { return tbInfo.Text; }

            set { tbInfo.Text = value;  }
        }
    }
}
