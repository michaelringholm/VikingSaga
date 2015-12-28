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
    /// Interaction logic for ButtonControl.xaml
    /// </summary>
    public partial class ButtonControl : UserControl
    {
        public ButtonControl()
        {
            InitializeComponent();
        }

        public String Text 
        {
            get { return ButtonText.Text; }
            set { ButtonText.Text = value; }
        }

        public double ButtonWidth
        {
            get { return MainButton.Width; }
            set { MainButton.Width = value; }
        }

        public double ButtonHeight
        {
            get { return MainButton.Height; }
            set { MainButton.Height = value; }
        }
    }
}
