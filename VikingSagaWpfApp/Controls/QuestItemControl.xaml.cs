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

namespace VikingSaga.Controls
{
    /// <summary>
    /// Interaction logic for QuestItemControl.xaml
    /// </summary>
    public partial class QuestItemControl : UserControl
    {
        public QuestItemControl()
        {
            InitializeComponent();
        }

        public String QuestTitle 
        {
            get { return tbQuestTitle.Text; }
            set { tbQuestTitle.Text = value; }
        }
        public String Status
        {
            get { return tbStatus.Text; }
            set { tbStatus.Text = value; }
        }
    }
}
